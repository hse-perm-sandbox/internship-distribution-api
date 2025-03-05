using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.InputModels
{
    public class RegisterInput
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsManager { get; set; } = false;
    }
}
