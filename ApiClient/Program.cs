using Abstractions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiClient
{
    internal class Program
    {
        internal static ServiceCollection Services = new ServiceCollection();
        internal static IServiceProvider ServiceProvider;
        internal static RootConfigSection Config;

        static void Main(string[] args)
        {
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true)
                .Build();

            var root = configRoot.Get<RootConfigSection>();
            Config = root;
            Services.AddSingleton(root);
            Services.AddSingleton(root.Api);
            Services.AddSingleton(root.Application);

            ServiceProvider = Services.BuildServiceProvider();

            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            await CallApiAsApplicationAsync();
        }

        static async Task CallApiAsApplicationAsync()
        {
            Console.WriteLine($"Calling API at '{Config.Api.BaseUri}' as application with client ID '{Config.Application.ClientId}'");
            await ExecuteApiEndpointAsync("/read", HttpMethod.Get, await GetApplicationTokenAsync());
            await ExecuteApiEndpointAsync("/write", HttpMethod.Post, await GetApplicationTokenAsync());
        }

        private static HttpClient Client = new HttpClient();
        private static async Task ExecuteApiEndpointAsync(string relativePath, HttpMethod method, AuthenticationResult token)
        {
            var url = $"{Config.Api.BaseUri}{relativePath}";
            Console.WriteLine($"Executing API endpoint: '{url}'...");

            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            HttpResponseMessage response;
            try
            {
                response = await Client.SendAsync(request);
                Console.WriteLine($"Request completed with status code: {response.StatusCode}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An exception occured: {ex.Message}");
            }
        }

        private static AuthenticationResult AppToken;
        private static async Task<AuthenticationResult> GetApplicationTokenAsync()
        {
            if(AppToken?.ExpiresOn > DateTimeOffset.UtcNow)
            {
                return AppToken;
            }

            var app = ConfidentialClientApplicationBuilder
                .Create(Config.Application.ClientId)
                .WithTenantId(Config.Application.TenantId)
                .WithClientSecret(Config.Application.ClientSecret)
                .Build();

            var result = await app
                .AcquireTokenForClient(Config.Api.Scopes)
                .ExecuteAsync();

            AppToken = result;
            return AppToken;
        }

        private static async Task<AuthenticationResult> GetUserTokenAsync()
        {
            try
            {
                var app = PublicClientApplicationBuilder
                    .Create(Config.Application.ClientId)
                    .WithTenantId(Config.Application.TenantId)
                    .WithDefaultRedirectUri()
                    .Build();

                var result = await app
                    .AcquireTokenInteractive(Config.Api.Scopes)
                    .ExecuteAsync();

                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return null;
        }

    }
}
