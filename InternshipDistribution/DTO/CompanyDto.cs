using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.DTO
{
    public class CompanyDto
    {
        [Required(ErrorMessage = "Название компании обязательно")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
