using Caliburn.Micro;
using CockpitBuilder.Common.PropertyEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    public class UserViewModel : PropertyEditorModel
    {
        public UserViewModel(int tag)
        {
            if (tag == 0)
                Name = $"tititi{tag}";
            else
                Name = $"to                {tag}";
        }



        private string name;
        public string Name
        {
            get => name;
            set
            {
                NotifyOfPropertyChange(() => Name);
                name = value;
            }
        }
    }


}
