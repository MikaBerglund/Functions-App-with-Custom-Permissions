using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;

namespace ApiPermissionBindings
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PermissionGatewayAttribute : Attribute
    {

        public PermissionGatewayAttribute(params string[] permissions)
        {
            this.Mode = PermissionMode.Any;
            this.Permissions = permissions ?? new string[0];
        }

        public PermissionGatewayAttribute(PermissionMode mode, params string[] permissions)
        {
            this.Mode = mode;
            this.Permissions = permissions ?? new string[0];
        }



        public IEnumerable<string> Permissions { get; set; }

        public PermissionMode Mode { get; set; }

    }

    public enum PermissionMode
    {
        Any,
        All
    }
}
