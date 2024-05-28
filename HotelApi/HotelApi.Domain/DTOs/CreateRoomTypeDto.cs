using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.DTOs
{
    public class CreateRoomTypeDto
    {
        [Display(Name = "Tipo habitación"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(255, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }
    }
}
