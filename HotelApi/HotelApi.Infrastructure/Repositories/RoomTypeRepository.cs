using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using HotelApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApi.Infrastructure.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly DataContext _context;

        public RoomTypeRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            var data = await _context.RoomTypes.FirstOrDefaultAsync(s => s.Id == id);

            if (data == null)
            {
                return false;
            }

            _context.RoomTypes.Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.RoomTypes.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<RoomType>> GetAll()
        {
            return await _context.RoomTypes
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<RoomType> GetById(int id)
        {
            return await _context.RoomTypes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<RoomType> GetByName(string name)
        {
            return await _context.RoomTypes.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task Save(RoomType model)
        {
            _context.RoomTypes.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(RoomType model)
        {
            _context.RoomTypes.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
