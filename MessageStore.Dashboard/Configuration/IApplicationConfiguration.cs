using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageStore.Dashboard.Configuration
{
    public interface IApplicationConfiguration
    {
        string MessageStoreApiUrl { get; set; }
    }
}
