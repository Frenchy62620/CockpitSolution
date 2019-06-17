using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class PropertyHasIndicatorEvent
    {
        public bool HasIndicator;
        public PropertyHasIndicatorEvent(bool hasindicator)
        {
            HasIndicator = hasindicator;
        }
    }
}
