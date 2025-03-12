using InternshipDistribution.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipDistribution.InputModels
{
    public class StudentInput
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        public string Lastname { get; set; }
        public string? Fathername { get; set; }

        [Required(ErrorMessage = "Id созданного User обязательно")]
        public int UserId { get; set; }
    }
}
