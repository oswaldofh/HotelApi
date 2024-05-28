using HotelApi.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.DTOs
{
    public class CreateBookingDto
    {
        [Display(Name = "Nombre completo"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string FullName { get; set; }


        [Display(Name = "Documento"),]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Document { get; set; }

        [Display(Name = "Genero"),]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public Gender Gender { get; set; }

        [Display(Name = "Tipo de documento"),]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DocumentType DocumentType { get; set; }

        [Display(Name = "Fecha nacimiento"),]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Email { get; set; }

        [Display(Name = "Telefono"),]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Nombre de contacto"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string FullNameContact { get; set; }

        [Display(Name = "Telefono"),]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string PhoneNumberContact { get; set; }

        [Display(Name = "Habitación"),]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int RoomId { get; set; }

        [Display(Name = "Fecha ingresa"),]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime FirstDate { get; set; }

        [Display(Name = "Fecha salida"),]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime LastDate { get; set; }
    }
}
