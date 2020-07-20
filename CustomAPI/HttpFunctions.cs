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

        [FunctionName(nameof(HttpFunctions.Read))]
        public async Task<HttpResponseMessage> Read([HttpTrigger("GET", Route = "read")]HttpRequestMessage request, [PermissionGateway(Permissions.DataReadAll)]PermissionGateway gateway, ClaimsPrincipal user)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Sample data returned after successful permission check.")
            };
        }

        [FunctionName(nameof(HttpFunctions.Write))]
        public async Task<HttpResponseMessage> Write([HttpTrigger("POST", Route = "write")]HttpRequestMessage request, [PermissionGateway(Permissions.DataReadWriteAll)]PermissionGateway gateway, ClaimsPrincipal user)
        {

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Whatever data you posted was written.")
            };
        }




        /// <summary>
        /// This method shows you how you can check for permissions before you execute an action that requires certain permissions.
        /// </summary>
        /// <remarks>
        /// This method also supports local development by converting any bearer token into a <see cref="ClaimsPrincipal"/> object. Note that
        /// this is performed only if you are running your function locally.
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> ExecuteProtectedActionAsync(HttpRequestMessage request, ClaimsPrincipal caller, Func<ClaimsPrincipal, Task<HttpResponseMessage>> action, params string[] permissions)
        {
            if(permissions?.Length > 0)
            {
                if(request.RequestUri.Host?.ToLower() == "localhost" && request.Headers.Authorization?.Scheme?.ToLower() == "bearer")
                {
                    try
                    {
                        var token = new JwtSecurityToken(request.Headers.Authorization.Parameter);
                        string nameClaimType = token.Claims.Any(x => x.Type == "appid") ? "appid" : ClaimTypes.Name;
                        caller = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, null, nameClaimType, "roles"));
                    }
                    catch { }
                }

                bool hasPermission = false;
                foreach(var p in permissions)
                {
                    if(caller.Claims.Where(x => x.Type == "roles" && x.Value == p).Count() > 0)
                    {
                        hasPermission = true;
                        break;
                    }
                }

                if(hasPermission)
                {
                    return await action(caller);
                }
            }

            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

    }
}
