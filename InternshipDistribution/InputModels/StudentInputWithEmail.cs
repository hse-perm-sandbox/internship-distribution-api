using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.InputModels
{
    public class StudentInputWithEmail
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        public string Lastname { get; set; }
        public string? Fathername { get; set; }
    }
}
