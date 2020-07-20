using System;
using System.Collections.Generic;
using System.Text;

namespace Abstractions.Configuration
{
    public class FunctionAppConfigSection
    {
        public string AzureWebJobsStorage { get; set; }

        public ApplicationConfigSection Application { get; set; }

    }
}
