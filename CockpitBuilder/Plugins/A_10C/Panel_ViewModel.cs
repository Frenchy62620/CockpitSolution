using System.Windows;
using System.Windows.Input;
using CockpitBuilder.Common.PropertyEditors;
using CockpitBuilder.Core.Common.Events;
using CockpitBuilder.Events;

namespace CockpitBuilder.Plugins.A_10C
{
    public class Panel_ViewModel : PluginModel, IHandle<EditEvent>, IHandle<TransformEvent>, IHandle<SelectedEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly string tag;

        public Panel_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            tag = (string)settings[0];
            UCLeft = (int)settings[1];
            UCTop = (int)settings[2];
            AngleRotation = (int)settings[3];
            BackgroundImage = (string)settings[4];
            ImageSize = new double[] { (int)settings[7], (int)settings[8] };

            ScaleX = (double)settings[10];


            Frame = true;
        }
        public override double Left { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override double Top { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override PropertyEditorModel[] GetProperties()
        {
            return new PropertyEditorModel[] { };
        }
        #region Selection Image
        public string BackgroundImage { get; set; }
        public double[] ImageSize{ get; set; }
        #endregion

        #region Mouse Events
        public void MouseLeftButtonDown(IInputElement elem, Point pos, bool UpOrRight)
        {

        }
        public void MouseLeftButtonUp()
        {

        }
        public void MouseEnter(MouseEventArgs e)
        {
            ToolTip = $"({UCLeft}, {UCTop})\n({ScaleX:0.##}, {(ScaleX * ImageSize[0]):0.##}, {(ScaleX * ImageSize[1]):0.##})";
        }

        public void MouseDoubleClick(MouseEventArgs e, object datacontext)
        {
            
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
