using Microsoft.Extensions.Logging;

namespace AwesomeServer.Services
{
    public class ServerOptions
    {
        public string ProjectName { get; set; }
        public string DbConnection { get; set; }
        public LogLevel LogLevel { get; set; }
        public EmailOptions EmailOptions { get; set; }

        public ServerOptions()
        {
            EmailOptions = new EmailOptions();
        }
    }
}