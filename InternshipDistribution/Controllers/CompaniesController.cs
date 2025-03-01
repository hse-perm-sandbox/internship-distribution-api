﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using InternshipDistribution.Services;
using InternshipDistribution.InputModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace InternshipDistribution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly BaseRepository<Company> _repository;
        private readonly CompanyService _companyService;

        public CompaniesController(ApplicationDbContext context)
        {
            _repository = new BaseRepository<Company>(context);
            _companyService = new CompanyService();
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
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<Company>> CreateCompany(CompanyInput companyDto)
        {
            var company = new Company();

            _companyService.UpdateCompanyFromDto(company, companyDto);

            var createdCompany = await _repository.AddAsync(company);

            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireManager")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyInput companyDto)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null)
                return NotFound();

            _companyService.UpdateCompanyFromDto(company, companyDto);

            var updated = await _repository.UpdateAsync(company);
            if (!updated)
                return BadRequest("Update failed");

            return Ok(company);
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "RequireManager")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] JsonPatchDocument<CompanyInput> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Invalid patch document");

            var company = await _repository.GetByIdAsync(id);
            if (company == null)
                return NotFound();

            _companyService.UpdateCompanyFromJsonPatchDocument(company, patchDoc);

            var updated = await _repository.UpdateAsync(company);
            if (!updated)
                return BadRequest("Update failed");

            return Ok(company);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireManager")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var isDeleted = await _repository.SoftDeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
