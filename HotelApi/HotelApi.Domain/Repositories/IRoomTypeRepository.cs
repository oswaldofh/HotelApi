using HotelApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApi.Domain.Repositories
{
    public interface IRoomTypeRepository
    {
        Task<IEnumerable<RoomType>> GetAll();
        Task<RoomType> GetById(int id);
        Task<RoomType> GetByName(string name);
        Task Save(RoomType model);
        Task Update(RoomType model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);
    }
}
