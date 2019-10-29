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
    public class Subject
    {
        public Subject()
        {
            this.OrderLine = new List<OrderLine>();
            this.Authors = new ObservableCollection<Author>();
        }

        public int SubjectId { get; set; }

        [Column("SubName", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string SubName { get; set; }

        //public virtual Author Author { get; set; }
        public virtual ObservableCollection<Author> Authors { get; set; }

        public virtual List<OrderLine> OrderLine { get; set; }
        

    }
}
