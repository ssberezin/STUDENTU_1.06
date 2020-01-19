using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model.DBModelClasses
{
    public class AuthorStatus:ICloneable
    {
        public AuthorStatus()
        {
            this.Author = new ObservableCollection<Author>();
            //this.AuthorStatusName = "не проверен";
        }

        public int AuthorStatusId { get; set; }

        [Column("Description", TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Description { get; set; }
        [Column("StatusName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string AuthorStatusName { get; set; }

        public virtual ObservableCollection<Author> Author { get; set; }

        public object Clone()
        {
            return new AuthorStatus()
            {
                //AuthorStatusId=this.AuthorStatusId,
                //Description=this.Description,
                //AuthorStatusName=this.AuthorStatusName,
                //Author=new ObservableCollection<Author>(this.Author)
            };

        }
    }
}
