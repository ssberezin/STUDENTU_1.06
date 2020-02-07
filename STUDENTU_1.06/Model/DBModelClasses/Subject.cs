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
    public class Subject:ICloneable
    {
        public Subject()
        {
            this.OrderLine = new ObservableCollection<OrderLine>();
            this.Authors = new ObservableCollection<Author>();
        }

        public int SubjectId { get; set; }

        [Column("SubName", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string SubName { get; set; }

        //public virtual Author Author { get; set; }
        public virtual ObservableCollection<Author> Authors { get; set; }

        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }

        public object Clone()
        {
            return new Subject()
            {
                SubjectId = this.SubjectId,
                SubName = this.SubName,
                Authors = new ObservableCollection<Author>(this.Authors),
                OrderLine = new ObservableCollection<OrderLine>(this.OrderLine)
            };
        }

        public bool CompareSubject(Subject obj1, Subject obj2)
        {
            if (obj1.SubjectId == obj2.SubjectId)
                return true;
            return false;
        }

      
    }
}
