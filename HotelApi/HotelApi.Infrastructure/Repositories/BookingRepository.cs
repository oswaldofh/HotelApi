using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using HotelApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DataContext _context;

        public BookingRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            var booking = await _context.Bookings
               .Include(b => b.Room)
               .ThenInclude(h => h.Hotel)
               .ThenInclude(c => c.City)
               .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return false;
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                 .ThenInclude(h => h.Hotel)
               .ThenInclude(c => c.City)
                .AnyAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetAll()
        {
            return await _context.Bookings
                 .Include(b => b.Room)
                  .ThenInclude(h => h.Hotel)
               .ThenInclude(c => c.City)
                 .OrderBy(b => b.Id)
                 .ToListAsync();
        }

        public async Task<Booking> GetById(int id)
        {
            return await _context.Bookings
                 .Include(b => b.Room)
                  .ThenInclude(h => h.Hotel)
               .ThenInclude(c => c.City)
                 .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Booking> GetByDocument(string document)
        {
            return await _context.Bookings
                 .Include(b => b.Room)
                  .ThenInclude(h => h.Hotel)
               .ThenInclude(c => c.City)
                 .FirstOrDefaultAsync(b => b.Document == document);
        }


        public async Task Save(Booking model)
        {
            _context.Bookings.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Booking model)
        {
            _context.Bookings.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetByDate(DateTime date)
        {
            return await _context.Bookings
               .Include(b => b.Room)
               .ThenInclude(h => h.Hotel)
               .ThenInclude(c => c.City)
               .Where(b => b.FirstDate <= date && b.LastDate >= date)
               .ToListAsync();
        }
    }
}
