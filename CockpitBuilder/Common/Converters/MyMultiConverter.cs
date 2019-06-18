using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CockpitBuilder.Common.Converters
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double LeftOrTop, ZoomFactor;

            LeftOrTop = (double)values[0];
            ZoomFactor = (double)values[1];
            return LeftOrTop * ZoomFactor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyMultiConverterMargin : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double left = 0, top = 0, right = 0, bottom = 0, width = 0, height = 0;

            left = (double)values[0];
            top = (double)values[1];
            right = (double)values[2];
            bottom = (double)values[3];
            width = (double)values[4];
            height = (double)values[5];

            Thickness margin = new Thickness
            {
                Left = (int)(left * width),
                Top = (int)(top * height),
                Right = (int)(right * width),
                Bottom = (int)(bottom * height)
            };

            return margin;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CircleRadiusOrCenterCalculus : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Center = new Point(LayoutWidth / 2d, LayoutHeight / 2d)
            // RadiusX = Math.Min(LayoutWidth, LayoutHeight) / 2d * GlyphScale
            bool radiusCalculus = values.Length == 3;
            double width = (double)values[0];
            double height = (double)values[1];

            if (radiusCalculus)
                return Math.Min(width, height) / 2d * (double)values[2];

            return new Point(width / 2d, height / 2d);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class CircleScaling : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double width = 0, height = 0;
            double.TryParse(values[0].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out width);
            double.TryParse(values[1].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out height);
            //string p = parameter.ToString();

            //int[] param = p.Split(' ').Select(x => System.Convert.ToInt32(x, CultureInfo.InvariantCulture)).ToArray();

            Thickness margin = new Thickness
            {
                Left = width,
                Top = height,
                Right = 0,
                Bottom = 0
            };

            return margin;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
