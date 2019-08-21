using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Windows_UWP.Utils
{
    public class BooleanToSubscribeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var booleanValue = (bool)value;
            if (booleanValue)
                return "Unsubscribe";
            return "Subscribe";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value.Equals("Subscribe") ? false : true;
        }
    }
}
