using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiPermissionBindings
{
    public class PermissionGateway
    {
        internal PermissionGateway(PermissionMode mode, IEnumerable<string> requiredPermissions, string accessToken)
        {
            this.Mode = mode;
            this.RequiredPermissions = requiredPermissions ?? new string[0];
            this.AccessToken = accessToken;
        }

        private PermissionMode Mode;
        private IEnumerable<string> RequiredPermissions;
        private string AccessToken;


        public async Task<HttpResponseMessage> ExecuteProtectedActionAsync(Func<Task<HttpResponseMessage>> action)
        {
            if(this.VerifyPermissions())
            {
                return await action();
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }


        public IEnumerable<string> GetTokenPermissions()
        {
            try
            {
                var token = new JwtSecurityToken(this.AccessToken);
                return from x in token.Claims where x.Type == "roles" select x.Value;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return new List<string>();
            }
        }

        private bool VerifyPermissions()
        {
            bool hasPermission = false;
            if(this.RequiredPermissions?.Count() > 0)
            {
                var tokenPermissions = this.GetTokenPermissions();
                if(this.Mode == PermissionMode.Any)
                {
                    hasPermission = tokenPermissions.Any(x => this.RequiredPermissions.Contains(x));
                }
                else
                {
                    hasPermission = tokenPermissions.All(x => this.RequiredPermissions.Contains(x));
                }
            }

            return hasPermission;
        }


    }
}
