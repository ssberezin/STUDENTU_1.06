
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.ViewModel;
using System.Windows;

namespace STUDENTU_1._06.Views
{
    /// <summary>
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {
        public EditOrder()
        {
            InitializeComponent();
            //this.DataContext = new ForEditOrder(this, new DefaultShowWindowService(),
            //    new DefaultDialogService());
            this.DataContext = new ForEditOrder();

        }

      
    }
}
