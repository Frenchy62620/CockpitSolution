using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class BackgroundColorEvent
    {
        public SolidColorBrush backgroundcolor;
        public bool resetcolor;
        public BackgroundColorEvent(Color Backgroundcolor, bool ResetColor = false)
        {
            resetcolor = ResetColor;
            if (!resetcolor)
                backgroundcolor = new SolidColorBrush(Backgroundcolor);
        }
    }
}
