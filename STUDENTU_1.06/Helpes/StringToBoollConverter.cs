using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace STUDENTU_1._06.Helpes
{
    class StringToBoollConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        //for enable button "Распределить заказ", if orderline status is not "принимается"

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return value.ToString() == "---" ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
