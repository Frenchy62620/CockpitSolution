using System;
using System.Windows.Media.Imaging;

namespace CockpitBuilder.Common.Resources
{
    public static class ResourceHelper
    {
        public static BitmapImage Load(string resource)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("pack://application:,,/Resources/" + resource);
            bi.EndInit();

            return bi;
        }
    }
}
