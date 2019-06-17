﻿using System.Windows;

namespace CockpitBuilder.Shells
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as AboutViewModel).GotoProjectPage();
        }
    }
}
