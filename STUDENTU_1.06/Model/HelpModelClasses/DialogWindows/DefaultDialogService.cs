using Microsoft.Win32;
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

        private static string lastPath = null;


        public string OpenFileDialog(string path)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = lastPath == null ? "c:\\" : lastPath;
            ofd.Filter = "Image files|*.jpg;*.jpeg;*.gif;*.bmp;*.png|All Files|*.*";
            ofd.Title = "Open image file";
            ofd.RestoreDirectory = true;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
                lastPath = ofd.FileName;
            return lastPath;
        }
    }
}
