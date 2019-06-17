namespace CockpitBuilder.Events
{
    public class SelectedEvent
    {
        public string Tag;

        public SelectedEvent(string tag = null)
        {
            Tag = tag;
        }
    }
}
