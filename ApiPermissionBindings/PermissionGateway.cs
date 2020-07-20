using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
