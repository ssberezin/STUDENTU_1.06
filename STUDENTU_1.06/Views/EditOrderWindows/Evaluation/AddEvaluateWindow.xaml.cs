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

namespace STUDENTU_1._06.Views.EditOrderWindows
{
    /// <summary>
    /// Логика взаимодействия для AddEvaluateWindow.xaml
    /// </summary>
    public partial class AddEvaluateWindow : Window
    {
        public AddEvaluateWindow(object DataContext)
        {
            InitializeComponent();
            this.DataContext = DataContext;
        }

     
    }
}
