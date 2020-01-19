using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class Direction :ICloneable
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

        public object Clone()
        {
            return new Direction()
            {
                DirectionId = this.DirectionId,
                DirectionName = this.DirectionName,
                OrderLine = new ObservableCollection<OrderLine>(this.OrderLine),
                Author = new ObservableCollection<Author>(this.Author)
            };
           
            
        }
    }
}
