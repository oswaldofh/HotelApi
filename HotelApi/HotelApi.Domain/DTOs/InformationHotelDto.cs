using HotelApi.Common.Enums;
using HotelApi.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelApi.Domain.DTOs
{
    public class InformationHotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string phone { get; set; }

        [JsonIgnore]
        public City City { get; set; }
        public string NameCity => City.Name;

        [JsonIgnore]
        public HotelType HotelType { get; set; }
        public string Type => HotelType.Name;

        [JsonIgnore]
        public HotelState HotelState { get; set; }
        public string State => HotelState.ToString();
    }
}
