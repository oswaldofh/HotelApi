using HotelApi.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.DTOs
{
    public class RoomTypeDto : CreateRoomTypeDto
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Id { get; set; }
    }
}
