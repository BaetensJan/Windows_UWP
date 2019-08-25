using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Windows_UWP.Utils
{
    public class DateTimeOffsetToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateTime = (DateTimeOffset)value;
            return dateTime.ToString("d");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var stringDate = (string)value;
            return DateTimeOffset.Parse(stringDate);
        }
    }
}
