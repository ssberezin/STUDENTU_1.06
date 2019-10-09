using System;

using System.Globalization;

using System.Windows.Data;

namespace STUDENTU_1._06.Helpes
{
    //this class need for bindig listbox and buttons for their enabled/diasbled
    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
