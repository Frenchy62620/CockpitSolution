using System.Windows.Controls;

namespace CockpitBuilder.Plugins.General
{
    /// <summary>
    /// Logique d'interaction pour SwitchOn_Off_On_View.xaml
    /// </summary>
    public partial class PushButton_View : UserControl
    {
        public PushButton_View()
        {
            InitializeComponent();
        }
        ~PushButton_View()
        {
            System.Diagnostics.Debug.WriteLine("sortie pushView");
        }
    }
}
