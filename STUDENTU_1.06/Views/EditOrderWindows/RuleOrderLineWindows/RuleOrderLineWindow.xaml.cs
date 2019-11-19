using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
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

namespace STUDENTU_1._06.Views.EditOrderWindows.RuleOrderLineWindows
{
    /// <summary>
    /// Логика взаимодействия для RuleOrderLineWindow.xaml
    /// </summary>
    public partial class RuleOrderLineWindow : Window
    {


        public RuleOrderLineWindow()
        {

            InitializeComponent();
            this.DataContext = new RuleOrderLine();
        }


    }
}
