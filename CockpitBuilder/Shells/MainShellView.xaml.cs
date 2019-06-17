using CockpitBuilder.Common.AvalonDock;
using System.Reflection;
using System.Windows;
using Xceed.Wpf.AvalonDock;

namespace CockpitBuilder.Shells
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainShellView : IDockingManagerSource
    {
        private static readonly FieldInfo MenuDropAlignmentField;

        static MainShellView()
        {
            MenuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment",
                BindingFlags.NonPublic | BindingFlags.Static);
            System.Diagnostics.Debug.Assert(MenuDropAlignmentField != null);

            EnsureStandardPopupAlignment();
            SystemParameters.StaticPropertyChanged += (s, e) => EnsureStandardPopupAlignment();
        }

        private static void EnsureStandardPopupAlignment()
        {
            if (SystemParameters.MenuDropAlignment && MenuDropAlignmentField != null)
            {
                MenuDropAlignmentField.SetValue(null, false);
            }
        }
        public MainShellView()
        {
            InitializeComponent();
        }

        public DockingManager DockingManager
        {
            get { return Manager; }
        }
    }
}
