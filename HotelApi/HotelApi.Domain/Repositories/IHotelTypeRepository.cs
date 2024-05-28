using HotelApi.Domain.Entities;

namespace HotelApi.Domain.Repositories
{
    public interface IHotelTypeRepository
    {
        Task<IEnumerable<HotelType>> GetAll();
        Task<HotelType> GetById(int id);
        Task<HotelType> GetByName(string name);
        Task Save(HotelType model);
        Task Update(HotelType model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);
    }
}
