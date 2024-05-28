using HotelApi.Common.Response;
using HotelApi.Domain.Repositories;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace HotelApi.Infrastructure.Repositories
{
    public class MailRepository : IMailRepository
    {

        private readonly IConfiguration _configuration; //PARA ACCEDER LO QUE SE GUARDA EN EL APPSETTING

        public MailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ResponseEmail SendMail(string toName, string toEmail, string subject, string body)
        {
            try
            {
                string from = _configuration["Mail:From"];
                string name = _configuration["Mail:Name"];
                string smtp = _configuration["Mail:Smtp"];
                string port = _configuration["Mail:Port"];
                string password = _configuration["Mail:Password"];

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(name, from));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;
                BodyBuilder bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body //RECIBE html, css, se puede personalizar
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient()) //SE CREA EL CLIENTE SE ENVIA Y SE DESCONECTA
                {
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return new ResponseEmail
                {
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new ResponseEmail
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex
                };
            }
        }
    }
}
