using HotelApi.Common.Enums;
using HotelApi.Domain.Entities;
using System.Text.Json.Serialization;

namespace HotelApi.Domain.DTOs
{
    public class InformationRoomDto
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public decimal Price { get; set; }

        public decimal Iva { get; set; }

        public string Location { get; set; }

        public int PersonQuantity { get; set; }

        [JsonIgnore]
        public Hotel Hotel { get; set; }
        public string NameHotel => Hotel.Name;
        public string City => Hotel.City.Name;
        [JsonIgnore]
        public RoomType RoomType { get; set; }
        public string RoomTypeId => RoomType.Name;
        [JsonIgnore]
        public RoomStatus RoomStatus { get; set; }
        public string Status => RoomStatus.ToString();
    }
}
