using System.Windows;
using System.Windows.Input;
using CockpitBuilder.Core.Common.Events;
using CockpitBuilder.Events;

namespace CockpitBuilder.Plugins.General
{
    public class Switch_ViewModel : PluginModel, IHandle<EditEvent>, IHandle<TransformEvent>, IHandle<SelectedEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly string tag;

        private readonly int deviceType;

        private readonly int nbPosition;

        private readonly bool pushButton;
        public Switch_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            tag = (string)settings[0];
            UCLeft = (int)settings[1];
            UCTop = (int)settings[2];
            AngleRotation = (int)settings[3];
            SwitchImage = new string[] { (string)settings[4], (string)settings[5], (string)settings[6] };
            ImageSize = new double[] { (int)settings[7], (int)settings[8] };
            deviceType = (int)settings[9];
            ScaleX = (double)settings[10];

            SwitchIndex = (int)settings[11];
            nbPosition = (int)settings[12];

            //this.pushButton = deviceType < (int)EnumDevices.DevicesType.EndOfPush;

            Frame = false;
        }
        ~Switch_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie switch");
        }
        #region Selection Image
        public string[] SwitchImage { get; set; }
        public double[] ImageSize{ get; set; }
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
        public void MouseLeftButtonDown(IInputElement elem, Point pos, bool UpOrRight)
        {
            switch (nbPosition)
            {
                case 2:
                    SwitchIndex = 1 - SwitchIndex;
                    return;
                case 3:
                    if (UpOrRight)
                        SwitchIndex = SwitchIndex == 1 ? 2 : 1;
                    else
                        SwitchIndex = SwitchIndex == 1 ? 0 : 1;
                    break;
            }
             //((UIElement)elem).CaptureMouse();
            Mouse.Capture((UIElement)elem);
            if (tag == "1_EXIT")
            {
                eventAggregator.Publish(new VisibilityPanelEvent(tagpanel: "1", visible: false));
                Mouse.Capture((UIElement)elem);
                //((UIElement)elem).CaptureMouse();
            }
        }
        public void MouseLeftButtonUp()
        {

            if (pushButton)                        // pushButton?
                SwitchIndex = nbPosition == 3 ? 1 : 0;  //  yes return to its initial position middle or down
            //if (tag == "1_EXIT")
            //{
            //    eventAggregator.Publish(new VisibilityPanelEvent(tagpanel: "1", visible: false));
            //}
            Mouse.Capture(null);
        }
        public void MouseEnter(MouseEventArgs e)
        {
            ToolTip = $"({UCLeft}, {UCTop})\n({ScaleX:0.##}, {(ScaleX * ImageSize[0]):0.##}, {(ScaleX * ImageSize[1]):0.##}), Tag = {tag}";
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
        private double _angleRotation;
        public double AngleRotation
        {
            get { return _angleRotation; }
            set
            {
                _angleRotation = value;
                NotifyOfPropertyChange(() => AngleRotation);
            }
        }
        private double _ucleft;
        public double UCLeft
        {
            get { return _ucleft; }
            set
            {
                _ucleft = value;
                NotifyOfPropertyChange(() => UCLeft);
            }
        }
        private double _uctop;
        public double UCTop
        {
            get { return _uctop; }
            set
            {
                _uctop = value;
                NotifyOfPropertyChange(() => UCTop);
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
                UCLeft = translate.DeltaLeft;
                UCTop  = translate.DeltaTop;
                return;
            }
            if (!IsSelected) return;
            if (translate.Size == 0)
            {
                UCLeft += translate.DeltaLeft;
                UCTop += translate.DeltaTop;
                return;
            }
            ScaleX = ScaleX * (translate.Size + ImageSize[0]) / ImageSize[0];
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
