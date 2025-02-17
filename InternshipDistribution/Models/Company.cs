using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.Models
{
    public class Company : BaseEntity
    {
        [Required(ErrorMessage = "Название компании обязательно")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
