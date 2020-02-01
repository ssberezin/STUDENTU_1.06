
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTU_1._06.Model
{
   public class Status:ICloneable
    {
        public Status()
        {
        
            this.OrderLine = new ObservableCollection<OrderLine>();            
            
        }
        [ForeignKey("OrderLine")]
        public int StatusId { get; set; }

        [Column("Description", TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Description { get; set; }
        [Column("StatusName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string StatusName { get; set; }

       public virtual ObservableCollection<OrderLine> OrderLine { get; set; }

        public object Clone()
        {
           return new Status()
           {
               StatusId = this.StatusId,
               Description = this.Description,
               StatusName = this.StatusName,
               OrderLine = new ObservableCollection<OrderLine>(this.OrderLine)
           };
        }
    }
}
