using CockpitBuilder.Common.PropertyEditors;
using CockpitBuilder.Events;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CockpitBuilder.Common.CustomControls;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Plugins.General
{
    public class PushButton_ViewModel : PluginModel, Core.Common.Events.IHandle<EditEvent>,
                                                     Core.Common.Events.IHandle<TransformEvent>,
                                                     Core.Common.Events.IHandle<SelectedEvent>,
                                                     Core.Common.Events.IHandle<NewLayoutEvent>,
                                                     Core.Common.Events.IHandle<NewAppearanceEvent>,
                                                     IPlugin
    {
        private readonly IEventAggregator eventAggregator;
        private readonly string tag;
        public PushButtonAppearanceEditorViewModel Appearance { get; }
        //public PushButtonAppearanceEditorViewModel Appearance { get; private set; }
        public LayoutPropertyEditorViewModel Layout { get; }
        public PushButtonBehaviorEditorViewModel Behavior { get; }

        public PushButton_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyEditorViewModel(eventAggregator, settings);
            Appearance = new PushButtonAppearanceEditorViewModel(eventAggregator, settings);
            Behavior = new PushButtonBehaviorEditorViewModel(eventAggregator, settings);

            base.IsUCSelected = true;

            NameUC = (string)settings[0];


            //eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { (PropertyEditorModel)Layout, Appearance, Behavior }));

            Frame = false;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

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
        //public override Point GetPosition()
        //{
        //    return new Point(Layout.UCLeft, Layout.UCTop);
        //}

        public override PropertyEditorModel[] GetProperties()
        {
            return new PropertyEditorModel[] { Layout, Appearance, Behavior };
        }

        ~PushButton_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie push");
        }
        #region Selection Image

        #endregion

        private double pluginwidth;
        public double PluginWidth
        {
            get => pluginwidth;
            set
            {
                if (pluginwidth != value)
                {
                    pluginwidth = value;
                    NotifyOfPropertyChange(() => PluginWidth);
                }
            }
        }

        private double pluginheight;
        public double PluginHeight
        {
            get => pluginheight;
            set
            {
                if (pluginheight != value)
                {
                    pluginheight = value;
                    NotifyOfPropertyChange(() => PluginHeight);
                }
            }
        }


        #region Mouse Events
        public void MouseLeftButtonDown(IInputElement elem, Point pos, MouseEventArgs e, object t, object dc)
        {
            if (IsClickCommingFromMonitorViewModel && !IsUCSelected)
            {
                eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { (PropertyEditorModel)Layout, Appearance, Behavior }));
                IsUCSelected = true;
                if (e != null)
                    e.Handled = true;
            }

            Mouse.Capture((UIElement)elem);
            //((UIElement)elem).CaptureMouse();
            var r = elem as Rectangle;
            Appearance.IndexImage = 1;
        }
        public void MouseLeftButtonUp()
        {
            Mouse.Capture(null);
            Appearance.IndexImage = 0;
        }
        public void MouseEnter(MouseEventArgs e)
        {
            /*{Appearance.Center})\n({ScaleX:0.##}*/
            //ToolTip = $"({Layout.UCLeft}, , {(ScaleX * Appearance.GlyphThickness):0.##}, {(ScaleX * Layout.Height):0.##}), Tag = {tag}";
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

        private string image;
        public string Image
        {
            get => image;
            set
            {
                image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        private string pushedimage;
        public string PushedImage
        {
            get => pushedimage;
            set
            {
                pushedimage = value;
                NotifyOfPropertyChange(() => PushedImage);
            }
        }


        #region HandleEvents
        public void Handle(EditEvent message)
        {
            Frame = !Frame;
            IsSelected = false;
        }


        public void Handle(TransformEvent translate)
        {
            //if (translate.fromProperty)
            //{
            //    UCLeft = translate.DeltaLeft;
            //    CenterCircle = translate.DeltaTop;
            //    return;
            //}
            //if (!IsSelected) return;
            //if (translate.Size == 0)
            //{
            //    UCLeft += translate.DeltaLeft;
            //    CenterCircle += translate.DeltaTop;
            //    return;
            //}
            //ScaleX = ScaleX * (translate.Size + GlyphThickness) / GlyphThickness;
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

        public void SetLayout()
        {
            throw new NotImplementedException();
        }

        public void Handle(NewLayoutEvent message)
        {
            //PluginHeight = message.NewLayoutHeight;
            //PluginWidth = message.NewLayoutWidth;
        }

        public void Handle(NewAppearanceEvent message)
        {
            //pushButtonAppearance = (PushButtonAppearance) message.Appearance;

        }
        #endregion
    }
}
