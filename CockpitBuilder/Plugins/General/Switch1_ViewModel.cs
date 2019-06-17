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
                                                  Core.Common.Events.IHandle<TransformEvent>,
                                                  Core.Common.Events.IHandle<SelectedEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly string tag;

        private readonly int deviceType;

        public ThreeWayToggleSwitchAppearanceEditorViewModel Appearance { get; }
        public LayoutPropertyEditorViewModel Layout { get; }
        private readonly ThreeWayToggleSwitchBehaviorEditorViewModel behavior;
        private readonly int nbPosition;

        private bool pushButton;
        public Switch1_ViewModel(IEventAggregator eventAggregator, LayoutPropertyEditorViewModel layout, 
                                                                   ThreeWayToggleSwitchAppearanceEditorViewModel appearance, 
                                                                   ThreeWayToggleSwitchBehaviorEditorViewModel behavior,
                                                                   params object[] settings)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            appearance.DeviceModel = this;
            behavior.DeviceModel = this;

            Layout = layout;
            Appearance = appearance;
            this.behavior = behavior;
            this.behavior.AppearancewModel = appearance;

            tag = (string)settings[0];
            layout.UCLeft = (int)settings[1];
            layout.UCTop = (int)settings[2];


            appearance.PositionImage0 = (string)settings[4];
            appearance.PositionImage1 = (string)settings[5];
            appearance.PositionImage2 = (string)settings[6];
            behavior.HasIndicator = false;
            behavior.SelectedSwitchTypeIndex = (int)settings[9];
            behavior.DefaultInitialPosition = (int)settings[11];

            layout.Width = (int)settings[7];
            layout.Height = (int)settings[8];
            layout.AngleRotation = (int)settings[3];

            ScaleX = (double)settings[10];

            nbPosition = (int)settings[12];

            eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { (PropertyEditorModel)layout, appearance, behavior }));

            Frame = true;
        }


        ~Switch1_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie switch");
        }
        #region Selection Image

        private string image0;
        public string Image0
        {
            get => image0;
            set
            {
                image0 = value;
                NotifyOfPropertyChange(() => Image0);
            }
        }
        private string image1;
        public string Image1
        {
            get => image1;
            set
            {
                image1 = value;
                NotifyOfPropertyChange(() => Image1);
            }
        }
        private string image2;
        public string Image2
        {
            get => image2;
            set
            {
                image2 = value;
                NotifyOfPropertyChange(() => Image2);
            }
        }

        private int _switchIndex;
        public int SwitchIndex
        {
            get { return _switchIndex; }
            set
            {
                _switchIndex = value;
                NotifyOfPropertyChange(() => SwitchIndex);
            }
        }
        #endregion

        #region Mouse Events
        public void MouseLeftButtonDown(IInputElement elem, Point pos, MouseEventArgs e)
        {
           if (IsClickCommingFromMonitorViewModel)
                eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { (PropertyEditorModel)Layout, Appearance, behavior }));

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
            switch (behavior.SelectedSwitchTypeIndex)
            {
                case 0:
                case 1:
                case 2:
                    SwitchIndex = 1 - SwitchIndex;
                    return;
                default:
                    switch(Layout.AngleRotation)
                    {
                        case 0:
                        case 180:
                            if (pos.Y < Layout.Height / 2)
                                SwitchIndex = SwitchIndex == 1 ? 2 : 1;
                            else
                                SwitchIndex = SwitchIndex == 1 ? 0 : 1;
                            break;
                        case 90:
                        case 270:
                            if (pos.Y < Layout.Height / 2)
                                SwitchIndex = SwitchIndex == 1 ? 2 : 1;
                            else
                                SwitchIndex = SwitchIndex == 1 ? 0 : 1;
                            break;
                    }
                    break;
            }
             ((UIElement)elem).CaptureMouse();
        }

        public void MouseLeftButtonUp()
        {
            switch (behavior.SelectedSwitchTypeIndex)
            {
                case 1:
                case 2:
                    SwitchIndex = 1 - SwitchIndex;
                    break;
                case 4:
                    if (SwitchIndex == 2) SwitchIndex = 1;
                    break;
                case 5:
                    if (SwitchIndex == 0) SwitchIndex = 1;
                    break;
                case 6:
                    SwitchIndex = 1;
                    break;
            }
            Mouse.Capture(null);
        }

        public void MouseEnter(MouseEventArgs e)
        {
            ToolTip = $"({Layout.UCLeft}, {Layout.UCTop})\n({ScaleX:0.##}, {(ScaleX * Layout.Width):0.##}, {(ScaleX * Layout.Height):0.##}), Tag = {tag}";
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
        public void Handle(SelectedEvent message)
        {
            if (string.IsNullOrEmpty(message.Tag))
            {
                IsSelected = false;
                return;
            }

            if (message.Tag[0] != '+')
            {
                IsSelected = message.Tag.Equals(tag);
                return;
            }

            if (tag.Equals(message.Tag.Substring(1)) && message.Tag[0] == '+')
            {
                IsSelected = !IsSelected;
            }
        }
        #endregion
    }
}
