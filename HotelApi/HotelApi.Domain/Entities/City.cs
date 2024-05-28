using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public ICollection<Hotel> Hotels { get; set; }

    }
}
