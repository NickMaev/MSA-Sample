namespace Shared.Configuration
{
    public abstract class AppSettingsBase
    {
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorUrl { get; set; }
        public string ProjectFullName { get; set; }
        public string ProjectDescription { get; set; }
        public string ApiVersion { get; set; }
        public string License { get; set; }
        public RabbitMqSettings RabbitMq { get; set; }
    }
}