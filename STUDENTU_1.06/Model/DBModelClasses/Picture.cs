using STUDENTU_1._06.Helpes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model.HelpModelClasses
{
    public class Picture
        
    {
        public int ImageId { get; set; }


        [Column("FileName", TypeName = "ntext")]
        [MaxLength(500)]
        public string FileName { get; set; }

        public virtual Persone Persone { get; set; }

        //[Column("Image", TypeName = "varbinary(max)")]        
        //public byte[] Image { get; set; }



    }
}
