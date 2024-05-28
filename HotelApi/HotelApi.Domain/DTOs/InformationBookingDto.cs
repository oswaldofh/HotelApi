using HotelApi.Common.Enums;
using HotelApi.Domain.Entities;
using System.Text.Json.Serialization;

namespace HotelApi.Domain.DTOs
{
    public class InformationBookingDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Document { get; set; }

        [JsonIgnore]
        public Gender Gender { get; set; }
        public string NameGender => Gender.ToString();

        [JsonIgnore]
        public DocumentType DocumentType { get; set; }
        public string NameDocumentType => DocumentType.ToString();
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullNameContact { get; set; }
        public string PhoneNumberContact { get; set; }
        [JsonIgnore]
        public Room Room { get; set; }

        public string Iva => Room.Iva.ToString() + "%";
        public decimal Value => Room.Price;
        public decimal ValueIva { get; set; }
        public decimal TotalValue { get; set; }
        public string NumberRoom => Room.Number;
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        [JsonIgnore]
        public BookingStatus BookingStatus { get; set; }
        public string Status => BookingStatus.ToString();
        public string City => Room.Hotel.City.Name;

    }
}
