using System.ComponentModel.DataAnnotations;

namespace InternshipDistribution.Enums
{
    public enum PriorityStatus
    {
        [Display(Name = "Не выбрано")]
        NotSelected,  

        [Display(Name = "Направлено")]
        Sent,         

        [Display(Name = "Отказ")]
        Rejected,    

        [Display(Name = "В работе")]
        InProgress,   

        [Display(Name = "Принято")]
        Accepted      
    }
}
