
using System;
using Windows.UI.Xaml.Data;
namespace SSD.Mobile.WP
{
    public class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value!=null && System.Convert.ToDateTime(value) != new DateTime(1,1,1))
                return System.Convert.ToDateTime(value).ToString("H:mm dd-MM-yyyy");
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}