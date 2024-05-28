using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.Entities
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
