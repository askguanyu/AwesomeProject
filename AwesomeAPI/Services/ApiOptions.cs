using Microsoft.Extensions.Logging;

namespace AwesomeAPI.Services
{
    public class ApiOptions
    {
        public string DbConnection { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}