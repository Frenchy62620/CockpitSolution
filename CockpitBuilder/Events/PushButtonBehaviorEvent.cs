using CockpitBuilder.Common.PropertyEditors;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class PushButtonBehaviorEvent
    {
        public PushButtonType ButtonType;
        public PushButtonBehaviorEvent(PushButtonType buttontype)
        {
            ButtonType = buttontype;
        }
    }
}
