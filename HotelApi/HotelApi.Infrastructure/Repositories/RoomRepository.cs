using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using HotelApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataContext _context;

        public RoomRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            var data = await _context.Rooms
                .Include(h => h.Hotel)
                .ThenInclude(c => c.City)
                .Include(rt => rt.RoomType)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (data == null)
            {
                return false;
            }

            _context.Rooms.Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Rooms
                .Include(h => h.Hotel)
                .ThenInclude(c => c.City)
                .Include(rt => rt.RoomType)
                .AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Room>> GetAll()
        {
            return await _context.Rooms
                .Include(h => h.Hotel)
                .ThenInclude(c => c.City)
                .Include(rt => rt.RoomType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetByCity(string city)
        {
            return await _context.Rooms
               .Include(h => h.Hotel)
               .ThenInclude(c => c.City)
               .Include(rt => rt.RoomType)
               .Where(r => r.Hotel.City.Name.ToLower().Trim() == city.ToLower().Trim())
               .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetByDate(DateTime date)
        {
            return await _context.Rooms
               .Include(h => h.Hotel)
               .ThenInclude(c => c.City)
               .Include(rt => rt.RoomType)
               .Where(h => !_context.Bookings.Any(r => r.RoomId == h.Id && date >= r.FirstDate && date < r.LastDate))
               .ToListAsync();
        }

        public async Task<Room> GetById(int id)
        {
            return await _context.Rooms
                .Include(h => h.Hotel)
                .ThenInclude(c => c.City)
                .Include(rt => rt.RoomType)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Room> GetByName(string name)
        {
            return await _context.Rooms
                .Include(h => h.Hotel)
                .ThenInclude(c => c.City)
                .Include(rt => rt.RoomType)
                .FirstOrDefaultAsync(c => c.Number.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<IEnumerable<Room>> GetByNumberPerson(int number)
        {
            return await _context.Rooms
               .Include(h => h.Hotel)
               .ThenInclude(c => c.City)
               .Include(rt => rt.RoomType)
               .Where(r => r.PersonQuantity >= number)
               .ToListAsync();
        }

        public async Task Save(Room model)
        {
            _context.Rooms.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Room model)
        {
            _context.Rooms.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
