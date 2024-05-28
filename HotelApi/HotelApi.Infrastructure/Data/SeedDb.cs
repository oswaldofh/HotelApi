using HotelApi.Domain.Entities;

namespace HotelApi.Infrastructure.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            //ESTA FUNCION CREA LA BASE DE DATOS Y LE APLICA LAS MIGRACIONES (hace la funcion de update-datebase) en codigo
            await _context.Database.EnsureCreatedAsync();

            await CheckCitiesAsync();
            await CheckHotelTypesAsync();
            await CheckRoomTypesAsync();

        }


        private async Task CheckRoomTypesAsync()
        {
            if (!_context.RoomTypes.Any())
            {
                _context.RoomTypes.Add(new RoomType { Name = "Suite", Description = "habitación doble con baño y salón de 12 m²" });
                _context.RoomTypes.Add(new RoomType { Name = "Junior suite", Description = "habitación doble con baño y salón de 8 m²" });
                _context.RoomTypes.Add(new RoomType { Name = "Gran suite", Description = "dos o más habitaciones dobles con sus correspondientes baños y un salón en común" });
                _context.RoomTypes.Add(new RoomType { Name = "Individual", Description = "Una cama y un baño" });
                await _context.SaveChangesAsync();

            }
        }


        private async Task CheckCitiesAsync()
        {
            if (!_context.Cities.Any())
            {
                _context.Cities.Add(new City { Name = "Medellin" });
                _context.Cities.Add(new City { Name = "Bogota" });
                _context.Cities.Add(new City { Name = "Cali" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckHotelTypesAsync()
        {
            if (!_context.HotelTypes.Any())
            {
                _context.HotelTypes.Add(new HotelType { Name = "Hotel urbano" });
                _context.HotelTypes.Add(new HotelType { Name = "Hotel para adultos" });
                _context.HotelTypes.Add(new HotelType { Name = "Hotel de playa" });
                _context.HotelTypes.Add(new HotelType { Name = "Hotel familiares" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
