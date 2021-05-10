using System;
using Laba1;
using System.Windows.Data;

namespace WpfApp1
{
    //[ValueConversion(typeof(Grid), typeof(string))]
    [ValueConversion(typeof(V1DataOnGrid), typeof(string))]
    public class GridStartConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
            //Grid grid = (Grid)value;
            //return grid.Start;
            if (value as V1DataOnGrid == null)
                return value?.ToString();
            V1DataOnGrid data = (V1DataOnGrid)value;
            if (data.Grid.Number <= 0)
            {
                return data.Grid.Start + " / V1DataOnGrid is empty";
            }
            return data.Grid.Start + " " + data.Values[0];
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
            //Grid grid = (Grid)value;
            //return grid.GetTime(grid.Number);
            if (value as V1DataOnGrid == null)
                return value?.ToString();
            V1DataOnGrid data = (V1DataOnGrid)value;
            if (data.Grid.Number <= 0)
            {
                return data.Grid.GetTime(data.Grid.Number) + " / V1DataOnGrid is empty";
            }
            return data.Grid.GetTime(data.Grid.Number) + " " + data.Values[data.Grid.Number-1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
