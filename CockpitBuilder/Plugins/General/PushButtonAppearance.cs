using Caliburn.Micro;
using CockpitBuilder.Common.CustomControls;

namespace CockpitBuilder.Plugins.General
{
    public class PushButtonAppearance: PropertyChangedBase
    {
        public TextFormat TextFormat { get; }
        public string NameUC;
        public PushButtonAppearance(string nameUC, string[] images, int startimageposition, TextFormat textformat)
        {
            NameUC = nameUC;
            Image = images[0];
            PushedImage = images[1];
            IndexImage = startimageposition;

            TextFormat = textformat;

            GlyphScale = 0.8d;
        }

        private string image;
        public string Image
        {
            get => image;
            set
            {
                if (image != value)
                {
                    image = value;
                    NotifyOfPropertyChange(() => Image);
                }
            }
        }
        private string pushedimage;
        public string PushedImage
        {
            get => pushedimage;
            set
            {
                if (pushedimage != value)
                {
                    pushedimage = value;
                    NotifyOfPropertyChange(() => PushedImage);
                }
            }
        }
        private int indexImage;
        public int IndexImage
        {
            get { return indexImage; }
            set
            {
                if (indexImage != value)
                {
                    indexImage = value;
                    NotifyOfPropertyChange(() => IndexImage);
                }
            }
        }


        private double glyphscale;
        public double GlyphScale
        {
            get => glyphscale;
            set
            {
                if (glyphscale != value)
                {
                    glyphscale = value;
                    NotifyOfPropertyChange(() => GlyphScale);
                }
                //DrawGlyph(GlyphSelected);
            }
        }

        //private Point center;
        //public Point Center
        //{
        //    get { return new Point(LayoutWidth / 2d, LayoutHeight / 2d); }
        //    set
        //    {
        //        center = value;
        //        NotifyOfPropertyChange(() => Center);
        //    }
        //}
        //private double radiusx;
        //public double RadiusX
        //{
        //    get { return radiusx; }
        //    set
        //    {
        //        radiusx = value;
        //        NotifyOfPropertyChange(() => RadiusX);
        //    }
        //}

    }
}
