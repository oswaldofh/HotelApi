using HotelApi.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Number { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal? Iva { get; set; }

        [Required]
        [MaxLength(15)]
        public string Location { get; set; }

        [Required]
        public int PersonQuantity { get; set; }

        [Required]
        [ForeignKey("HotelId")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        [Required]
        [ForeignKey("RoomTypeId")]
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        [Required]
        public RoomStatus RoomStatus { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
