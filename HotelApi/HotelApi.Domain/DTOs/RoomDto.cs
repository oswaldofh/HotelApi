using HotelApi.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.DTOs
{
    public class RoomDto : CreateRoomDto
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Id { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public RoomStatus RoomStatus { get; set; }
    }
}
