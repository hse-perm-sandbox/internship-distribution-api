using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.InputModels
{
    public class CompanyInput
    {
        [Required(ErrorMessage = "Название компании обязательно")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
