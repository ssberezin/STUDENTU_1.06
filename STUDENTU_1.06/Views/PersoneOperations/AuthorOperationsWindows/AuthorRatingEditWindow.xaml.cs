
using System.Windows;


namespace STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows
{
    /// <summary>
    /// Логика взаимодействия для AuthorRatingEditWindow.xaml
    /// </summary>
    public partial class AuthorRatingEditWindow : Window
    {
        public AuthorRatingEditWindow(object DataContext)
        {
            InitializeComponent();
            this.DataContext = DataContext;
        }
    }
}
