using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class Direction : Helpes.ObservableObject ,ICloneable
    {
        public Direction()
        {          
            this.OrderLine = new ObservableCollection<OrderLine>();
            this.Author = new ObservableCollection<Author>();
            this._Checked = false;
        }
                
        public int DirectionId { get; set; }

        [Column("DirectionName", TypeName = "nvarchar")]
        [MaxLength(80)]
        public string DirectionName { get; set; }

        //для фильтрации
        //for filter
        private bool _checked;
        [NotMapped]
        public bool _Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    OnPropertyChanged(nameof(_Checked));
                }
            }
        }

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
                Author = new ObservableCollection<Author>(this.Author),
                _Checked= this._Checked
            };                      
        }
        public bool CompareDirection(Direction obj1, Direction obj2)
        {
            if (obj1.DirectionId == obj2.DirectionId)
                return true;
            return false;
        }


    }
}
