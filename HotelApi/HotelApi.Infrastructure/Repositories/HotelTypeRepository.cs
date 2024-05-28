using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using HotelApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Infrastructure.Repositories
{

    public class HotelTypeRepository : IHotelTypeRepository
    {
        private readonly DataContext _context;

        public HotelTypeRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            var hotelType = await _context.HotelTypes.FirstOrDefaultAsync(s => s.Id == id);

            if (hotelType == null)
            {
                return false;
            }

            _context.HotelTypes.Remove(hotelType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.HotelTypes.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<HotelType>> GetAll()
        {
            return await _context.HotelTypes
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<HotelType> GetById(int id)
        {
            return await _context.HotelTypes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<HotelType> GetByName(string name)
        {
            return await _context.HotelTypes.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task Save(HotelType model)
        {
            _context.HotelTypes.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(HotelType model)
        {
            _context.HotelTypes.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
