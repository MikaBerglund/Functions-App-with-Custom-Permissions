using System;
using System.Collections.Generic;
using System.Text;

namespace Abstractions.Configuration
{
    public class ApplicationConfigSection
    {

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string TenantId { get; set; }

    }
}
