using Caliburn.Micro;
using CockpitBuilder.Events;
using CockpitBuilder.Plugins;
using CockpitBuilder.Plugins.General;
using System;
using System.Windows;
using System.Windows.Controls;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Common.PropertyEditors
{
    public class LayoutPropertyEditorViewModel:PropertyEditorModel/*, Core.Common.Events.IHandle<PropertyLayoutEvent>*/
    {
        private readonly IEventAggregator eventAggregator;
   
        public bool Linked = true;
        public double Factor;
        public LayoutPropertyEditorViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            var view = ViewLocator.LocateForModel(this, null, null);
            ViewModelBinder.Bind(this, view, null);

            NameUC = (string)settings[0];
            UCLeft = ((int[])settings[1])[0];
            UCTop = ((int[])settings[1])[1];
            
            var width = (double)((int[])settings[1])[2];
            var height = (double)((int[])settings[1])[3];

            Factor = height / width;

            Width = width;
            Height = height;

            AngleRotation = ((int[])settings[1])[4];

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
                if (value != Width)
                {
                    width = value;
                    if (Linked) Height = Math.Round(value * Factor, 0, MidpointRounding.ToEven);
                    NotifyOfPropertyChange(() => Width);
                }
            }
        }

        private double height;
        public double Height
        {
            get => height;
            set
            {
                if (value != Height)
                {
                    height = value;
                    if (Linked) Width = Math.Round(value / Factor, 0, MidpointRounding.ToEven);
                    NotifyOfPropertyChange(() => Height);
                }
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
            if (Linked) Factor = Height / Width;
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
