using CockpitBuilder.Events;
using System.Windows.Media;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Common.PropertyEditors
{
    public class MonitorPropertyEditorViewModel:PropertyEditorModel, Core.Common.Events.IHandle<PropertyMonitorEvent>
    {
        private readonly IEventAggregator eventAggregator;
        public MonitorPropertyEditorViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            Name = "Monitor";
            FillBackground = false;
            BackgroundColor = Colors.Gray;
            eventAggregator.Subscribe(this);
        }

        public string Name { get; set; }
        //private string name;
        //public string Name
        //{
        //    get => name;
        //    set
        //    {
        //        NotifyOfPropertyChange(() => Name);
        //        name = value;
        //    }
        //}

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
                eventAggregator.Publish(new BackgroundColorEvent(BackgroundColor, !value));
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

        public void Handle(PropertyMonitorEvent message)
        {
            BackgroundImage = message.ImageBackground;
            BackgroundColor = message.ColorBackground;
            FillBackground = message.Fill;
            AlwaysOnTop = message.AlwaysOnTop;
        }
    }
}
