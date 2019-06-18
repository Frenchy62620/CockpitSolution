using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    public class MyAdorner : Adorner
    {
        public MyAdorner(UIElement targetElement) : base(targetElement) { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            drawingContext.DrawRectangle(null, new Pen(Brushes.Green, 4), adornedElementRect);
        }
    }
}
