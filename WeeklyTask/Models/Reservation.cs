using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WeeklyTask.Areas.Identity.Data;



namespace WeeklyTask.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Reservation Date")]
        [CheckReservationDate]
        public DateTime ReservationDate { get; set; }

        [Required]
        [Range(1, 100)]
        [Display(Name = "Party Size")]
        public int PartySize { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }
        public string ContactName { get; set; }

        public string ContactPhone { get; set; }


    }

    public class CheckReservationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reservation = (Reservation)validationContext.ObjectInstance;
            if (value != null && ((DateTime)value) < DateTime.Now)
            {
                return new ValidationResult("Reservation date cannot be in the past.");
            }
            return ValidationResult.Success;
        }
    }
}
