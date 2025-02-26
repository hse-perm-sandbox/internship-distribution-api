using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        [MaxLength(255, ErrorMessage = "Email слишком длинный")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }  // Меняем на хеш пароля

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(100, ErrorMessage = "Имя слишком длинное")]
        public string Name { get; set; }

        [Required]
        public bool IsManager { get; set; }
    }
}
