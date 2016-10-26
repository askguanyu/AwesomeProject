using System.IO;
using AwesomeLib;
using Microsoft.AspNetCore.Hosting;

namespace AwesomeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseConfiguration(AwesomeMethods.CreateConfigurationBuilder("hosting").Build())
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}