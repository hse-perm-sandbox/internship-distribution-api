using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace InternshipDistribution.Services
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;
        private readonly UserRepository _userRepository;
        private readonly FileStorageService _fileStorageService;

        public StudentService(StudentRepository studentRepository, UserRepository userRepository, FileStorageService fileStorageService)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
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
                throw new BadHttpRequestException($"Student с Id = {id} не найден", StatusCodes.Status404NotFound);

            return student;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<string> UploadResumeAsync(int studentId, IFormFile file)
        {
            var student = await GetStudentById(studentId);

            var fileName = await _fileStorageService.SaveResume(file, student);
            await _studentRepository.SaveResumeNameAsync(studentId, fileName);

            return fileName;
        }

        public async Task<bool> DeleteResumeAsync(int studentId)
        {
            var student = await GetStudentById(studentId);

            if (student.Resume == null)
                throw new BadHttpRequestException("Резюме не найдено", StatusCodes.Status404NotFound);

            await _studentRepository.DeleteResumeNameAsync(student);

            var isDeleted = _fileStorageService.DeleteResume(student.Resume);
            if (!isDeleted)
                throw new BadHttpRequestException("Не удалось удалить файл", StatusCodes.Status400BadRequest);

            return true;
        }
    }
}
