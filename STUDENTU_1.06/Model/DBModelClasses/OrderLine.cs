using STUDENTU_1._06.Model.DBModelClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace STUDENTU_1._06.Model
{
   

    public class OrderLine : Helpes.ObservableObject , ICloneable
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

        //это свойство нужно для отслеживания кол-ва подзаказов, из которых состоит один заказ
        //в xaml от их колическва и знака применяется свой цвет заливки таких заказов
        // this property is needed to track the number of sub-orders of which one order consists
        // in xaml their color and sign apply their fill color for such orders
        
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
        

        public object Clone()
        {
            //тут пока будет глухомань, т.к. ветка User вообще не начата
            User user = new User()
            {
                //UserId = this.User.UserId,
                //Login = this.User.Login,
                //PassWord = this.User.PassWord,
                //Persone = persone,
                //OrderLine = new ObservableCollection<OrderLine>(this.User.OrderLine)
            };


            //тут тоже пока голяк
            Evaluation evaluation = new Evaluation()
            {
                //EvaluationId = this.Dates.Evaluation.EvaluationId,
                //Winner = this.Dates.Evaluation.Winner,
                //Description = this.Dates.Evaluation.Description,
                //Dates = new ObservableCollection<Dates>(this.Dates.Evaluation.Dates),
                //Moneys = new ObservableCollection<Money>(this.Dates.Evaluation.Moneys),
                //Authors = new ObservableCollection<Author>(this.Dates.Evaluation.Authors)
            };

            Dates dates = new Dates()
            {
                DatesId = this.Dates.DatesId,
                StartDateWork = this.Dates.StartDateWork,
                DateOfAuthorPaid = this.Dates.DateOfAuthorPaid,
                EndDateWork = this.Dates.EndDateWork,
                DayBirth = this.Dates.DayBirth,
                DeadLine = this.Dates.DeadLine,
                DateDone = this.Dates.DateDone,
                DateOfPaid = this.Dates.DateOfPaid,
                AuthorDeadLine = this.Dates.AuthorDeadLine,
                DateOfReception = this.Dates.DateOfReception,
                Evaluation = evaluation,
                Persone = (Persone)this.Client.Persone.Clone(),
                OrderLine = new ObservableCollection<OrderLine>(this.Dates.OrderLine)
            };
            //dates = (Dates)this.Dates.Clone();
            Money money= new Money()
            {
                Price = this.Money.Price,
                Prepayment = this.Money.Prepayment,
                PaidToAuthor = this.Money.PaidToAuthor,
                PaidByClient = this.Money.PaidByClient,
                AuthorPrice = this.Money.AuthorPrice,
                AuthorEvalPrice = this.Money.AuthorEvalPrice,
                OrderLine = new ObservableCollection<OrderLine>(this.Money.OrderLine),
                Evaluation = evaluation
            };
           
            
            
            //и тут пока голяк
            AfterDoneDescription afterDoneDescription = new AfterDoneDescription()
            {
                //AfterDoneDescriptionId = this.AfterDoneDescriptions.AfterDoneDescriptionId,
                //InputToBase = this.AfterDoneDescriptions.InputToBase,
                //PrintOreNot = this.AfterDoneDescriptions.PrintOreNot,
                //Binding = this.AfterDoneDescriptions.Binding,
                //BindingDiscription = this.AfterDoneDescriptions.BindingDiscription,
                //OrderLine = new ObservableCollection<OrderLine>(this.AfterDoneDescriptions.OrderLine)
            };
            
            return new OrderLine()
            {
                OrderLineId = this.OrderLineId,
                OrderNumber = this.OrderNumber,
                OrderCount = this.OrderCount,
                ParentId = this.ParentId,
                Variant = this.Variant,
                DescriptionForClient = this.DescriptionForClient,
                WorkDescription = this.WorkDescription,
                workInCredit = this.workInCredit,
                Saved = this.Saved,
                ParentOrder = this.ParentOrder,
                RedactionLog = this.RedactionLog,
                User = user,
                Client = (Client)this.Client.Clone(),
                Dates=dates,
                Money= money,
                Status=(Status)this.Status.Clone(),
                Subject=(Subject)this.Subject.Clone(),
                WorkType=(WorkType)this.WorkType.Clone(),
                Direction=(Direction)this.Direction.Clone(),
                Source=(Source)this.Source.Clone(),
                
                AfterDoneDescriptions=afterDoneDescription,
                Author= new ObservableCollection <Author> (this.Author)
            };
        }



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

       
        public static bool OrderLinesCompare (OrderLine obj1, OrderLine obj2)
        {

            if ((obj1.OrderLineId == obj2.OrderLineId) && (obj1.OrderNumber == obj2.OrderNumber) &&
                (obj1.OrderCount == obj2.OrderCount) && (obj1.ParentId == obj2.ParentId) &&
                (obj1.Variant == obj2.Variant) && (obj1.DescriptionForClient == obj2.DescriptionForClient) &&
                (obj1.WorkDescription == obj2.WorkDescription) && (obj1.WorkInCredit == obj2.WorkInCredit) &&
                (obj1.Saved == obj2.Saved) && (obj1.RedactionLog == obj2.RedactionLog) &&
                
                obj1.Client.CompareClients(obj1.Client,obj2.Client) && obj1.Dates.CompareDate(obj1.Dates, obj2.Dates)  &&
                obj1.Money.MoneyCompare(obj1.Money,obj2.Money) && obj1.Status.StatusCompare(obj1.Status,obj2.Status) &&
                obj1.Subject.CompareSubject(obj1.Subject,obj2.Subject) && obj1.WorkType.WorkTypeCompare(obj1.WorkType,obj2.WorkType) &&
                obj1.Direction.CompareDirection(obj1.Direction,obj2.Direction) && obj1.Source.CompareSource(obj1.Source, obj2.Source))
                return true;
            return false;
        }



    }
}
