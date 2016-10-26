using System.Threading.Tasks;

namespace AwesomeServer.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}