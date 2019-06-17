using CockpitBuilder.Common.PropertyEditors;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class PropertyLayoutEvent
    {
        public double Left;
        public double Top;
        public double Width;
        public double Height;
        public SwitchRotation Rotation;
        public PropertyLayoutEvent(double left, double top, double width, double height, int rotation)
        {
            switch(rotation)
            {
                case 0:
                    Rotation = SwitchRotation.None;
                    break;
                case 90:
                    Rotation = SwitchRotation.CW;
                    break;
                case -90:
                case 270:
                    Rotation = SwitchRotation.CCW;
                    break;
                default:
                    Rotation = SwitchRotation.INV;
                    break;
            }
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
    }
}
