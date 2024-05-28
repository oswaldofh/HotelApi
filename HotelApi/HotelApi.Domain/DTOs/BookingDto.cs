using HotelApi.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.DTOs
{
    public class BookingDto : CreateBookingDto
    {

        [Display(Name = "Id")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Id { get; set; }

        [Display(Name = "Valor total")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal TotalValue { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public BookingStatus BookingStatus { get; set; }
    }
}
