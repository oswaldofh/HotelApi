using HotelApi.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApi.Domain.Entities
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(15)]
        public string phone { get; set; }

        [Required]
        [ForeignKey("CityId")]
        public int CityId { get; set; }
        public City City { get; set; }

        [Required]
        [ForeignKey("HotelTypeId")]
        public int HotelTypeId { get; set; }
        public HotelType HotelType { get; set; }

        [Required]
        public HotelState HotelState { get; set; }
        public ICollection<Room> Rooms { get; set; }

    }
}
