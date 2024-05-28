using AutoMapper;
using HotelApi.Domain.DTOs;
using HotelApi.Domain.Entities;

namespace HotelApi.Infrastructure.HotelApiMappers
{
    public class HotelApiMapper : Profile
    {
        public HotelApiMapper()
        {
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, InformationBookingDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CreateCityDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, InformationHotelDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<HotelType, CreateHotelTypeDto>().ReverseMap();
            CreateMap<HotelType, HotelTypeDto>().ReverseMap();
            CreateMap<RoomType, RoomTypeDto>().ReverseMap();
            CreateMap<RoomType, CreateRoomTypeDto>().ReverseMap();
            CreateMap<Room, CreateRoomDto>().ReverseMap();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<Room, InformationRoomDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();

        }
    }
}
