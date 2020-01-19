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
    public class AfterDoneDescription :ICloneable
    {
       
        public AfterDoneDescription()
        {
            this.PrintOreNot = false;
            this.Binding = false;
            this.OrderLine = new ObservableCollection<OrderLine>();
        }

        [ForeignKey("OrderLine")]
        public int AfterDoneDescriptionId { get; set; }
        
        public bool InputToBase { get; set; }
        public bool PrintOreNot { get; set; }
        //переплет обложкой
        public bool Binding { get; set; }

        [Column("BindingDiscription", TypeName = "ntext")]
        [MaxLength(500)]
        public string BindingDiscription { get; set; }


        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }

        public object Clone()
        {
            return new AfterDoneDescription()
            {
                //AfterDoneDescriptionId= obj.AfterDoneDescriptionId,
                //InputToBase= obj.InputToBase,
                //PrintOreNot= obj.PrintOreNot,
                //Binding= obj.Binding,
                //BindingDiscription= obj.BindingDiscription,
                //OrderLine = new ObservableCollection<OrderLine>(obj.OrderLine)
            };
        }
    }
}
