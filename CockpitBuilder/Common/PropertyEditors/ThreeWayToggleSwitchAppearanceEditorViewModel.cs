using CockpitBuilder.Events;
using CockpitBuilder.Plugins.General;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Common.PropertyEditors
{
    //,
    //                                Core.Common.Events.IHandle<ThreeWayToggleSwitchAppearanceEvent>,
    //                                Core.Common.Events.IHandle<PropertyHasIndicatorEvent>,
    //                                Core.Common.Events.IHandle<PropertyHas3ImagesEvent>
    public class ThreeWayToggleSwitchAppearanceEditorViewModel : PropertyEditorModel
    {
        private readonly IEventAggregator eventAggregator;

        public ThreeWayToggleSwitchAppearanceEditorViewModel(IEventAggregator eventAggregator/*, int tag*/)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            PositionIndicatorImage0 = "";
            PositionIndicatorImage1 = "";
            PositionIndicatorImage2 = "";
            Has3Images = false;
            Name = "Appareance";
        }

        public Switch1_ViewModel DeviceModel;

        public string Name { get; set; }

        private string positionImage0;
        public string PositionImage0
        {
            get => positionImage0;
            set
            {
                positionImage0 = value;
                DeviceModel.Image0 = value;
                NotifyOfPropertyChange(() => PositionImage0);
            }
        }

        private string positionImage1;
        public string PositionImage1
        {
            get => positionImage1;
            set
            {
                positionImage1 = value;
                DeviceModel.Image1 = value;
                NotifyOfPropertyChange(() => PositionImage1);
            }
        }

        private string positionImage2;
        public string PositionImage2
        {
            get => positionImage2;
            set
            {
                positionImage2 = value;
                DeviceModel.Image2 = value;
                NotifyOfPropertyChange(() => PositionImage2);
            }
        }

        private string positionIndicatorImage0;
        public string PositionIndicatorImage0
        {
            get => positionIndicatorImage0;
            set
            {
                positionIndicatorImage0 = value;
                NotifyOfPropertyChange(() => PositionIndicatorImage0);
            }
        }

        private string positionIndicatorImage1;
        public string PositionIndicatorImage1
        {
            get => positionIndicatorImage1;
            set
            {
                positionIndicatorImage1 = value;
                NotifyOfPropertyChange(() => PositionIndicatorImage1);
            }
        }

        private string positionIndicatorImage2;
        public string PositionIndicatorImage2
        {
            get => positionIndicatorImage2;
            set
            {
                positionIndicatorImage2 = value;
                NotifyOfPropertyChange(() => PositionIndicatorImage2);
            }
        }

        private bool hasIndicator;
        public bool HasIndicator
        {
            get => hasIndicator;
            set
            {
                hasIndicator = value;
                NotifyOfPropertyChange(() => HasIndicator);
            }
        }
        private bool has3Images;
        public bool Has3Images
        {
            get => has3Images;
            set
            {
                has3Images = value;
                NotifyOfPropertyChange(() => Has3Images);
            }
        }
        //public void Handle(PropertyHasIndicatorEvent message)
        //{
        //    HasIndicator = message.HasIndicator;
        //}

        //public void Handle(PropertyHas3ImagesEvent message)
        //{
        //    Has3Images = message.Has3Images;
        //}

        //public void Handle(ThreeWayToggleSwitchAppearanceEvent message)
        //{
        //    HasIndicator = message.IndicatorLight;
        //    Has3Images = message.Has3Images;
        //    PositionImage0 = message.PositionOnImage[0];
        //    PositionImage1 = message.PositionOnImage[1];
        //    PositionImage2 = message.PositionOnImage[2];
        //    PositionIndicatorImage0 = message.PositionIndicatorOnImage[0];
        //    PositionIndicatorImage1 = message.PositionIndicatorOnImage[1];
        //    PositionIndicatorImage2 = message.PositionIndicatorOnImage[2];
        //}
    }

}
