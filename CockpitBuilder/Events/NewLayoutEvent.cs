
namespace CockpitBuilder.Events
{
    public class NewLayoutEvent
    {
        public double NewLayoutWidth, NewLayoutHeight, NewLayoutAngle;
        public string ID;
        public NewLayoutEvent(double newLayoutWidth, double newLayoutHeight, double newLayoutAngle, string id)
        {
            NewLayoutWidth = newLayoutWidth;
            NewLayoutHeight = newLayoutHeight;
            NewLayoutAngle = newLayoutAngle;
            ID = id;
        }
    }
}
