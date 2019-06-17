using CockpitBuilder.Views.Main.DockPanel.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockpitBuilder.Events
{
    public class ScriptDocumentAddedEvent
    {
        public MonitorViewModel Document { get; private set; }

        public bool toDelete { get; private set; }
        public ScriptDocumentAddedEvent(MonitorViewModel document, bool todelete = false)
        {
            Document = document;
            toDelete = todelete;
        }
    }
}
