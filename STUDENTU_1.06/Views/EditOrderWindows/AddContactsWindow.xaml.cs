
using System.Windows;


namespace STUDENTU_1._06.Views
{
    /// <summary>
    /// Логика взаимодействия для AddContactsWindow.xaml
    /// </summary>
    public partial class AddContactsWindow : Window
    {
        public AddContactsWindow(object DataContext)
        {
            InitializeComponent();
            this.DataContext = DataContext;
        }
        
    }
}
