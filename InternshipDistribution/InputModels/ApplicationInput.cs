using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.InputModels
{
    public class ApplicationInput
    {
        [Required(ErrorMessage = "ID студента обязательно")]
        public int StudentId { get; set; }

        public int? Priority1CompanyId { get; set; }
        public int? Priority2CompanyId { get; set; }
        public int? Priority3CompanyId { get; set; }
    }
}
