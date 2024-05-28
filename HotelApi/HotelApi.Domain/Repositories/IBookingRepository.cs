using HotelApi.Domain.Entities;

namespace HotelApi.Domain.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAll();
        Task<Booking> GetById(int id);
        Task<Booking> GetByDocument(string document);
        Task<IEnumerable<Booking>> GetByDate(DateTime date);
        Task Save(Booking model);
        Task Update(Booking model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);
    }
}
