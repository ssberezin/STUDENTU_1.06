using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.ViewModel;
using System.Windows;


namespace STUDENTU_1._06
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //_Authorisation()
            //this.DataContext = new MainViewModel(this, new DefaultShowWindowService());
            this.DataContext = new _Authorisation();
        }

     
    }
}
