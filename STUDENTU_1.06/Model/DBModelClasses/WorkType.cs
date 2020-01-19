using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model
{
    public class WorkType:ICloneable
    {
        public WorkType()
        {
            //this.TypeOfWork = "255";
            this.OrderLine = new ObservableCollection<OrderLine>();
        }
        public int WorkTypeId { get; set; }

        [Column("TypeOfWork", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string TypeOfWork { get; set; }

        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }

        public object Clone()
        {
            return new WorkType()
            {
                WorkTypeId = this.WorkTypeId,
                TypeOfWork = this.TypeOfWork,
                OrderLine = new ObservableCollection<OrderLine>(this.OrderLine)
            };
        }
    }
}
