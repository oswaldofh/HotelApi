using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.DTOs
{
    public class CreateRoomDto
    {

        [Display(Name = "Numero"),]
        [MaxLength(10, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Number { get; set; }
    
        [Display(Name = "Precio"),]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal Price { get; set; }

        [Display(Name = "Iva"),]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal Iva { get; set; }

        [Display(Name = "Ubicación"),]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Location { get; set; }

        [Display(Name = "Cantidad de perosna"),]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int PersonQuantity { get; set; }


        [Display(Name = "Tipo de habitación"),]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int RoomTypeId { get; set; }
    

        [Display(Name = "Hotel"),]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int HotelId { get; set; }
    }
}
