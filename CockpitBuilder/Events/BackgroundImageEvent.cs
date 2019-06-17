namespace CockpitBuilder.Events
{
    public class BackgroundImageEvent
    {
        public string FullImageName;
        public int ImageHeight;
        public int ImageWidth;
        public BackgroundImageEvent(string image)
        {
            FullImageName = image;
        }
    }
}
