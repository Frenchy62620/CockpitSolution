using Caliburn.Micro;
using CockpitBuilder.Common.PropertyEditors;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.ObjectModel;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    public class BindingsViewModel : PanelViewModel
    {
        public BindingsViewModel(IResolutionRoot resolutionRoot)
        {
            Title = "Bindings";

        }
    }
}
