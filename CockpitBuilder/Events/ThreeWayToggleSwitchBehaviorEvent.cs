using CockpitBuilder.Common.PropertyEditors;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class ThreeWayToggleSwitchBehaviorEvent
    {
        public bool IndicatorLight;
        public SwitchPosition DefaultPosition;
        public SwitchType SwitchType;
        public ThreeWayToggleSwitchBehaviorEvent(SwitchType switchtype, SwitchPosition defaultposition, bool indicatorlight)
        {
            SwitchType = switchtype;
            DefaultPosition = defaultposition;
            IndicatorLight = indicatorlight;
        }
    }
}
