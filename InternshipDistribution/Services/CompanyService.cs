using InternshipDistribution.DTO;
using InternshipDistribution.Models;

namespace InternshipDistribution.Services
{
    public class CompanyService
    {
        public void UpdateCompanyFromDto(Company company, CompanyDto companyDto)
        {
            company.Name = companyDto.Name;
            company.Description = companyDto.Description;
        }
    }
}
