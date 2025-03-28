namespace InternshipDistribution.Dto
{
    public class StudentWithApplication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string? Fathername { get; set; }
        public int UserId { get; set; }
        public string? ResumeFileName { get; set; }
        public int? Priority1CompanyId { get; set; }
        public int? Priority2CompanyId { get; set; }
        public int? Priority3CompanyId { get; set; }

        public string Priority1Status { get; set; }
        public string Priority2Status { get; set; }
        public string Priority3Status { get; set; }

        public string Status { get; set; }
    }
}
