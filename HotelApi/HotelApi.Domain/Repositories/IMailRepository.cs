using HotelApi.Common.Response;

namespace HotelApi.Domain.Repositories
{
    public interface IMailRepository
    {
        ResponseEmail SendMail(string toName, string toEmail, string subject, string body);
    }
}
