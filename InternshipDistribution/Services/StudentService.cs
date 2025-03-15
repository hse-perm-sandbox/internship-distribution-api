using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;

namespace InternshipDistribution.Services
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;
        private readonly UserRepository _userRepository;
        private readonly FileStorageService _fileStorageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentService(StudentRepository studentRepository, UserRepository userRepository, FileStorageService fileStorageService, IHttpContextAccessor httpContextAccessor)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<Student?> GetStudentById(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new KeyNotFoundException($"Student с Id = {id} не найден");

            return student;
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
    }
}
