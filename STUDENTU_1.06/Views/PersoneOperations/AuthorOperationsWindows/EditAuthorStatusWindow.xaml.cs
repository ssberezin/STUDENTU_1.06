
using System.Windows;

namespace STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows
{
    /// <summary>
    /// Логика взаимодействия для EditAuthorStatusWindow.xaml
    /// </summary>
    public partial class EditAuthorStatusWindow : Window
    {
        public EditAuthorStatusWindow(object DataContext)
        {
            InitializeComponent();
            this.DataContext = DataContext;
        }
    }
}
