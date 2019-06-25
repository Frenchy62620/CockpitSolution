using Caliburn.Micro;
using CockpitBuilder.Common.PropertyEditors;
using System.Windows;

namespace CockpitBuilder.Plugins
{
    public abstract class PluginModel : PropertyChangedBase
    {
        public abstract double Width { get; set; }
        public abstract double Height { get; set; }
        public abstract double Left { get; set; }
        public abstract double Top { get; set; }
        public abstract PropertyEditorModel[] GetProperties();

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
        //private bool isucSelected;
        //public bool IsUCSelected
        //{
        //    get => isucSelected;
        //    set
        //    {
        //        isucSelected = value;
        //        NotifyOfPropertyChange(() => IsUCSelected);
        //    }
        //}
        public bool IsClickCommingFromMonitorViewModel;
        public string NameUC;
    }
}
