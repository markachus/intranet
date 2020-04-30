using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.Data.Helpers
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Revert { get; private set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            this.DestinationProperties = destinationProperties 
                ?? throw new ArgumentNullException(nameof(destinationProperties));

            this.Revert = revert;
        }
    }
}
