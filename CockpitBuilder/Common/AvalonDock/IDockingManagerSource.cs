using Xceed.Wpf.AvalonDock;

namespace CockpitBuilder.Common.AvalonDock
{
    internal interface IDockingManagerSource
    {
        DockingManager DockingManager { get; }
    }
}