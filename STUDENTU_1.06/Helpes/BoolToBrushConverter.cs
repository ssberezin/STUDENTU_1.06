using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

//not used
namespace STUDENTU_1._06.Helpes
{
    
        public class BoolToBrushConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //назначаем цвет заливки строки-победителя в списке оценок заказа
            // set the fill color of the winning row in the list of order ratings
            if (value is bool)
            {
                
                if ((bool)value)
                {
                    return Brushes.LightBlue;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
