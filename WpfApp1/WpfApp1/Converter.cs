using System;
using System.Collections.Generic;
using System.Text;
using Laba1;
using System.Windows.Data;

namespace WpfApp1
{
        [ValueConversion(typeof(Grid), typeof(string))]
        public class GridStartConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                Grid grid = (Grid)value;
                 return grid.Start;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return value;
            }
        }
    public class GridEndConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Grid grid = (Grid)value;

            return grid.GetTime(grid.Number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
