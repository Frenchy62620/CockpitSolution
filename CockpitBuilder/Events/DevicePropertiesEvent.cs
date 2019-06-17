namespace CockpitBuilder.Events
{
    public class DevicePropertiesEvent
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public string Tag; 
        public DevicePropertiesEvent(string tag, int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Tag = tag;
        }
    }
}
