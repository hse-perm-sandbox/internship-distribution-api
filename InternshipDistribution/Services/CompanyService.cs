using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace InternshipDistribution.Services
{
    public class CompanyService
    {
        public void UpdateCompanyFromDto(Company company, CompanyInput companyDto)
        {
            company.Name = companyDto.Name;
            company.Description = companyDto.Description;
        }

        public void UpdateCompanyFromJsonPatchDocument(Company company, JsonPatchDocument<CompanyInput> patchDoc)
        {
            // Маппим Company -> CompanyInput
            var companyInput = new CompanyInput
            {
                Name = company.Name,
                Description = company.Description
            };

            // Применяем патч к DTO
            patchDoc.ApplyTo(companyInput);

            UpdateCompanyFromDto(company, companyInput);
        }
    }
}
