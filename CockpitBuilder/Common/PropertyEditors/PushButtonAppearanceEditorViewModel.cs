using CockpitBuilder.Events;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CockpitBuilder.Plugins.General;
using CockpitBuilder.Common.CustomControls;
using Caliburn.Micro;

namespace CockpitBuilder.Common.PropertyEditors
{
    public class PushButtonAppearanceEditorViewModel:PropertyEditorModel, Core.Common.Events.IHandle<PushButtonAppearanceEvent>,
                                                                          Core.Common.Events.IHandle<NewLayoutEvent>
       
    {
        private readonly IEventAggregator eventAggregator;

        public string NameUC { get; set; }
        public PushButtonAppearanceEditorViewModel(IEventAggregator eventAggregator, params object[] settings)
        {

            bool IsModeEditor = (bool)settings[0];
            if (IsModeEditor)
            {
                var view = ViewLocator.LocateForModel(this, null, null);
                ViewModelBinder.Bind(this, view, null);
            }

            NameUC = (string)settings[1];

            VAlignTypes = Enum.GetValues(typeof(TextVerticalAlignment)).Cast<TextVerticalAlignment>().ToList();
            HAlignTypes = Enum.GetValues(typeof(TextHorizontalAlignment)).Cast<TextHorizontalAlignment>().ToList();

            var index = 3;
            Image = ((string[])settings[index])[0];
            PushedImage = ((string[])settings[index++])[1];
            IndexImage = (int)settings[index++];

            GlyphThickness = (double)settings[index++];
            GlyphScale = (double)settings[index++];
            SelectedPushButtonGlyph = (PushButtonGlyph)(int)settings[index++];
            GlyphColor = (Color)settings[index++];
            GlyphText = "";
            TextPushOffset = "1,1";
            SelectedVAlignType = TextVerticalAlignment.Center;
            SelectedHAlignType = TextHorizontalAlignment.Center;

            TextFormat = new TextFormat(fontFamily: "Franklin Gothic",
                                        style: FontStyles.Normal,
                                        weight: FontWeights.Normal,
                                        size: 12d,
                                        padding: new double[] { 0d, 0d, 0d, 0d },
                                        horizontalAlignment: TextHorizontalAlignment.Center,
                                        verticalAlignment: TextVerticalAlignment.Center
                                       );

            TextColor = Colors.White;

            eventAggregator.Subscribe(this);
            this.eventAggregator = eventAggregator;

            Name = "Appearance";
        }

        //~PushButtonAppearanceEditorViewModel()
        //{
        //    System.Diagnostics.Debug.WriteLine("sortie pushAppearance");
        //}



        public string Name { get; set; }

        private double LayoutWidth, LayoutHeight;

        public IReadOnlyList<TextVerticalAlignment> VAlignTypes { get; }
        public IReadOnlyList<TextHorizontalAlignment> HAlignTypes { get; }

        private TextFormat textformat;
        public TextFormat TextFormat
        {
            get => textformat;

            set
            {
                textformat = value;
                NotifyOfPropertyChange(() => TextFormat);
            }
        }

        private string textPushOffset;
        public string TextPushOffset
        {
            get => textPushOffset;

            set
            {
                textPushOffset = value;
                var a = value.Split(',').Select(i => Convert.ToInt32(i)).ToArray();
                Offset = new Point(a[0], a[1]);
                OffsetX = a[0];OffsetY = a[1];
                NotifyOfPropertyChange(() => TextPushOffset);
            }
        }

        private Point offset;
        public Point Offset
        {
            get => offset;

            set
            {
                offset = value;
                NotifyOfPropertyChange(() => Offset);
            }
        }
        private double offsetY;
        public double OffsetY
        {
            get => offsetY;

            set
            {
                offsetY = value;
                NotifyOfPropertyChange(() => OffsetY);
            }
        }
        private double offsetX;
        public double OffsetX
        {
            get => offsetX;

            set
            {
                offsetX = value;
                NotifyOfPropertyChange(() => OffsetX);
            }
        }
        private TextVerticalAlignment selectedVAlignType;
        public TextVerticalAlignment SelectedVAlignType
        {
            get => selectedVAlignType;

            set
            {
                selectedVAlignType = value;
                VAlign = (VerticalAlignment)value;
                NotifyOfPropertyChange(() => SelectedVAlignType);
            }
        }
        private TextHorizontalAlignment selectedHAlignType;
        public TextHorizontalAlignment SelectedHAlignType
        {
            get => selectedHAlignType;

            set
            {
                selectedHAlignType = value;
                HAlign = (HorizontalAlignment)value;
                NotifyOfPropertyChange(() => SelectedHAlignType);
            }
        }
        private HorizontalAlignment hAlign;
        public HorizontalAlignment HAlign
        {
            get => hAlign;

            set
            {
                hAlign = value;
                NotifyOfPropertyChange(() => HAlign);
            }
        }
        private VerticalAlignment vAlign;
        public VerticalAlignment VAlign
        {
            get => vAlign;

            set
            {
                vAlign = value;
                NotifyOfPropertyChange(() => VAlign);
            }
        }
        private PushButtonGlyph selectedPushButtonGlyph;
        public PushButtonGlyph SelectedPushButtonGlyph
        {
            get => selectedPushButtonGlyph;

            set
            {
                selectedPushButtonGlyph = value;
                GlyphSelected = (int)value;
                NotifyOfPropertyChange(() => SelectedPushButtonGlyph);
            }
        }
        private int glyphSelected;
        public int GlyphSelected
        {
            get { return glyphSelected; }
            set
            {
                glyphSelected = value;
                NotifyOfPropertyChange(() => GlyphSelected);
            }
        }

        private Color _glyphColor;
        public Color GlyphColor
        {
            get => _glyphColor;
            set
            {
                _glyphColor = value;
                NotifyOfPropertyChange(() => GlyphColor);
            }
        }

        private double _glyphThickness;
        public double GlyphThickness
        {
            get => _glyphThickness;
            set
            {
                _glyphThickness = value;
                NotifyOfPropertyChange(() => GlyphThickness);
            }
        }

        private double glyphscale;
        public double GlyphScale
        {
            get => glyphscale;
            set
            {
                glyphscale = value;
                NotifyOfPropertyChange(() => GlyphScale);
            }
        }

        private string glyphText;
        public string GlyphText
        {
            get => glyphText;
            set
            {
                glyphText = value;
                NotifyOfPropertyChange(() => GlyphText);
            }
        }

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
        private int indexImage;
        public int IndexImage
        {
            get { return indexImage; }
            set
            {
                indexImage = value;
                NotifyOfPropertyChange(() => IndexImage);
            }
        }

        private Color _textColor;
        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                NotifyOfPropertyChange(() => TextColor);
            }
        }


        public void LeftPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextFormat.PaddingRight = TextFormat.PaddingLeft;
            }
        }

        public void RightPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextFormat.PaddingLeft = TextFormat.PaddingRight;
            }
        }

        public void TopPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextFormat.PaddingBottom = TextFormat.PaddingTop;
            }
        }

        public void BottomPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextFormat.PaddingTop = TextFormat.PaddingBottom;
            }
        }


 

        public void Handle(PushButtonAppearanceEvent message)
        {
            //Image = message.Image[0];
            //PushedImage = message.Image[1];
        }

        public void Handle(NewLayoutEvent message)
        {
            LayoutHeight = message.NewLayoutHeight;
            LayoutWidth = message.NewLayoutWidth;
            //DrawGlyph(GlyphSelected);
        }

        //private void SendEvent()
        //{
        //    eventAggregator.Publish(new NewAppearanceEvent(NameUC, Appearance));
        //}
    }
}
