using CockpitBuilder.Events;
using CockpitBuilder.Plugins;
using CockpitBuilder.Plugins.General;
using System.Windows;
using System.Windows.Controls;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Common.PropertyEditors
{
    public class LayoutPropertyEditorViewModel:PropertyEditorModel/*, Core.Common.Events.IHandle<PropertyLayoutEvent>*/
    {
        private readonly IEventAggregator eventAggregator;
   
        public bool Linked = true;
        public LayoutPropertyEditorViewModel(IEventAggregator eventAggregator/*, params object[] settings*/)
        {
            this.eventAggregator = eventAggregator;
            Name = "Layout";
            ImageIndex = 0;
            eventAggregator.Subscribe(this);
        }
        ~LayoutPropertyEditorViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie Layout");
        }

        private string OldText;
        private string NewText;

        public string Name { get; set; }

        private string nameUC;
        public string NameUC
        {
            get => nameUC;
            set
            {
                nameUC = value;
                NotifyOfPropertyChange(() => NameUC);
            }
        }

        private double ucleft;
        public double UCLeft
        {
            get => ucleft;
            set
            {
                ucleft = value;
                NotifyOfPropertyChange(() => UCLeft);
            }
        }

        private double uctop;
        public double UCTop
        {
            get => uctop;
            set
            {
                uctop = value;
                NotifyOfPropertyChange(() => UCTop);
            }
        }

        private double width;
        public double Width
        {
            get => width;
            set
            {
                if (Linked)
                {
                    var factor = value / width;
                    Height = Height * factor;                   
                }
                width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        private double height;
        public double Height
        {
            get => height;
            set
            {
                if (Linked)
                {
                    var factor = value / Height;
                    Width = Width * factor;
                }
                height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        private int imageIndex;
        public int ImageIndex
        {
            get => imageIndex;
            set
            {
                imageIndex = value;
                NotifyOfPropertyChange(() => ImageIndex);
            }
        }
        private SwitchRotation selectedSwitchRotation;
        public SwitchRotation SelectedSwitchRotation
        {
            get => selectedSwitchRotation;
            set
            {
                selectedSwitchRotation = value;
                NotifyOfPropertyChange(() => SelectedSwitchRotation);
            }
        }

        private int angleRotation;
        public int AngleRotation
        {
            get => angleRotation;
            set
            {
                angleRotation = value;
                SelectedSwitchRotation = (SwitchRotation)value;
            }
        }

        public void ChangeLockUnlock()
        {
            ImageIndex = 1 - ImageIndex;
            Linked = !Linked;
        }

        public void GotFocus(object sender, System.EventArgs e)
        {
            OldText = (sender as TextBox).Text;
        }
        public void LostFocus(object sender, System.EventArgs e)
        {
            NewText = (sender as TextBox).Text;
        }
    }
}
