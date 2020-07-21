using Abstractions.Configuration;
using Abstractions.Security;
using ApiPermissionBindings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustomAPI
{
    public class HttpFunctions
    {
        public HttpFunctions(ApplicationConfigSection appConfig)
        {
            this.AppConfig = appConfig;
        }

        private ApplicationConfigSection AppConfig;
        private HttpResponseMessage DefaultResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Action succeeded.") };

        [FunctionName(Names.Read)]
        public async Task<HttpResponseMessage> Read(
            [HttpTrigger("GET", Route = "read")]HttpRequestMessage request,
            [PermissionGateway(Permissions.DataReadAll, Permissions.DataReadWriteAll)]PermissionGateway gateway,
            ClaimsPrincipal user
        )
        {
            return await gateway.ExecuteProtectedActionAsync(() =>
            {
                return Task.FromResult(this.DefaultResponse);
            });
        }

        [FunctionName(Names.Write)]
        public async Task<HttpResponseMessage> Write(
            [HttpTrigger("POST", Route = "write")]HttpRequestMessage request,
            [PermissionGateway(Permissions.DataReadWriteAll)]PermissionGateway gateway,
            ClaimsPrincipal user
        )
        {
            return await gateway.ExecuteProtectedActionAsync(() =>
            {
                return Task.FromResult(this.DefaultResponse);
            });
        }

    }
}
