namespace AwesomeServer.Services
{
    public class EmailOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }
}