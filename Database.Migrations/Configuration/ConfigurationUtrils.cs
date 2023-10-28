using System.Configuration;

namespace Database.Migrations.Configuration
{
    public static class ConfigurationUtils
    {
        public static string? GetLocalConnectionString() =>
            ConfigurationManager.ConnectionStrings["Local"].ConnectionString;

        public static string? GetRemoteConnectionString() =>
            ConfigurationManager.ConnectionStrings["Remote"]?.ConnectionString ?? null;
    }
}
