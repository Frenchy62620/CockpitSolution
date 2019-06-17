using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class DisplayPropertiesViewEvent
    {
        public string[] Views;
        public DisplayPropertiesViewEvent(string[] views)
        {
            Views = views;
        }
    }
}
