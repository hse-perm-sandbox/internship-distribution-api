using Humanizer;
using InternshipDistribution.Dto;
using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using System.Xml.Linq;

namespace InternshipDistribution.Services
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;
        private readonly UserRepository _userRepository;
        private readonly FileStorageService _fileStorageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthService _authService;
        private readonly ApplicationRepository _applicationRepository;
        private readonly BCryptPasswordHasher _passwordHasher;

        public StudentService(StudentRepository studentRepository, UserRepository userRepository,
            FileStorageService fileStorageService, IHttpContextAccessor httpContextAccessor,
            AuthService authService, BCryptPasswordHasher passwordHasher,
           ApplicationRepository applicationRepository)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _passwordHasher = passwordHasher;
            _applicationRepository = applicationRepository;
        }

        public async Task<Student?> CreateStudentAsync(StudentInput studentInput)
        {
            var user = await _userRepository.GetByIdAsync(studentInput.UserId);
            if (user == null)
                throw new BadHttpRequestException($"User с Id = {studentInput.UserId} не найден", StatusCodes.Status404NotFound);

            if (user.IsManager == true)
                throw new BadHttpRequestException($"User с Id = {studentInput.UserId} менаджер, он не может быть студентом", StatusCodes.Status400BadRequest);

            var student = new Student
            {
                UserId = studentInput.UserId,
                Name = studentInput.Name,
                Lastname = studentInput.Lastname,
                Fathername = studentInput.Fathername,
            };

            return await _studentRepository.AddAsync(student);
        }

        public async Task<BulkCreateStudentsResponse> BulkCreateStudentsAsync(List<StudentInputWithEmail> studentsInput)
        {
            var students = new BulkCreateStudentsResponse();

            foreach (var input in studentsInput)
            {
                try
                {
                    var registrationUser = await _authService.RegisterStudentWithGeneratedPassword(input);

                    if (registrationUser == null)
                    {
                        students.FailedEmails.Add(input.Email);
                        continue;
                    }

                    var student = new Student
                    {
                        UserId = registrationUser.User.Id,
                        Name = input.Name,
                        Lastname = input.Lastname,
                        Fathername = input.Fathername
                    };

                    await _studentRepository.AddAsync(student);

                    students.CreatedStudents.Add(new StudentWithPasswordResult
                    {
                        StudentId = student.Id,
                        Email = registrationUser.User.Email,
                        GeneratedPassword = registrationUser.GeneratedPassword,
                        Name = student.Name,
                        Lastname = student.Lastname,
                        Fathername = student.Fathername
                    });
                }
                catch (Exception ex)
                {
                    students.FailedEmails.Add(input.Email);
                }
            }

            return students;
        }

        public async Task<Student?> GetStudentById(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new KeyNotFoundException($"Student с Id = {id} не найден");

            return student;
        }

        public async Task<Student?> GetStudentByUserIdAsync()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var stusent = await _studentRepository.GetStudentByUserIdAsync(userId);

            if (stusent == null)
                throw new KeyNotFoundException($"Student с userId = {userId} не найден");

            return stusent;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<string> UploadResumeAsync(int studentId, IFormFile file)
        {
            await CheckAccess(studentId);

            var student = await GetStudentById(studentId);

            var fileName = await _fileStorageService.SaveResume(file, student);
            await _studentRepository.SaveResumeNameAsync(studentId, fileName);

            return fileName;
        }

        public async Task<bool> DeleteResumeAsync(int studentId)
        {
            await CheckAccess(studentId);

            var student = await GetStudentById(studentId);

            if (student.Resume == null)
                throw new BadHttpRequestException("Резюме не найдено", StatusCodes.Status404NotFound);

            var isDeleted = _fileStorageService.DeleteResume(student.Resume);
            if (!isDeleted)
                throw new BadHttpRequestException("Не удалось удалить файл", StatusCodes.Status400BadRequest);

            await _studentRepository.DeleteResumeNameAsync(student);

            return true;
        }

        public async Task CheckAccess(int studentId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                throw new UnauthorizedAccessException("Пользователь не аутентифицирован");

            var currentUser = await _userRepository.GetByIdAsync(int.Parse(currentUserId));
            var student = await GetStudentById(studentId);

            if (!currentUser.IsManager && student.UserId != int.Parse(currentUserId))
                throw new UnauthorizedAccessException("Доступ запрещен");
        }

        public StudentOutputDto StudentToStudentOutPutDto(Student student)
        {
            StudentOutputDto studentDto = new StudentOutputDto
            {
                Id = student.Id,
                Name = student.Name,
                Lastname = student.Lastname,
                Fathername = student.Fathername,
                UserId = student.UserId,
                ResumeFileName = student.Resume
            };

            return studentDto;
        }

        public async Task<IEnumerable<StudentWithApplication>> GetAllStudentsWithApplicationsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            var result = new List<StudentWithApplication>();

            foreach (var student in students)
            {
                DistributionApplication? application = null;

                application = await _applicationRepository.GetByStudentIdAsync(student.Id);

                result.Add(new StudentWithApplication
                {
                    Id = student.Id,
                    Name = student.Name,
                    Lastname = student.Lastname,
                    Fathername = student.Fathername,
                    UserId = student.UserId,
                    ResumeFileName = student.Resume,
                    Priority1CompanyId = application?.Priority1CompanyId,
                    Priority2CompanyId = application?.Priority2CompanyId,
                    Priority3CompanyId = application?.Priority3CompanyId,
                    Priority1Status = application?.Priority1Status.Humanize() ?? "NotSelected",
                    Priority2Status = application?.Priority2Status.Humanize() ?? "NotSelected",
                    Priority3Status = application?.Priority3Status.Humanize() ?? "NotSelected",
                    Status = application?.Status.Humanize() ?? "NotCreated"
                });
            }

            return result;
        }
    }
}
