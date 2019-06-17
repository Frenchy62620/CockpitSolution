using CockpitBuilder.Common.PropertyEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CockpitBuilder.Events
{
    public class DisplayPropertiesView1Event
    {
        public PropertyEditorModel[] Views;
        public DisplayPropertiesView1Event(PropertyEditorModel[] views)
        {
            Views = views;
        }
    }
}
