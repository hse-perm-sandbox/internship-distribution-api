using InternshipDistribution.Dto;
using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.Controllers
{
    [ApiController]
    [Route("api/applications")]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationService _applicationService;

        public ApplicationsController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> CreateApplication(ApplicationInput dto)
        {
            try
            {
                var application = await _applicationService.CreateApplicationAsync(dto);
                return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetApplication(int id)
        {
            var application = await _applicationService.GetApplicationAsync(id);
            return application != null ? Ok(application) : NotFound();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllApplications()
        {
            var apps = await _applicationService.GetAllApplicationsAsync();
            return Ok(apps);
        }

        [HttpGet("by-student/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            try
            {
                var application = await _applicationService.GetByStudentIdAsync(studentId);
                return Ok(application);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id}/priorities")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> UpdatePriorities(int id,[FromBody] UpdatePrioritiesInput dto)
        {
            try
            {
                var application = await _applicationService.UpdatePrioritiesAsync(id, dto);
                return Ok(application);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "RequireManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            try
            {
                var result = await _applicationService.DeleteApplicationAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
