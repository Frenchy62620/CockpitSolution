using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Views.Main.ToolBarTray
{
    public class MainToolBarTrayViewModel : Screen
    {
        private readonly IEventAggregator eventAggregator;
        private bool _canAlignBottom;

        public MainToolBarTrayViewModel(IEventAggregator eventAggregator)
        {

        }

        public void AlignBottom()
        {

        }

        bool canAlignBottom
        {
            get => _canAlignBottom;
            set
            {
                _canAlignBottom = value;
                NotifyOfPropertyChange(() => canAlignBottom);
            }
        }

    }
}
