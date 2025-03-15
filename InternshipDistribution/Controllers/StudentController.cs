using InternshipDistribution.Dto;
using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;

namespace InternshipDistribution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly FileStorageService _fileStorageService;

        public StudentController(StudentService studentService, FileStorageService fileStorageService)
        {
            _studentService = studentService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<StudentOutputDto>> Create(StudentInput input)
        {
            var student = await _studentService.CreateStudentAsync(input);

            var outputDto = _studentService.StudentToStudentOutPutDto(student);

            return CreatedAtAction(nameof(Get), new { id = outputDto.Id }, outputDto);
        }

        [HttpGet("me")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<ActionResult<StudentOutputDto>> GetCurrentStudent()
        {
            try
            {
                var student = await _studentService.GetStudentByUserIdAsync();

                return _studentService.StudentToStudentOutPutDto(student);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await _studentService.GetStudentById(id);

            if (student == null)
                return NotFound();

            return student;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            var users = await _studentService.GetAllStudentsAsync();
            return Ok(users);
        }

        [HttpPost("{student_id}/resume")]
        [Authorize]
        public async Task<IActionResult> UploadResume(int student_id, IFormFile file)
        {
            try
            {
                var fileName = await _studentService.UploadResumeAsync(student_id, file);
                return Ok(new { FileName = fileName });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{student_id}/resume")]
        [Authorize]
        public async Task<IActionResult> DownloadResume(int student_id)
        {
            try
            {
                await _studentService.CheckAccess(student_id);
                var student = await _studentService.GetStudentById(student_id);

                if (student?.Resume == null)
                    return NotFound("Резюме не найдено");

                var stream = _fileStorageService.GetResume(student.Resume);
                return File(stream, "application/pdf", $"{student.Resume}");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Внутренняя ошибка сервера" });
            }
        }

        [HttpDelete("{student_id}/resume")]
        [Authorize]
        public async Task<IActionResult> DeleteResume(int student_id)
        {
            try
            {
                await _studentService.DeleteResumeAsync(student_id);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Внутренняя ошибка сервера" });
            }
        }
    }
}
