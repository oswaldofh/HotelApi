using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.DTOs
{
    public class CreateHotelDto
    {
        [Display(Name = "Hotel"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }


        [Display(Name = "Dirección"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Address { get; set; }

        [Display(Name = "Dirección"),]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string phone { get; set; }

        [Display(Name = "Ciudad"),]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CityId { get; set; }

        [Display(Name = "Tipo de hotel"),]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int HotelTypeId { get; set; }

    }
}
