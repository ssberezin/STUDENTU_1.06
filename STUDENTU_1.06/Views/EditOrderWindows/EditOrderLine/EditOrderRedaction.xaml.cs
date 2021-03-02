using STUDENTU_1._06.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace STUDENTU_1._06.Views.EditOrderWindows.EditOrderLine
{
    /// <summary>
    /// Логика взаимодействия для EditOrderRedaction.xaml
    /// </summary>
    public partial class EditOrderRedaction : Window
    {
        public EditOrderRedaction(int UserId)
        {
            InitializeComponent();
            this.DataContext = new ForEditOrder(UserId);
        }
        public EditOrderRedaction(int OrderLineId, int UserId)
        {
            InitializeComponent();
            this.DataContext = new ForEditOrder(OrderLineId, UserId);
        }
    }
}
