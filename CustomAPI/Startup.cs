using Abstractions.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

[assembly: WebJobsStartup(typeof(CustomAPI.Startup))]

namespace CustomAPI
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var provider = builder.Services.BuildServiceProvider();
            var config = provider.GetService<IConfiguration>();
            var rootConfig = config.Get<FunctionAppConfigSection>();

            builder.Services.AddSingleton(rootConfig);
            builder.Services.AddSingleton(rootConfig.Application);

            builder.AddPermissionGateway();
        }
    }
}
