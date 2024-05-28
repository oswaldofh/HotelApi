using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.DTOs
{
    public class CreateCityDto
    {

        [Display(Name = "Ciudad"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }
    }
}
