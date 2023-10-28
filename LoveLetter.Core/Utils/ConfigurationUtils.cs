using System.Configuration;

namespace LoveLetter.Core.Utils
{
    public static class ConfigurationUtils
    {
        public static string? GetLocalConnectionString() =>
            ConfigurationManager.ConnectionStrings["Local"].ConnectionString;

        public static string? GetRemoteConnectionString() =>
            ConfigurationManager.ConnectionStrings["Remote"]?.ConnectionString ?? null;

        public static string? GetConnectionString() =>
             string.IsNullOrEmpty(GetRemoteConnectionString()) ?
                GetLocalConnectionString()
                : GetRemoteConnectionString();
    }
}
