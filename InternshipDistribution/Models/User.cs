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
        public string PasswordHash { get; set; }

        [Required]
        public bool IsManager { get; set; } = false;

        public virtual Student Student { get; set; }
    }
}
