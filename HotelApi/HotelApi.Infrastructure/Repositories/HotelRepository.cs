using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using HotelApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly DataContext _context;

        public HotelRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            var hotel = await _context.Hotels
                .Include(c => c.City)
                .Include(r => r.Rooms)
                .Include(t => t.HotelType)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (hotel == null)
            {
                return false;
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Hotels
                .Include(c => c.City)
                .Include(r => r.Rooms)
                .Include(t => t.HotelType)
                .AnyAsync(c => c.Id == id);
        }

        public async Task<bool> ExistHotelInCity(string name, int cityId)
        {
            var state = await _context.Hotels
                 .Include(c => c.City)
                 .Include(r => r.Rooms)
                 .Include(t => t.HotelType)
                 .Where(h => h.Name == name && h.CityId == cityId)
                 .FirstOrDefaultAsync();

            if (state != null)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Hotel>> GetAll()
        {
            return await _context.Hotels
                .Include(c => c.City)
                .Include(r => r.Rooms)
                .Include(t => t.HotelType)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Hotel> GetById(int id)
        {
            return await _context.Hotels
                .Include(c => c.City)
                .Include(r => r.Rooms)
                 .Include(t => t.HotelType)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Hotel> GetByName(string name)
        {
            return await _context.Hotels
                .Include(c => c.City)
                .Include(r => r.Rooms)
                .Include(t => t.HotelType)
                .FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<IEnumerable<Hotel>> GetHotelsInCity(int cityId)
        {
            return await _context.Hotels
                .Include(c => c.City)
                .Include(r => r.Rooms)
                .Include(t => t.HotelType)
                .Where(h => h.CityId == cityId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Hotel>> GetHotelsInCity(string city)
        {
            return await _context.Hotels
                .Include(c => c.City)
                .Include (r => r.Rooms)
                .Include(t => t.HotelType)
                .Where(h => h.City.Name.ToLower().Trim() == city.ToLower().Trim())
                .ToListAsync();
        }

        public async Task<IEnumerable<Hotel>> GetHotelsInType(int typeId)
        {
            return await _context.Hotels
                .Include(c => c.City)
                .Include(t => t.HotelType)
                .Where(h => h.HotelTypeId == typeId)
                .ToListAsync();
        }

        public async Task Save(Hotel model)
        {
            _context.Hotels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Hotel model)
        {
            _context.Hotels.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
