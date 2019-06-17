namespace CockpitBuilder.Events
{
    public class VisibilityPanelEvent
    {
        public string TagPanel;
        public bool Visible;

        public VisibilityPanelEvent(string tagpanel, bool visible)
        {
            TagPanel = tagpanel;
            Visible = visible; 
        }
    }
}
