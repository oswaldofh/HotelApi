using HotelApi.Domain.Entities;

namespace HotelApi.Domain.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAll();
        Task<Room> GetById(int id);
        Task<Room> GetByName(string name);
        Task<IEnumerable<Room>> GetByCity(string city);
        Task<IEnumerable<Room>> GetByNumberPerson(int number);
        Task<IEnumerable<Room>> GetByDate(DateTime date);
        Task Save(Room model);
        Task Update(Room model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);
    }
}
