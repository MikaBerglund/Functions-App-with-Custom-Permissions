
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiPermissionBindings
{
    public class ApiPermissionBinding : IBinding
    {

        internal ApiPermissionBinding(PermissionMode mode, IEnumerable<string> requiredPermissions)
        {
            this.Mode = mode;
            this.RequiredPermissions = requiredPermissions;
        }

        private PermissionMode Mode;
        private IEnumerable<string> RequiredPermissions;

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            context.BindingData.TryGetValue("Headers", out object val);
            var headers = val as IDictionary<string, string> ?? new Dictionary<string, string>();
            headers.TryGetValue("Authorization", out string headerValue);
            AuthenticationHeaderValue.TryParse(headerValue, out AuthenticationHeaderValue authHeader);

            return Task.FromResult<IValueProvider>(new ApiPermissionValueProvider(this.Mode, this.RequiredPermissions, authHeader.Parameter));
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => null;

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
    }

    public class ApiPermissionExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            
            context.AddBindingRule<PermissionGatewayAttribute>().Bind(new ApiPermissionBindingProvider());
        }
    }

    public class ApiPermissionBindingProvider : IBindingProvider
    {
        internal ApiPermissionBindingProvider() { }


        public async Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var attribute = context.Parameter.GetCustomAttribute<PermissionGatewayAttribute>();
            return await Task.FromResult(new ApiPermissionBinding(attribute.Mode, attribute.Permissions));
        }
    }

    public class ApiPermissionValueProvider : IValueProvider
    {
        internal ApiPermissionValueProvider(PermissionMode mode, IEnumerable<string> requiredPermissions, string accessToken)
        {
            this.Mode = mode;
            this.RequiredPermissions = requiredPermissions;
            this.AccessToken = accessToken;
        }

        private PermissionMode Mode;
        private IEnumerable<string> RequiredPermissions;
        private string AccessToken;

        public Type Type => typeof(PermissionGateway);

        public Task<object> GetValueAsync()
        {

            return Task.FromResult<object>(new PermissionGateway(this.Mode, this.RequiredPermissions, this.AccessToken));
        }

        public string ToInvokeString() => string.Empty;

    }
}
