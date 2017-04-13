using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PassSafe.Helpers
{
    class VisibleWhenErrorsConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo l)
        {
            bool actualV = (bool)v;
            if (actualV)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo l) => null;
    }
}
