using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    /// <summary>
    /// Logique d'interaction pour ToolBoxView.xaml
    /// </summary>
    public partial class ToolBoxView : UserControl
    {
        public ToolBoxView()
        {
            InitializeComponent();
            foreach (var visual in FindVisualChildren<Visual>(this))
            {
                if (visual.GetType().ToString() == "rectangleBarChart")
                {
                    /*   Your code here  */
                }
            }
        }
        private double area( List<double> lats, List<double> lons)
        {
            double sum = 0;
            double prevcolat = 0;
            double prevaz = 0;
            double colat0 = 0;
            double az0 = 0;
            for (int i = 0; i < lats.Count; i++)
            {
                double colat = 2 * Math.Atan2(Math.Sqrt(Math.Pow(Math.Sin(lats[i] * Math.PI / 180 / 2), 2) + Math.Cos(lats[i] * Math.PI / 180) * Math.Pow(Math.Sin(lons[i] * Math.PI / 180 / 2), 2)), Math.Sqrt(1 - Math.Pow(Math.Sin(lats[i] * Math.PI / 180 / 2), 2) - Math.Cos(lats[i] * Math.PI / 180) * Math.Pow(Math.Sin(lons[i] * Math.PI / 180 / 2), 2)));
                double az = 0;
                if (lats[i] >= 90)
                {
                    az = 0;
                }
                else if (lats[i] <= -90)
                {
                    az = Math.PI;
                }
                else
                {
                    az = Math.Atan2(Math.Cos(lats[i] * Math.PI / 180) * Math.Sin(lons[i] * Math.PI / 180), Math.Sin(lats[i] * Math.PI / 180)) % (2 * Math.PI);
                }
                if (i == 0)
                {
                    colat0 = colat;
                    az0 = az;
                }
                if (i > 0 && i < lats.Count)
                {
                    sum = sum + (1 - Math.Cos(prevcolat + (colat - prevcolat) / 2)) * Math.PI * ((Math.Abs(az - prevaz) / Math.PI) - 2 * Math.Ceiling(((Math.Abs(az - prevaz) / Math.PI) - 1) / 2)) * Math.Sign(az - prevaz);
                }
                prevcolat = colat;
                prevaz = az;
            }
            sum = sum + (1 - Math.Cos(prevcolat + (colat0 - prevcolat) / 2)) * (az0 - prevaz);
            return 5.10072E14 * Math.Min(Math.Abs(sum) / 4 / Math.PI, 1 - Math.Abs(sum) / 4 / Math.PI);
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)

                {

                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)

                    {

                        yield return (T) child;

                    }



                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }

        }

        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }
    }
}
