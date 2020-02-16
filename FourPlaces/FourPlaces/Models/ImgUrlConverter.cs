using System;
using System.Globalization;
using Xamarin.Forms;

namespace FourPlaces.Models
{
    class ImgUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value != 0)
            {
                return "https://td-api.julienmialon.com/images/" + (int)value;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
