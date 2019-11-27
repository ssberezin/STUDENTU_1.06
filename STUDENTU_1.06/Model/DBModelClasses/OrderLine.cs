using STUDENTU_1._06.Model.DBModelClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class OrderLine : Helpes.ObservableObject
    {
        
        public OrderLine()
        {
           // this.AfterDoneDescriptions = new List<AfterDoneDescription>();
            this.WorkInCredit = true;
            this.OrderCount = 1;
            this.Author = new ObservableCollection<Author>();
            this.variant = null;           
        }

        public int OrderLineId { get; set; }
        public int OrderNumber { get; set; }

        //это свойство нужно для отслеживания кол-ва подзаказов, на котрое может разбиться один заказ
        // this property is needed to track the number of sub-orders on which one order can be broken
        public int OrderCount { get; set; }

               
        [Column("Variant", TypeName = "nvarchar")]
        [MaxLength(255)]
        private string  variant;
        public string Variant
        {
            get { return variant; }
            set
            {
                if (variant != value)
                {
                    variant = value;
                    OnPropertyChanged(nameof(Variant));
                }
            }
        }

        [Column("DescriptionForClient", TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string DescriptionForClient { get; set; }

        [Column("WorkDescription", TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string WorkDescription { get; set; }        
       
        public bool WorkInCredit { get; set; }

        [Column("RedactionLog", TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string RedactionLog { get; set; }

     
        public virtual User User { get; set; }
        public virtual Client Client { get; set; }     
        public virtual Dates Dates { get; set; }
        public virtual Money Money { get; set; }
        public virtual Status Status { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual WorkType WorkType { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual Source Source { get; set; }
        public virtual AfterDoneDescription AfterDoneDescriptions { get; set; }

        public virtual ObservableCollection<Author> Author { get; set; }

        public Author GetExecuteAuthor(ObservableCollection<Author> authors)
        {
                       
            foreach (Author item in authors)
            {
                foreach (Evaluation i in item.Evaluation)
                    if (i.Winner == true)                   
                        return item;
            }
            return null;
        }

    }
}
