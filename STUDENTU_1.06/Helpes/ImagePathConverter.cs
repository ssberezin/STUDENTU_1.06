using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace STUDENTU_1._06.Helpes
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            Uri uri = new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(uri);
            return ib;
            //BitmapImage _i = (BitmapImage)value;
            //ImageBrush _b = new ImageBrush();
            //if (_i != null)
            //{
            //    _b.ImageSource = _i;
            //}
            //return _b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
