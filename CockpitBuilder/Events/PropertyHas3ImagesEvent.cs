using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class PropertyHas3ImagesEvent
    {
        public bool Has3Images;
        public PropertyHas3ImagesEvent(bool has3images)
        {
            Has3Images = has3images;
        }
    }
}
