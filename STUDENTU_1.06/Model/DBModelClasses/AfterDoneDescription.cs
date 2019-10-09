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
    public class AfterDoneDescription
    {
       
        public AfterDoneDescription()
        {
            this.PrintOreNot = false;
            this.Binding = false;
            this.OrderLine = new ObservableCollection<OrderLine>();
        }


        public int AfterDoneDescriptionId { get; set; }
        
        public bool InputToBase { get; set; }
        public bool PrintOreNot { get; set; }
        public bool Binding { get; set; }

        [Column("BindingDiscription", TypeName = "ntext")]
        [MaxLength(2000)]
        public string BindingDiscription { get; set; }


        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }
        


    }
}
