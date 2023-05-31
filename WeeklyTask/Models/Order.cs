using WeeklyTask.Areas.Identity.Data;

namespace WeeklyTask.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? Description { get; set; } //Pick up or deliverly
        public string? DeliveryAddress { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public string? UserEmail { get; set; }
        public string? GuestEmail { get; set; }
        //public string? UserId { get; set; } // Foreign key for the user
        //public ApplicationUser User { get; set; } // Navigation property for the user
        // other properties as needed
    }

}
