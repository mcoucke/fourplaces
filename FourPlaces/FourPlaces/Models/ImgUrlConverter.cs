using System;
using System.Globalization;
using Xamarin.Forms;

namespace FourPlaces.Models
{
    class ImgUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "https://td-api.julienmialon.com/images/" + (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
