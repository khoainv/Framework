
using System;
using Windows.UI.Xaml.Data;
namespace SSD.Mobile.WP
{
    public class MoneyFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value!=null)
                return System.Convert.ToDecimal(value).ToString("N0");
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}