using InternshipDistribution.Enums;
using InternshipDistribution.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InternshipDistribution.Dto
{
    public class DistributionApplicationDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int? Priority1CompanyId { get; set; }
        public int? Priority2CompanyId { get; set; }
        public int? Priority3CompanyId { get; set; }

        public string Priority1Status { get; set; } 
        public string Priority2Status { get; set; }
        public string Priority3Status { get; set; }

        public string Status { get; set; }
    }
}
