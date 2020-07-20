using ApiPermissionBindings;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {

        public static IWebJobsBuilder AddPermissionGateway(this IWebJobsBuilder builder)
        {
            builder.AddExtension<ApiPermissionExtensionProvider>();
            return builder;
        }
    }
}
