using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PassSafe.Helpers
{
    class OpenWhenFocusedConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo l)
        {
            return (bool)v;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo l) => null;
    }
}