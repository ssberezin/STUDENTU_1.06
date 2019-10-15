
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class Author
    {

      
        public Author()
        {
            this.Subject = new ObservableCollection<Subject>();
            this.Direction = new ObservableCollection<Direction>();
            this.Evaluation = new ObservableCollection<Evaluation>();
            this.OrderLine = new ObservableCollection<OrderLine>();
            
            // this.Sourse = "1000";
        }

        [Column("Sourse", TypeName = "ntext")]
        [MaxLength(2000)]
        public string Sourse { get; set; }

        

        public int AuthorId { get; set; }        

        public virtual Persone Persone { get; set; }
        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }
        public virtual ObservableCollection<Subject> Subject { get; set; }
        public virtual ObservableCollection<Direction> Direction { get; set; }
        public virtual ObservableCollection<Evaluation> Evaluation { get; set; }


        public virtual Status Status { get; set; }
    }
}
