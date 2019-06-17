using CockpitBuilder.Views.Main.DockPanel.Panels;

namespace CockpitBuilder.Events
{
    public class MonitorViewStartedEvent
    {
        public MonitorViewModel Monitorviewmodel;

        public MonitorViewStartedEvent(MonitorViewModel monitorviewmodel)
        {
            Monitorviewmodel = monitorviewmodel;
        }
    }
}
