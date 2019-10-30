using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class Direction 
    {
        public Direction()
        {          
            this.OrderLine = new ObservableCollection<OrderLine>();
            this.Author = new ObservableCollection<Author>();
        }
                
        public int DirectionId { get; set; }

        [Column("DirectionName", TypeName = "nvarchar")]
        [MaxLength(80)]
        public string DirectionName { get; set; }

        public virtual ObservableCollection<Author> Author { get; set; }
        //public virtual Author Author { get; set; }

        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }
        


    }
}
