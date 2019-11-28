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
    public class Client
    {        
        public Client()
        {
            this.OrderLine = new ObservableCollection<OrderLine>();
        }
        //[ForeignKey("OrderLine")]
        public int ClientId { get; set; }

        [Column("UniversityName", TypeName = "ntext")]
        [MaxLength(2000)]
        public string UniversityName { get; set; }

        public int Course { get; set; }

        [Column("GroupName", TypeName = "ntext")]
        [MaxLength(255)]
        public string GroupName { get; set; }
        

        public virtual Persone Persone { get; set; }

        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }
    }
}
