using CockpitBuilder.Views.Main.DockPanel.Panels;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace CockpitBuilder.Common.AvalonDock
{
    public class AutobinderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PreviewTemplate { get; set; }
        public DataTemplate ToolBoxTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var itemAsLayoutContent = item as LayoutContent;
            return PreviewTemplate;
            //if (item is PreviewTabViewModel || item is ProfileTabViewModel || item is ToolBoxViewModel)
            //    return PreviewTemplate;

            //if (item is ToolBoxViewModel)
            //    return ToolBoxTemplate;

            return base.SelectTemplate(item, container);
        }
    }

    public class AutobinderTemplate : DataTemplate
    {
        
    }
}
