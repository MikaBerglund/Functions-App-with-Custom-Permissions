using System;
using System.Collections.Generic;
using System.Text;

namespace Abstractions.Configuration
{
    public class ApiConfigSection
    {
        public string BaseUri { get; set; }

        public ICollection<string> Scopes { get; set; }
    }
}
