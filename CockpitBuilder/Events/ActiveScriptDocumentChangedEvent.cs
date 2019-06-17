using CockpitBuilder.Views.Main.DockPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockpitBuilder.Events
{
    public class ActiveScriptDocumentChangedEvent
    {
        public PanelViewModel Document { get; private set; }
        public ActiveScriptDocumentChangedEvent(PanelViewModel document)
        {
            Document = document;
        }
    }
}
