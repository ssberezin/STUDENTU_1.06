using STUDENTU_1._06.Model;
using STUDENTU_1._06.ViewModel.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Helpes
{
    public class TMPStaticClass
    {

       // public static OrderLine CurrentOrder = new OrderLine() { Dates=new Dates()};

        public static OrderLine CurrentOrder { get; set; }
        public static _Filters TMPFilters { get; set; }
        //public static Author CurrentAuthor { get; set; }
        //public static ObservableCollection<Direction> TMPOrdersDirection;
       // public static List<int> OrdersIDs;
    }
}
