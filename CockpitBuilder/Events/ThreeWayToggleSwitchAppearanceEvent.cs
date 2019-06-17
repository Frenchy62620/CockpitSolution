using CockpitBuilder.Common.PropertyEditors;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class ThreeWayToggleSwitchAppearanceEvent
    {
        public bool IndicatorLight;
        public string[] PositionIndicatorOnImage;
        public string[] PositionOnImage;
        public bool Has3Images;
        public ThreeWayToggleSwitchAppearanceEvent(string[] positionimage, string[] positionindicatorimage, bool indicatorlight, bool has3images)
        {
            IndicatorLight= indicatorlight;
            PositionOnImage = positionimage;
            PositionIndicatorOnImage = positionindicatorimage;
            Has3Images = has3images;
        }
    }
}
