using CockpitBuilder.Common.PropertyEditors;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class PushButtonAppearanceEvent
    {
        public string[] Image;
        public Color GlyphColor;
        public Color TextColor;
        public PushButtonAppearanceEvent(string[] image, Color glyphColor, Color textColor)
        {
            Image = image;

            GlyphColor = glyphColor;
            TextColor = textColor;
        }
    }
}
