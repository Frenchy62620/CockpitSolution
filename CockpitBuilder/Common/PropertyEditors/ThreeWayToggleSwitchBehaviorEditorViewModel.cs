using Caliburn.Micro;
using CockpitBuilder.Events;
using CockpitBuilder.Plugins.General;
using System;
using System.Collections.Generic;
using System.Linq;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Common.PropertyEditors
{
    public class ThreeWayToggleSwitchBehaviorEditorViewModel : PropertyEditorModel/*, Core.Common.Events.IHandle<ThreeWayToggleSwitchBehaviorEvent>*/
    {
        private readonly IEventAggregator eventAggregator;

        public ThreeWayToggleSwitchBehaviorEditorViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            var index = 0;
            bool IsModeEditor = (bool)settings[index++];
            if (IsModeEditor)
            {
                var view = ViewLocator.LocateForModel(this, null, null);
                ViewModelBinder.Bind(this, view, null);
            }

            this.eventAggregator = eventAggregator;

            SwitchOrientation = Enum.GetValues(typeof(SwitchOrientation)).Cast<SwitchOrientation>().ToList();

            eventAggregator.Subscribe(this);

            Name = "Behavior";
        }

        public string Name { get; set; }

        private int selectedSwitchTypeIndex;
        public int SelectedSwitchTypeIndex
        {
            get => selectedSwitchTypeIndex;
            set
            {
                selectedSwitchTypeIndex = value;
                SetNumberOfPosition(value >= 3);
                Has3Images = value >= 3;
                NotifyOfPropertyChange(() => SelectedSwitchTypeIndex);
            }
        }

        public IReadOnlyList<SwitchOrientation> SwitchOrientation { get; }

        private SwitchOrientation selectedOrientation;
        public SwitchOrientation SelectedOrientation
        {
            get => selectedOrientation;
            set
            {
                selectedOrientation = value;
                NotifyOfPropertyChange(() => SelectedOrientation);
            }
        }


        private List<SwitchPosition> defaultPositions;
        public List<SwitchPosition> DefaultPositions
        {
            get => defaultPositions;
            set
            {
                defaultPositions = value;
                NotifyOfPropertyChange(() => DefaultPositions);
            }
        }

        private int defaultInitialPosition;
        public int DefaultInitialPosition
        {
            get => defaultInitialPosition;

            set
            {
                //if (!AppearancewModel.Has3Images && value > 1)
                //    defaultInitialPosition = 1;
                //else
                //    defaultInitialPosition = value;


                SelectedDefaultPosition = (SwitchPosition)defaultInitialPosition;
            }
        }

        private SwitchPosition selectedDefaultPosition;
        public SwitchPosition SelectedDefaultPosition
        {
            get => selectedDefaultPosition;

            set
            {
                selectedDefaultPosition = value;
                NotifyOfPropertyChange(() => SelectedDefaultPosition);
            }
        }

        void SetNumberOfPosition(bool numberOfPositionEqual3)
        {
            DefaultPositions = Enum.GetValues(typeof(SwitchPosition)).Cast<SwitchPosition>().Take(numberOfPositionEqual3 ? 3 : 2).ToList();
            if (SelectedSwitchTypeIndex == 3 || (SelectedDefaultPosition == SwitchPosition.Two && !numberOfPositionEqual3))
                SelectedDefaultPosition = DefaultPositions[1];
        }

        private bool hasIndicator;
        public bool HasIndicator
        {
            get => hasIndicator;
            set
            {
                hasIndicator = value;
                //AppearancewModel.HasIndicator = value;

                NotifyOfPropertyChange(() => HasIndicator);
            }
        }
        private bool has3images;
        public bool Has3Images
        {
            get => has3images;
            set
            {
                has3images = value;
                //AppearancewModel.HasIndicator = value;

                NotifyOfPropertyChange(() => Has3Images);
            }
        }
    }
}
