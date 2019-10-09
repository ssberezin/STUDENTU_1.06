using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STUDENTU_1._06.Model.HelpModelClasses.ShowWindows
{
    interface IShowWindowService
    {
        void ShowWindow(Window window);
        void CloseWindow(Window window);
    }
}
