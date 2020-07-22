using System;
using System.Collections.Generic;
using System.Text;

namespace Abstractions.Configuration
{
    public class RootConfigSection
    {
        public ApiConfigSection Api { get; set; }

        public ApplicationConfigSection Application { get; set; }
    }
}
