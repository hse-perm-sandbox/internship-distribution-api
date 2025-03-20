using InternshipDistribution.InputModels;

namespace InternshipDistribution.Dto
{
    public class BulkCreateStudentRequest
    {
        public List<StudentInputWithEmail> Students { get; set; }
    }

    public class BulkCreateStudentsResponse
    {
        public List<StudentWithPasswordResult> CreatedStudents { get; set; } = new();
        public List<string> FailedEmails { get; set; } = new();
    }

    public class StudentWithPasswordResult
    {
        public int StudentId { get; set; }
        public string Email { get; set; }
        public string GeneratedPassword { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string? Fathername { get; set; }
    }
}
