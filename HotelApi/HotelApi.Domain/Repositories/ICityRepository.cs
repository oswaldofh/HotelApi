using HotelApi.Domain.Entities;

namespace HotelApi.Domain.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCities();
        Task<City> GetCity(int id);
        Task<City> GetCity(string name);
        Task SaveCity(City city);
        Task UpdateCity(City city);
        Task<bool> DeleteCity(int id);
        Task<bool> ExistCity(int id);
    }
}
