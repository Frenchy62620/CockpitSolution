using CockpitBuilder.Core.Contracts;

namespace CockpitBuilder.Common.PropertyEditors
{
    
    [PropertyEditor("Helios.Base.PushButton", "Behavior")]
    [PropertyEditor("Helios.Base.IndicatorPushButton", "Behavior")]
    public partial class PushButtonBehaviorEditorView
    {
        public PushButtonBehaviorEditorView()
        {
            InitializeComponent();
        }
    }
}
