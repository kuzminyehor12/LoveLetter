using Microsoft.Identity.Client;
using System.Configuration;

namespace LoveLetter.Core.Utils
{
    public static class ConfigurationUtils
    {
        public static AuthenticationResult AuthenticateToAzure()
        {
            string? clientId = ConfigurationManager.AppSettings["AzureAd:ClientId"];
            string? authority = ConfigurationManager.AppSettings["AzureAd:Authority"];
            string[] scopes = ConfigurationManager.AppSettings["AzureAd:Scopes"]?.Split(';') ?? Array.Empty<string>();

            var app = PublicClientApplicationBuilder.Create(clientId)
                  .WithAuthority(authority)
                  .WithRedirectUri("http://localhost")
                  .Build();

            return app.AcquireTokenInteractive(scopes)
                .WithPrompt(Prompt.SelectAccount)
                .ExecuteAsync().Result;
        }
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
