
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model.DBModelClasses
{
    public class Source
    {
        public Source()
        {
            this.OrderLine = new ObservableCollection<OrderLine>();
        }

        public int SourceId {  get;set;}

        [Column("SourceName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string SourceName { get; set; }

        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }
    }


}
