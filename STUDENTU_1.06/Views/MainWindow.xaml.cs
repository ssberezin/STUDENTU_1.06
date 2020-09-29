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
            //тут у нас окно авторизации делалось не в первую очередь, потому для того, чтоб оно
            //стартовало первым, было принято решение сделать его главным. Т.е. MainViewModel.cs у нас для 
            // FirstAfterMein.xaml, а контекстом MainWindow.xaml  имеем _Authorezation.cs

            // here we made the authorization window not in the first place, because in order for it
            // started first, it was decided to make it the main one. That is, we have MainViewModel.cs for
            // FirstAfterMein.xaml, and the MainWindow.xaml context has _Authorezation.cs

            InitializeComponent();            
            this.DataContext = new _Authorisation();
        }

     
    }
}
