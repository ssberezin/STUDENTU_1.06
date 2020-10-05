using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.ViewModel;
using System.Windows;

namespace STUDENTU_1._06.ViewModel
{
    /// <summary>
    /// Логика взаимодействия для FirstAfterMein.xaml
    /// </summary>
    public partial class FirstAfterMein : Window
    {
        public FirstAfterMein(int userId)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(this, new DefaultShowWindowService(), userId);
        }
    }
}
