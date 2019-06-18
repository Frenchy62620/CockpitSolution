using System;
using System.Windows;
using System.Windows.Media;
using CockpitBuilder.Core.Common.Events;
using CockpitBuilder.Events;
using CockpitBuilder.Views.Main.DockPanel.Models;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    public class PreviewTabViewModel : PanelViewModel, IHandle<DragSelectedItemEvent>, Core.Common.Events.IHandle<MonitorViewStartedEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly SolidColorBrush color1 = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush color2 = new SolidColorBrush(Colors.LightGray);
        public double ScrollWidth;
        public double ScrollHeight;

        public MonitorViewModel MonitorViewModel{ get; set; }

        public PreviewTabViewModel(IEventAggregator eventAggregator)
        {
            DisplayManager DisplayManager = new DisplayManager();
            MonitorCollection mc = DisplayManager.Displays;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            BackgroundColor1 = color1;
            BackgroundColor2 = color2;
            Title = "Preview";
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            var d = GetView() as PreviewTabView;
            ScrollWidth = d.PreviewScroller.ActualWidth;
            ScrollHeight = d.PreviewScroller.ActualHeight;
            //System.Diagnostics.Debug.WriteLine($"h = {d.ActualHeight} w= {d.ActualWidth}");
            ZoomFactor = Math.Min(ScrollWidth / 1920d, ScrollHeight / 1080d);
            SetZoomFactor(ZoomFactor);
            PreviewWidth = ZoomFactor * 1920d;
            PreviewHeight = ZoomFactor * 1080d;

        }
        private double zoomlevel;
        public double ZoomLevel
        {
            get => zoomlevel;
            set
            {
                zoomlevel = value;
                NotifyOfPropertyChange(() => ZoomLevel);
            }
        }

        private double zoomfactor;
        public double ZoomFactor
        {
            get => zoomfactor;
            set
            {
                zoomfactor = value;
            }
        }
        private double previewheight;
        public double PreviewHeight
        {
            get => previewheight;
            set
            {
                previewheight = value;
                NotifyOfPropertyChange(() => PreviewHeight);
            }
        }
        private double previewwidth;
        public double PreviewWidth
        {
            get => previewwidth;
            set
            {
                previewwidth = value;
                NotifyOfPropertyChange(() => PreviewWidth);
            }
        }
        private bool showPanels;
        public bool ShowPanels
        {
            get => showPanels;
            set
            {
                showPanels = value;
                NotifyOfPropertyChange(() => ShowPanels);
            }
        }

        private bool fullSize;
        public bool FullSize
        {
            get => fullSize;
            set
            {
                fullSize = value;
                ZoomPanelVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                if (value)
                {
                    SetZoomFactor(1d);
                    PreviewWidth =  1920d;
                    PreviewHeight = 1080d;
                }
                else
                {
                    ZoomFactor = Math.Min(ScrollWidth / 1920d, ScrollHeight / 1080d);
                    SetZoomFactor(ZoomFactor);
                    PreviewWidth = ZoomFactor * 1920d;
                    PreviewHeight = ZoomFactor * 1080d;
                }
                NotifyOfPropertyChange(() => FullSize);
            }
        }
        private Visibility zoomPanelVisibility;
        public Visibility ZoomPanelVisibility
        {
            get => zoomPanelVisibility;
            set
            {
                zoomPanelVisibility = value;
                NotifyOfPropertyChange(() => ZoomPanelVisibility);
            }
        }
        private SolidColorBrush backgroundColor1;
        public SolidColorBrush BackgroundColor1
        {
            get => backgroundColor1;
            set
            {
                backgroundColor1 = value;
                NotifyOfPropertyChange(() => BackgroundColor1);
            }
        }
        private SolidColorBrush backgroundColor2;
        public SolidColorBrush BackgroundColor2
        {
            get => backgroundColor2;
            set
            {
                backgroundColor2 = value;
                NotifyOfPropertyChange(() => BackgroundColor2);
            }
        }

        public void ScrollViewerSizeChanged(SizeChangedEventArgs e)
        {
            ScrollWidth = e.NewSize.Width;
            ScrollHeight = e.NewSize.Height;
            System.Diagnostics.Debug.WriteLine($"hs = {e.NewSize.Height} ws= {e.NewSize.Width}");
        }

        public void MainGrid_SizeChanged(System.Windows.SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"h = {e.NewSize.Height} w= {e.NewSize.Width}");
        }


        public void OnValuechanged(RoutedPropertyChangedEventHandler<double> v)
        {

        }
        public void Handle(DragSelectedItemEvent message)
        {
            System.Diagnostics.Debug.WriteLine($"image = {message.FullImageName}");
        }

        public void Handle(MonitorViewStartedEvent message)
        {
             MonitorViewModel =  message.Monitorviewmodel;
        }

        public void SetZoomFactor(double value)
        {
            MonitorViewModel.ZoomFactor = value;
            foreach (var p in MonitorViewModel.MyCockpitViewModels)
            {
                p.ZoomFactor = value;
            }
        }
    }
}
