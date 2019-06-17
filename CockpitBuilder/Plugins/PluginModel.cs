using Caliburn.Micro;
using System.Windows;

namespace CockpitBuilder.Plugins
{
    public abstract class PluginModel : PropertyChangedBase
    {
        private double zoomfactor;
        public double ZoomFactor
        {
            get => zoomfactor;
            set
            {
                zoomfactor = value;
                NotifyOfPropertyChange(() => ZoomFactor);
            }
        }
        private bool isucSelected;
        public bool IsUCSelected
        {
            get => isucSelected;
            set
            {
                isucSelected = value;
                NotifyOfPropertyChange(() => IsUCSelected);
            }
        }
        public bool IsClickCommingFromMonitorViewModel;
        public string NameUC;
    }
}
