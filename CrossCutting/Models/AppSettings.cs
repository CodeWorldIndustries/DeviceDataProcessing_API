namespace CrossCutting.Models
{
    public class AppSettings
    {
        public string AllowedHosts { get; set; }
        public string AppApiKey { get; set; }
        public DatabaseSettings Database { get; set; }
        public string Environment { get; set; }
        public LoggingSettings Logging { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }

    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; set; }
    }

    public class LogLevelSettings
    {
        public string Default { get; set; }
        public string Microsoft_AspNetCore { get; set; }
    }
}
