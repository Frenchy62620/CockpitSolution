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
        //public abstract double Angle { get; set; }
        public abstract PropertyEditorModel[] GetProperties();

        private double zoomfactorfrompluginmodel;
        public double ZoomFactorFromPluginModel
        {
            get => zoomfactorfrompluginmodel;
            set
            {
                zoomfactorfrompluginmodel = value;
                NotifyOfPropertyChange(() => ZoomFactorFromPluginModel);
            }
        }
        //private double angleRot;
        //public double AngleRot
        //{
        //    get => angleRot;
        //    set
        //    {
        //        angleRot = value;
        //        NotifyOfPropertyChange(() => AngleRot);
        //    }
        //}
        //public bool IsClickCommingFromMonitorViewModel;
        public string NameUC;
    }
}
