using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.Enums
{
    public enum ApplicationStatus
    {
        [Display(Name = "Создано")]
        Created,              // Черновик (ничего не отправлено)

        [Display(Name = "На рассмотрении")]
        UnderReview,          // Отправлено ≥1 компании

        [Display(Name = "Завершено")]
        Completed            // Хотя бы одна компания приняла
    }
}
