using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace STUDENTU_1._06.Helpes
{

    public class StringToBrushConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //назначаем цвет заливки строк в случае если имеем заказ разбитый на несколько подзаказов
            // assign line fill color if we have an order divided into several suborders
            int i;
            if (int.TryParse(value.ToString(), out  i))
            {
                if (i > 1) return Brushes.Orchid;
                else
                    return DependencyProperty.UnsetValue;
            }
            else
                // назначаем цвет шрифта строк
            switch (value.ToString())
            {
                case "-принят-":
                    return Brushes.Red;
                case "-готов-":
                    return Brushes.Blue;
                case "-отдан заказчику-":
                    return Brushes.Green;
                case "-выполняется-":
                    return Brushes.Brown;
                case "-на оценке-":
                    return Brushes.Gray;
                case "-принят на доработку-":
                      return Brushes.OrangeRed;
                case "-дорабатывается-":
                      return Brushes.SaddleBrown;
                case "-ждем новостей от заказчика-":
                        return Brushes.DarkGray;

                    //case "отказ":
                    //        return Brushes.Orange;
                    default:
                    return DependencyProperty.UnsetValue;
            }
        }
      
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
