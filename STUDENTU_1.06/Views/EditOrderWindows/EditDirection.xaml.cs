using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.ViewModel;

using System.Windows;


namespace STUDENTU_1._06.Views
{

    public partial class EditDirection : Window
    {
    
        public EditDirection(object DataContext)
        {
            InitializeComponent();
            this.DataContext = DataContext;
        }
    }
}
