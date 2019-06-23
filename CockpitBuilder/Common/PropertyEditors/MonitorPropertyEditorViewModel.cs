using Caliburn.Micro;
using CockpitBuilder.Events;
using System.Windows.Media;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Common.PropertyEditors
{
    public class MonitorPropertyEditorViewModel:PropertyEditorModel, Core.Common.Events.IHandle<PropertyMonitorEvent>
    {
        private readonly SolidColorBrush color1 = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush color2 = new SolidColorBrush(Colors.LightGray);

        private readonly IEventAggregator eventAggregator;
        public MonitorPropertyEditorViewModel(IEventAggregator eventAggregator)
        {
            var view = ViewLocator.LocateForModel(this, null, null);
            ViewModelBinder.Bind(this, view, null);

            this.eventAggregator = eventAggregator;
            Name = "Monitor";
            FillBackground = false;
            BackgroundColor = Colors.Gray;
            BackgroundColor1 = color1;
            BackgroundColor2 = color2;

            eventAggregator.Subscribe(this);
        }

        public string Name { get; set; }

        private bool alwaysOnTop;
        public bool AlwaysOnTop
        {
            get { return alwaysOnTop; }
            set
            {
                alwaysOnTop = value;
                NotifyOfPropertyChange(() => AlwaysOnTop);
            }
        }

        private string backgroundImage;
        public string BackgroundImage
        {
            get { return backgroundImage; }
            set
            {
                backgroundImage = value;
                NotifyOfPropertyChange(() => BackgroundImage);
                eventAggregator.Publish(new BackgroundImageEvent(value));
            }
        }

        private bool fillBackground;
        public bool FillBackground
        {
            get { return fillBackground; }
            set {
                fillBackground = value;
                NotifyOfPropertyChange(() => FillBackground);
                var b = new SolidColorBrush(BackgroundColor);
                BackgroundColor1 = value ? b : color1;
                BackgroundColor2 = value ? b : color2;
            }
        }

        private Color backgroundColor;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                NotifyOfPropertyChange(() => BackgroundColor);
            }
        }

        private SolidColorBrush backgroundColor1;
        public SolidColorBrush BackgroundColor1
        {
            get { return backgroundColor1; }
            set
            {
                backgroundColor1 = value;
                NotifyOfPropertyChange(() => BackgroundColor1);
            }
        }
        private SolidColorBrush backgroundColor2;
        public SolidColorBrush BackgroundColor2
        {
            get { return backgroundColor2; }
            set
            {
                backgroundColor2 = value;
                NotifyOfPropertyChange(() => BackgroundColor2);
            }
        }


        public void Handle(PropertyMonitorEvent message)
        {
            BackgroundImage = message.ImageBackground;
            BackgroundColor = message.ColorBackground;
            FillBackground = message.Fill;
            AlwaysOnTop = message.AlwaysOnTop;
        }
    }
}
