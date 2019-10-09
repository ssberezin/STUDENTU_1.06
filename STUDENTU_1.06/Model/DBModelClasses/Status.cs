
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTU_1._06.Model
{
   public class Status
    {
        public Status()
        {
            //this.StatusName = "255";
            //this.Description = "1000";
            this.OrderLine = new List<OrderLine>();
            this.StatusName = "принимается";
        }
        
        public int StatusId { get; set; }

        [Column("Description", TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Description { get; set; }
        [Column("StatusName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string StatusName { get; set; }

        //public virtual OrderLine OrderLine { get; set; }

        public virtual List<OrderLine> OrderLine { get; set; }
        //[System.ComponentModel.DataAnnotations.Required]
        //public string  MoneyId { get; set; }
    }
}
