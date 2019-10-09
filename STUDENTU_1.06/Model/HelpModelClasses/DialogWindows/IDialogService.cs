using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model.HelpModelClasses.DialogWindows
{
   public interface IDialogService         
    {
        bool YesNoDialog(string message);
        void ShowMessage(string message);
    }
}
