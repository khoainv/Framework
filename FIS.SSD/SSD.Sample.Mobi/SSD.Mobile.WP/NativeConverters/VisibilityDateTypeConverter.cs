
using SSD.Mobile.Share;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
namespace SSD.Mobile.WP
{
    public class VisibilityDateTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = (DateType)value;
            return val == AppPara.CurrentDateType ? Visibility.Collapsed:Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}