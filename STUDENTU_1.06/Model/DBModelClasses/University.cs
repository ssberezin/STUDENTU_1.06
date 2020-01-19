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
    public class University:ICloneable
    {
        public University()
        {
            this.Clients = new ObservableCollection<Client>();
        }
        public int UniversityId { get; set; }

        [Column("UniversityName", TypeName = "ntext")]
        [MaxLength(500)]
        public string UniversityName { get; set; }

        [Column("City", TypeName = "ntext")]
        [MaxLength(100)]
        public string City { get; set; }        
        

        public virtual ObservableCollection<Client> Clients { get; set; }

        public object Clone()
        {
            return new University()
            {
                //UniversityId = this.UniversityId,
                //UniversityName=this.UniversityName,
                //City=this.City,
                //Clients=new ObservableCollection<Client>(this.Clients)
            };
        }
    }
}
