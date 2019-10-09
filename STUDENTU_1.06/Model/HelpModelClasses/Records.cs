using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model
{
   public  class Records
    {
        public int RecordId { get; set; }
        public int OrderNumber { get; set; }
        public int OrderCount { get; set; }
        public DateTime DateOfReception { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime DateDone { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Prepayment { get; set; }
        public Contacts Contacts { get; set; }
        public string Status { get; set; }
        public string TypeOfWork { get; set; }
        public string AuthorNickName { get; set; }
        public string ClientName { get; set; }
        public string SubName { get; set; }
    }
}
