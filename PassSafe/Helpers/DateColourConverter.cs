using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PassSafe.Helpers
{
    [ValueConversion(typeof(DateTime), typeof(SolidColorBrush))]
    class DateColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime lastUpdated = (DateTime)value;
            int daysSinceLastUpdate = (DateTime.Now - lastUpdated).Days;
            if (daysSinceLastUpdate <= 31)
                return new SolidColorBrush(Colors.Green);
            else if (daysSinceLastUpdate <= 62)
                return new SolidColorBrush(Colors.Orange);
            else
                return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
