using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageStore.Dashboard.Configuration
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string MessageStoreApiUrl { get; set; }
    }
}
