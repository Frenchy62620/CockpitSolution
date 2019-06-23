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
        public bool Clear;
        public DisplayPropertiesView1Event(PropertyEditorModel[] views, bool clear = false)
        {
            Clear = clear;
            Views = views;
        }
    }
}
