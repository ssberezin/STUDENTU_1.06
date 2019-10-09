using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model
{
    public class PersoneDescription
    {
        public PersoneDescription()
        {
            this.BlackList = false;
            this.Persone = new List<Persone>();
        }



        public int PersoneDescriptionId { get; set; }

        [Column("Description", TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Column("FeedBack", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string FeedBack { get; set; }
        public bool Status { get; set; }
        [Column("ReasonFortermCoop", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string ReasonFortermCoop { get; set; }
        [Column("Source", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Source { get; set; }
        public bool BlackList { get; set; }

        public virtual List<Persone> Persone { get; set; }        

    }

}
