using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipDistribution.Models
{
    public class Student : BaseEntity
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        public string Lastname { get; set; }
        public string? Fathername { get; set; }
        public string? Resume { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }  // Тип должен совпадать с ключом в User

        // Навигационное свойство
        public virtual User User { get; set; }
    }
}
