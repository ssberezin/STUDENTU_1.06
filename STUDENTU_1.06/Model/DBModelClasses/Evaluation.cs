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
    public class Evaluation
    {
        public Evaluation()
        {
            this.Dates = new ObservableCollection<Dates>();
            this.Moneys = new ObservableCollection<Money>();
            this.Authors = new ObservableCollection<Author>();
            this.Winner = false;
        }

        public int EvaluationId { get; set; }
        public bool Winner { get; set; }

       

        [Column("Description", TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string Description { get; set; }
        

        public virtual ObservableCollection<Dates> Dates { get; set; }
        public virtual ObservableCollection<Money> Moneys { get; set; }

        public virtual ObservableCollection<Author> Authors { get; set; }

        

    }
}
