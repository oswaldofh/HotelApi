using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using HotelApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _context;

        public CityRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteCity(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(s => s.Id == id);

            if (city == null)
            {
                return false;
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistCity(int id)
        {
            return await _context.Cities.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            return await _context.Cities
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<City> GetCity(int id)
        {
            return await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<City> GetCity(string name)
        {
            return await _context.Cities.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task SaveCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCity(City city)
        {
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
        }
    }
}
