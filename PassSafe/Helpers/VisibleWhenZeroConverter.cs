using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PassSafe.Helpers
{
    class VisibleWhenZeroConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo l)
        {
            var returnVal = Equals(0d, System.Convert.ToDouble(v)) ? Visibility.Visible : Visibility.Collapsed;
            return returnVal;
        }
            

        public object ConvertBack(object v, Type t, object p, CultureInfo l) => null;
    }
}
