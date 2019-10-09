using System;
using System.Globalization;
using System.Windows.Data;
namespace STUDENTU_1._06.Helpes
{
    public class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return ((DateTime)value).ToString("dd.MM.yyyy hh:mm");
            
            if (((DateTime)value).ToString("dd.MM.yyyy") == "01.01.1900")
            {
                return "";
            }
            return ((DateTime)value).ToString("dd.MM.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
