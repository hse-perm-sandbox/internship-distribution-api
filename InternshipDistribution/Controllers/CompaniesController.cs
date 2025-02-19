using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;

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
            return await _context.Companies.Where(c => c.DeletedAt == null).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null || company.DeletedAt != null)
            {
                return NotFound();
            }

            return company;
        }

        [HttpPost]
        public async Task<ActionResult<Company>> CreateCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            var existingCompany = await _context.Companies.FindAsync(id);
            if (existingCompany == null || existingCompany.DeletedAt != null)
            {
                return NotFound();
            }

            existingCompany.Name = company.Name;
            existingCompany.Description = company.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var isDeleted = await _repository.SoftDeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
