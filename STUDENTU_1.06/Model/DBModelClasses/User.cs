
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class User
    {       
        public User()
        {
            this.OrderLine = new List<OrderLine>();
        }
        public int UserId { get; set; }

        [Column("Login", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Login { get; set; }
        [Column("PassWord", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string PassWord { get; set; }        

        public virtual Persone Persone { get; set; }
        public virtual List<OrderLine> OrderLine { get; set; }

    }
}
