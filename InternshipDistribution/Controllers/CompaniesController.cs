using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using InternshipDistribution.DTO;

namespace InternshipDistribution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : Controller
    {
        private readonly BaseRepository<Company> _repository;
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _repository = new BaseRepository<Company>(context);
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            var companies = await _repository.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _repository.GetByIdAsync(id);

            if (company == null)
                return NotFound();

            return company;
        }

        [HttpPost]
        public async Task<ActionResult<Company>> CreateCompany(CompanyDto companyDto)
        {
            var company = new Company
            {
                Name = companyDto.Name,
                Description = companyDto.Description
            };

            var createdCompany = await _repository.AddAsync(company);

            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyDto companyDto)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null)
                return NotFound();

            company.Name = companyDto.Name;
            company.Description = companyDto.Description;

            var updated = await _repository.UpdateAsync(company);
            if (!updated)
                return BadRequest("Update failed");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var isDeleted = await _repository.SoftDeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
