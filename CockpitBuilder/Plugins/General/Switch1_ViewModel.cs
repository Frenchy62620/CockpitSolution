using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using CockpitBuilder.Common.PropertyEditors;
using CockpitBuilder.Events;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Plugins.General
{
    public class Switch1_ViewModel : PluginModel, Core.Common.Events.IHandle<EditEvent>, 
                                                  Core.Common.Events.IHandle<TransformEvent>
    {
        private readonly IEventAggregator eventAggregator;

        public LayoutPropertyEditorViewModel Layout { get; }
        public ThreeWayToggleSwitchAppearanceEditorViewModel Appearance { get; }
        public ThreeWayToggleSwitchBehaviorEditorViewModel Behavior { get; }


        public Switch1_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyEditorViewModel(eventAggregator, settings);
            Appearance = new ThreeWayToggleSwitchAppearanceEditorViewModel(eventAggregator, settings);
            Behavior = new ThreeWayToggleSwitchBehaviorEditorViewModel(eventAggregator, settings);

            NameUC = (string)settings[1];

            //ScaleX = (double)settings[10];
            ScaleX = 1;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        #region PluginModel
        public override double Left
        {
            get => Layout.UCLeft;
            set => Layout.UCLeft = value;
        }
        public override double Top
        {
            get => Layout.UCTop;
            set => Layout.UCTop = value;
        }
        public override double Width
        {
            get => Layout.Width;
            set => Layout.Width = value;
        }
        public override double Height
        {
            get => Layout.Height;
            set => Layout.Height = value;
        }

        public override PropertyEditorModel[] GetProperties()
        {
            return new PropertyEditorModel[] { Layout, Appearance, Behavior };
        }
        #endregion

        ~Switch1_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie switch");
        }


        #region Mouse Events
        public void MouseLeftButtonDownOnUC(IInputElement elem, Point pos, MouseEventArgs e)
        {


            //e.Handled = true;
            //var r = elem as Rectangle;
        //[Description("On - On")]
        //OnOn,
        //[Description("On - Mom")]
        //OnMom,
        //[Description("Mom - On")]
        //MomOn,
        //[Description("On - On - On")]
        //OnOnOn,
        //[Description("On - On - Mom")]
        //OnOnMom,
        //[Description("Mom - On - On")]
        //MomOnOn,
        //[Description("Mom - On - Mom")]
        //MomOnMom
            switch (Behavior.SelectedSwitchTypeIndex)
            {
                case 0:
                case 1:
                case 2:
                    Appearance.IndexImage = 1 - Appearance.IndexImage;
                    return;
                default:
                    switch(Layout.AngleRotation)
                    {
                        case 0:
                        case 180:
                            if (pos.Y < Layout.Height / 2)
                                Appearance.IndexImage = Appearance.IndexImage == 1 ? 2 : 1;
                            else
                                Appearance.IndexImage = Appearance.IndexImage == 1 ? 0 : 1;
                            break;
                        case 90:
                        case 270:
                            if (pos.Y < Layout.Height / 2)
                                Appearance.IndexImage = Appearance.IndexImage == 1 ? 2 : 1;
                            else
                                Appearance.IndexImage = Appearance.IndexImage == 1 ? 0 : 1;
                            break;
                    }
                    break;
            }
             ((UIElement)elem).CaptureMouse();
        }

        public void MouseLeftButtonUp()
        {
            switch (Behavior.SelectedSwitchTypeIndex)
            {
                case 1:
                case 2:
                    Appearance.IndexImage = 1 - Appearance.IndexImage;
                    break;
                case 4:
                    if (Appearance.IndexImage == 2) Appearance.IndexImage = 1;
                    break;
                case 5:
                    if (Appearance.IndexImage == 0) Appearance.IndexImage = 1;
                    break;
                case 6:
                    Appearance.IndexImage = 1;
                    break;
            }
            Mouse.Capture(null);
        }

        public void MouseEnterInUC(MouseEventArgs e)
        {
            //ToolTip = $"({Layout.UCLeft}, {Layout.UCTop})\n({ScaleX:0.##}, {(ScaleX * Layout.Width):0.##}, {(ScaleX * Layout.Height):0.##}), Tag = {tag}";
        }
        #endregion

        #region Mode Edition
        private bool _frame;
        public bool Frame
        {
            get { return _frame; }
            set
            {
                _frame = value;
                NotifyOfPropertyChange(() => Frame);
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }
        #endregion

        #region Scale, rotation, XY translation usercontrol
        private double _scalex;
        public double ScaleX
        {
            get { return _scalex; }
            set
            {
                _scalex = value;
                NotifyOfPropertyChange(() => ScaleX);
            }
        }

        #endregion

        #region ToolTip
        private string _tooltip;
        public string ToolTip
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                NotifyOfPropertyChange(() => ToolTip);
            }
        }
        #endregion

        #region HandleEvents
        public void Handle(EditEvent message)
        {
            Frame = !Frame;
            IsSelected = false;
        }
        public void Handle(TransformEvent translate)
        {
            if (translate.fromProperty)
            {
                Layout.UCLeft = translate.DeltaLeft;
                Layout.UCTop  = translate.DeltaTop;
                return;
            }
            if (!IsSelected) return;
            if (translate.Size == 0)
            {
                Layout.UCLeft += translate.DeltaLeft;
                Layout.UCTop += translate.DeltaTop;
                return;
            }
            ScaleX = ScaleX * (translate.Size + Layout.Width) / Layout.Width;
        }
        #endregion
    }
}
