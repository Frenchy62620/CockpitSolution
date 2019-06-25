using CockpitBuilder.Core.Common.Events;
using CockpitBuilder.Plugins.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockpitBuilder.Common.PropertyEditors
{
    public class PushButtonBehaviorEditorViewModel:PropertyEditorModel
    {
        private readonly IEventAggregator eventAggregator;
        public PushButtonBehaviorEditorViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            PushButtonTypes = Enum.GetValues(typeof(PushButtonType)).Cast<PushButtonType>().ToList();
            //SelectedPushButtonType = PushButtonType.Toggle;
            Name = "Behavior";
        }

        ~PushButtonBehaviorEditorViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie pushBehaviour");
        }

        public string Name { get; set; }

        public IReadOnlyList<PushButtonType> PushButtonTypes { get; }

        private PushButtonType selectedPushButtonType;
        public PushButtonType SelectedPushButtonType
        {
            get => selectedPushButtonType;

            set
            {
                selectedPushButtonType = value;
                NotifyOfPropertyChange(() => SelectedPushButtonType);
            }
        }

        private int pushButtonTypeIndex;
        public int PushButtonTypeIndex
        {
            get => pushButtonTypeIndex;

            set
            {
                pushButtonTypeIndex = value;
                SelectedPushButtonType = (PushButtonType)value;
            }
        }
    }
}
