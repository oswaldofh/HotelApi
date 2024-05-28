using HotelApi.Domain.Entities;

namespace HotelApi.Domain.Repositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAll();
        Task<Hotel> GetById(int id);
        Task<Hotel> GetByName(string name);
        Task Save(Hotel model);
        Task Update(Hotel model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);
        Task<bool> ExistHotelInCity(string name, int cityId);
        Task<IEnumerable<Hotel>> GetHotelsInCity(int cityId);
        Task<IEnumerable<Hotel>> GetHotelsInCity(string city);
        Task<IEnumerable<Hotel>> GetHotelsInType(int typeId);
    }
}
