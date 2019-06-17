using CockpitBuilder.Views.Main.DockPanel.Models;

namespace CockpitBuilder.Events
{
    public class DragSelectedItemEvent
    {
        public string FullImageName;
        public int ImageHeight;
        public int ImageWidth;

        public DragSelectedItemEvent(ToolBoxItem tbi)
        {
            FullImageName = tbi.FullImageName;
            ImageHeight = tbi.ImageHeight;
            ImageWidth = tbi.ImageWidth;
        }
    }
}
