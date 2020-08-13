namespace Shared.Configuration
{
    public class RabbitMqSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int TimeoutInMilliseconds { get; set; }
    }
}