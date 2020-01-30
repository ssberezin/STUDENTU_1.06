using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model.HelpModelClasses.ShowWindows
{
    public interface IRequireViewIdentification
    {
        Guid ViewID { get; }
    }
}
