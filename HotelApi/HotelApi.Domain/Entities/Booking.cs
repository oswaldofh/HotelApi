using HotelApi.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(15)]
        public string Document { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullNameContact { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumberContact { get; set; }

        [Required]
        public decimal TotalValue { get; set; }

        [Required]
        public decimal ValueIva { get; set; }

        [Required]
        [ForeignKey("RoomId")]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Required]
        public DateTime FirstDate { get; set; }

        [Required]
        public DateTime LastDate { get; set; }

        [Required]
        public BookingStatus BookingStatus { get; set; }
    }
}
