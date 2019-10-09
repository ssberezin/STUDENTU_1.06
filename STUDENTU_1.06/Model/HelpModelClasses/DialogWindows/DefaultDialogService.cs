using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STUDENTU_1._06.Model.HelpModelClasses.DialogWindows
{
   class DefaultDialogService : IDialogService
    {
        
        bool IDialogService.YesNoDialog(string message)
        {
            MessageBoxResult res = MessageBox.Show(message, "", MessageBoxButton.YesNo);
            return res == MessageBoxResult.Yes ? true : false;
        }

        void IDialogService.ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
