using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model
{
    public class WorkType
    {
        public WorkType()
        {
            //this.TypeOfWork = "255";
            this.OrderLine = new List<OrderLine>();
        }
        public int WorkTypeId { get; set; }

        [Column("TypeOfWork", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string TypeOfWork { get; set; }

        public virtual List<OrderLine> OrderLine { get; set; }

    }
}
