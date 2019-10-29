using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model
{
    public class Contacts
    {
        public Contacts()
        {
            this.Phone1 = "+380";
            this.Phone2 = "---";
            this.Phone3 = "---";
            this.Email1 = "---";
            this.Email2 = "---";           
            this.Adress = "---";
            this.Skype = "---";
            this.VK = "---";
            this.FaceBook = "---";
            this.Persone = new List<Persone>();

        }
        [Column("Phone1", TypeName = "ntext")]
        [MaxLength(13)]
        public string Phone1 { get; set; }
        [Column("Phone2", TypeName = "ntext")]
        [MaxLength(13)]
        public string Phone2 { get; set; }
        [Column("Phone3", TypeName = "ntext")]
        [MaxLength(13)]
        public string Phone3 { get; set; }

        [Column("Email1", TypeName = "ntext")]
        [MaxLength(100)]
        public string Email1 { get; set; }
        [Column("Email2", TypeName = "ntext")]
        [MaxLength(100)]
        public string Email2 { get; set; }

        [Column("Adress", TypeName = "ntext")]
        [MaxLength(255)]
        public string Adress { get; set; }
        [Column("Skype", TypeName = "ntext")]
        [MaxLength(100)]
        public string Skype { get; set; }
        [Column("VK", TypeName = "ntext")]
        [MaxLength(100)]
        public string VK { get; set; }
        [Column("FaceBook", TypeName = "ntext")]
        [MaxLength(100)]
        public string FaceBook { get; set; }

        public int ContactsId { get; set; }

        //  public virtual Persone Persone { get; set; }
        public virtual List<Persone> Persone { get; set; }

        //check for validation of Contacts fields
        public bool ContactsValidation()
        {
            bool flag = true;

            if (Phone1 == "+380" && Phone2 == "---" && Phone3 == "---" && Email1 == "---" && Email2 == "---" && VK == "---" && FaceBook == "---")
                flag = false;

            return flag;
        }
    }
}
