using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class PropertyMonitorEvent
    {
        public bool AlwaysOnTop;
        public bool Fill;
        public string ImageBackground;
        public Color ColorBackground;
        public PropertyMonitorEvent(bool alwaysontop, bool fill, string imagebackground, Color colorbackground)
        {
            AlwaysOnTop = alwaysontop;
            Fill = fill;
            ImageBackground = imagebackground;
            ColorBackground = colorbackground;
        }
    }
}
