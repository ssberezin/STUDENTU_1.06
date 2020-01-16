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
            this.WorkInCredit = false;
            this.OrderCount = 1;
            this.Author = new ObservableCollection<Author>();
            this.variant = null;
            this.Saved = false;
            this.ParentOrder = true;
        }
        [Key]
        public int OrderLineId { get; set; }
        public int OrderNumber { get; set; }

        //это свойство нужно для отслеживания кол-ва подзаказов, из которіх состоит один заказ
        // this property is needed to track the number of sub-orders of which one order consists
        public int OrderCount { get; set; }
        //используется как один из признаков того, что заказ разбит на подзаказы
        // used as one of the signs that the order is broken into sub-orders
        public int ParentId { get; set; }

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
        private bool workInCredit ;
        public bool WorkInCredit 
        {
            get { return workInCredit; }
            set
            {
                if (workInCredit != value)
                {
                    workInCredit = value;
                    OnPropertyChanged(nameof(WorkInCredit));
                }
            }
        }

        //для того, чтоб отслеживать состояние заказа из ...ForEditOrder.cs например. Это нужно для редактирования 
        // in order to track the status of the order from ... ForEditOrder.cs for example. This is for editing.
        private bool saved;
        [NotMapped]
        public bool Saved
        {
            get { return saved; }
            set
            {
                if (saved != value)
                {
                    saved = value;
                    OnPropertyChanged(nameof(Saved));
                }
            }
        }
        //для того, чтоб отслеживать , имеет ли текущий заказ подзаказы
        //это нужно для вариантивной подсветки заказов разных клиентов в XAML
        //отслеживание будет по  ParentOrder и OrderCount

        // in order to track whether the current order has sub-orders
        // this is necessary for the optional highlighting of orders of different customers in XAML
        // tracking will be by ParentOrder and OrderCount

        [NotMapped]
        private bool parentOrder;
        public bool ParentOrder
        {
            get { return parentOrder; }
            set
            {
                if (parentOrder != value)
                {
                    parentOrder = value;
                    OnPropertyChanged(nameof(ParentOrder));
                }
            }
        }


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
