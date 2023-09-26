using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string CustomerName { get; set; } = null!;

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public Decimal Price { get; set; }
    }
}
