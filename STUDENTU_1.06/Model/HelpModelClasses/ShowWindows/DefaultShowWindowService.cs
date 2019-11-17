using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STUDENTU_1._06.Model.HelpModelClasses.ShowWindows
{
   public  class DefaultShowWindowService : IShowWindowService
    {
        public void ShowWindow(Window window)
        {
            window.Show();
        }
        public void CloseWindow(Window window)
        {
            window.Close();
        }

        public void ShowDialog(Window window)
        {
            window.ShowDialog();
        }
    }
}
