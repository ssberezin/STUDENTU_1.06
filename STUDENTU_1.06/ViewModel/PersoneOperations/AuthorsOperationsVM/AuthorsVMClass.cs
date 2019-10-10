using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STUDENTU_1._06.ViewModel.PersoneOperations.AuthorsOperationsVM
{
   public partial class AuthorsVMClass : Helpes.ObservableObject
    {

        IDialogService dialogService;
        IShowWindowService showWindow;

        public AuthorsVMClass(Window editWindow, DefaultShowWindowService showWindow,
           IDialogService dialogService)
        {

            this.showWindow = showWindow;
            this.dialogService = dialogService;
            editWindow.Loaded += EditWindow_Loaded;

        }


        private void EditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

   }
}
