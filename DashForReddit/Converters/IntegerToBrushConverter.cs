using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DashForReddit.Converters
{
    class IntegerToBrushConverter : IValueConverter
    {
        private static List<string> colors = new List<string>() { "Black", "Blue", "Green", "Red", "Purple", "Gold", "Brown", "Cyan", "Magenta" };
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return colors[(int)value % colors.Count];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
