using InternshipDistribution.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipDistribution.Models
{
    public class DistributionApplication : BaseEntity
    {
        [Required(ErrorMessage = "ID студента обязательно")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int? Priority1CompanyId { get; set; }
        public int? Priority2CompanyId { get; set; }
        public int? Priority3CompanyId { get; set; }

        public PriorityStatus Priority1Status { get; set; } = PriorityStatus.NotSelected;
        public PriorityStatus Priority2Status { get; set; } = PriorityStatus.NotSelected;
        public PriorityStatus Priority3Status { get; set; } = PriorityStatus.NotSelected;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Created;

        [ForeignKey("Priority1CompanyId")]
        public virtual Company? Priority1Company { get; set; }
        [ForeignKey("Priority2CompanyId")]
        public virtual Company? Priority2Company { get; set; }
        [ForeignKey("Priority3CompanyId")]
        public virtual Company? Priority3Company { get; set; }
    }
}
