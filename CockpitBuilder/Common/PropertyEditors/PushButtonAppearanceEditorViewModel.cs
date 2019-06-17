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

namespace CockpitBuilder.Common.PropertyEditors
{
    public class PushButtonAppearanceEditorViewModel:PropertyEditorModel, Core.Common.Events.IHandle<PushButtonAppearanceEvent>
    {
        LayoutPropertyEditorViewModel layout;
        public PushButtonAppearanceEditorViewModel(IEventAggregator eventAggregator)
        {
            VAlignTypes = Enum.GetValues(typeof(TextVerticalAlignment)).Cast<TextVerticalAlignment>().ToList();
            HAlignTypes = Enum.GetValues(typeof(TextHorizontalAlignment)).Cast<TextHorizontalAlignment>().ToList();
            SelectedVAlignType = TextVerticalAlignment.Center;
            SelectedHAlignType = TextHorizontalAlignment.Center;
            TextFormat = new TextFormat();
            Name = "Appearance";

            //TextColor = Colors.White;

            eventAggregator.Subscribe(this);
        }

        ~PushButtonAppearanceEditorViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie pushAppearance");
        }

        //public PushButtonAppearanceEditorViewModel AppearancewModel;
        public LayoutPropertyEditorViewModel Layout;

        public string Name { get; set; }

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
                DrawGlyph(GlyphSelected);
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
                DrawGlyph(GlyphSelected);
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

        private Point startPoint;
        public Point StartPoint
        {
            get => startPoint;
            set
            {
                startPoint = value;
                NotifyOfPropertyChange(() => StartPoint);
            }
        }

        private Point middlePoint;
        public Point MiddlePoint
        {
            get => middlePoint;
            set
            {
                middlePoint = value;
                NotifyOfPropertyChange(() => MiddlePoint);
            }
        }
        private Point endPoint;
        public Point EndPoint
        {
            get => endPoint;
            set
            {
                endPoint = value;
                NotifyOfPropertyChange(() => EndPoint);
            }
        }
        private Point head1Point;
        public Point Head1Point
        {
            get => head1Point;
            set
            {
                head1Point = value;
                NotifyOfPropertyChange(() => Head1Point);
            }
        }
        private Point head2Point;
        public Point Head2Point
        {
            get => head2Point;
            set
            {
                head2Point = value;
                NotifyOfPropertyChange(() => Head2Point);
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


        private Point center;
        public Point Center
        {
            get { return new Point(Layout.Width / 2d, Layout.Height / 2d);  }
            set
            {
                center = value;
                NotifyOfPropertyChange(() => Center);
            }
        }
        private double radiusx;
        public double RadiusX
        {
            get { return radiusx; }
            set
            {
                radiusx = value;
                NotifyOfPropertyChange(() => RadiusX);
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
        private void DrawGlyph(int moduleToDraw)
        {
            switch(moduleToDraw)
            {
                case (int)PushButtonGlyph.None:
                    return;
                case (int)PushButtonGlyph.Circle:
                    DrawCircle();
                    return;
                case (int)PushButtonGlyph.RightArrow:
                    DrawArrow(true);
                    return;
                case (int)PushButtonGlyph.LeftArrow:
                    DrawArrow(false);
                    return;
                case (int)PushButtonGlyph.UpCaret:
                    DrawCaret(true);
                    return;
                case (int)PushButtonGlyph.DownCaret:
                    DrawCaret(false);
                    return;
            }
        }

        private void DrawCircle()
        {
            Center = new Point(Layout.Width / 2d, Layout.Height / 2d);
            RadiusX = Math.Min(Layout.Width, Layout.Height) / 2d * GlyphScale;
        }
        private void DrawCaret(bool up = true)
        {
            double offsetX = Center.X * GlyphScale;
            double offsetY = offsetX / 2d;
            if (up)
            {
                StartPoint = new Point(Center.X - offsetX, Center.Y + offsetY);
                MiddlePoint = new Point(Center.X, Center.Y - offsetY);
                EndPoint = new Point(Center.X + offsetX, Center.Y + offsetY);
            }
            else
            {
                StartPoint = new Point(Center.X - offsetX, Center.Y - offsetY);
                MiddlePoint = new Point(Center.X, Center.Y + offsetY);
                EndPoint = new Point(Center.X + offsetX, Center.Y - offsetY);
            }
        }

        private void DrawArrow(bool Right)
        {
            double y = Layout.Height / 2d;
            double arrowLength = Layout.Width * GlyphScale;
            double padding = (Layout.Width - arrowLength) / 2d;
            double arrowLineLength = arrowLength * .6d;
            double headHeightOffset = GlyphThickness * 2d;

            //Point lineStart, lineEnd, head, head1, head2;

            if (Right)
            {
                StartPoint = new Point(padding, y);
                EndPoint = new Point(StartPoint.X + arrowLineLength, y);

                MiddlePoint = new Point(Layout.Width - padding, y);
                Head1Point = new Point(EndPoint.X, y - headHeightOffset);
                Head2Point = new Point(EndPoint.X, y + headHeightOffset);
            }
            else
            {
                StartPoint = new Point(Layout.Width - padding, y);
                EndPoint = new Point(StartPoint.X - arrowLineLength, y);

                MiddlePoint = new Point(padding, y);
                Head1Point = new Point(EndPoint.X, y + headHeightOffset);
                Head2Point = new Point(EndPoint.X, y - headHeightOffset);
            }

        }

        public void Handle(PushButtonAppearanceEvent message)
        {
            Image = message.Image[0];
            PushedImage = message.Image[1];
        }
    }
}
