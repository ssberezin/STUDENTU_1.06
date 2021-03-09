using STUDENTU_1._06.ViewModel.PersoneOperations.PersoneOperations;
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

namespace STUDENTU_1._06.Views.PersoneOperations.UserOperations
{
    /// <summary>
    /// Логика взаимодействия для UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : Window
    {
        //for fthe first registration
        public UserRegistration()
        {
            InitializeComponent();
            this.DataContext = new UserOps(this);
        }

        //for registration any new user by anothee user
        public UserRegistration(int UserId)
        {
            InitializeComponent();
            this.DataContext = new UserOps(UserId);
        }

        //for edit user data
        public UserRegistration(int UserId, int tmp)
        {
            InitializeComponent();
            this.DataContext = new UserOps(UserId, tmp);
        }
    }
}
