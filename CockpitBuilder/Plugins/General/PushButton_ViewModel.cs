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
        public LayoutPropertyEditorViewModel Layout { get; }
        public  PushButtonBehaviorEditorViewModel behavior;
        public PushButtonAppearance pushButtonAppearance { get; private set; }
        public PushButton_ViewModel(IEventAggregator eventAggregator, 
                                    LayoutPropertyEditorViewModel layout, 
                                    PushButtonAppearanceEditorViewModel appearance,
                                    PushButtonBehaviorEditorViewModel behavior,
                                    params object[] settings)
        {
            //layout.DeviceModel = this;
            //behavior.DeviceModel = this;
            //TextFormat TextFormat = new TextFormat(fontFamily: "Franklin Gothic",
            //                                       style: FontStyles.Normal,
            //                                       weight: FontWeights.Normal,
            //                                       size: 12d,
            //                                       padding: new double[] { 0d, 0d, 0d, 0d },
            //                                       horizontalAlignment: TextHorizontalAlignment.Center,
            //                                       verticalAlignment: TextVerticalAlignment.Center);

            base.IsUCSelected = true;
            //appearance.Layout = layout;
            this.behavior = behavior;
            NameUC = (string)settings[0];
            layout.NameUC = NameUC;
            layout.UCLeft = (int)settings[1];
            layout.UCTop = (int)settings[2];
            layout.Width = (int)settings[6];
            layout.Height = (int)settings[7];
            layout.AngleRotation = (int)settings[3];

            PluginWidth = (int)settings[6];
            PluginHeight = (int)settings[7];

            appearance.Image = (string)settings[4];
            appearance.PushedImage = (string)settings[5];
            appearance.SelectedPushButtonGlyph = (PushButtonGlyph)1;
            appearance.GlyphScale = 0.8d;
            appearance.Center = new Point(layout.Width / 2d, layout.Height / 2d);
            appearance.RadiusX = Math.Min(layout.Width, layout.Height) / 2d * appearance.GlyphScale;

            appearance.GlyphColor = Colors.White;
            appearance.GlyphThickness = 2;
            appearance.TextColor = Colors.White;
            appearance.TextFormat.FontSize = 20;
            appearance.TextPushOffset = "10,1";
            behavior.PushButtonTypeIndex = (int)settings[8];




            ScaleX = (double)settings[9];

            //appearance.IndexImage = (int)settings[10];

            pushButtonAppearance = new PushButtonAppearance(nameUC: (string)settings[0], 
                                                            images: new string[] { (string)settings[4], (string)settings[5] }, 
                                                            startimageposition: (int)settings[10], 
                                                            textformat: new TextFormat(fontFamily: "Franklin Gothic",
                                                                                       style: FontStyles.Normal,
                                                                                       weight: FontWeights.Normal,
                                                                                       size: 12d,
                                                                                       padding: new double[] { 0d, 0d, 0d, 0d },
                                                                                       horizontalAlignment: TextHorizontalAlignment.Center,
                                                                                       verticalAlignment: TextVerticalAlignment.Center
                                                                                      )
                                                           );

            eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { (PropertyEditorModel)layout, appearance, behavior }));

            Frame = false;

            Layout = layout;
            Appearance = appearance;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
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
                eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { (PropertyEditorModel)Layout, Appearance, behavior }));
                IsUCSelected = true;
                if (e != null)
                    e.Handled = true;
            }

            Mouse.Capture((UIElement)elem);
            //((UIElement)elem).CaptureMouse();
            var r = elem as Rectangle;
            pushButtonAppearance.IndexImage = 1;
        }
        public void MouseLeftButtonUp()
        {
            Mouse.Capture(null);
            pushButtonAppearance.IndexImage = 0;
        }
        public void MouseEnter(MouseEventArgs e)
        {
            ToolTip = $"({Layout.UCLeft}, {Appearance.Center})\n({ScaleX:0.##}, {(ScaleX * Appearance.GlyphThickness):0.##}, {(ScaleX * Layout.Height):0.##}), Tag = {tag}";
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
            pushButtonAppearance = (PushButtonAppearance) message.Appearance;

        }
        #endregion
    }
}
