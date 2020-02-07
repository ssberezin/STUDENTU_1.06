
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace STUDENTU_1._06.Model
{
    public class Dates:ICloneable
    {

        public Dates ()
            {

            // this.Evaluations = new List<Evaluation>();
          
            this.StartDateWork = ZeroDefaultDate( DateTime.Now);
            this.EndDateWork = new DateTime(1900, 1, 1);
            this.DayBirth = new DateTime(1900, 1, 1);
            this.DeadLine = ZeroDefaultDate(DateTime.Now).AddDays(1).AddHours(9);            
            this.DateDone = new DateTime(1900, 1, 1);
            this.DateOfPaid = new DateTime(1900, 1, 1);
            this.DateOfAuthorPaid = new DateTime(1900, 1, 1);
            this.AuthorDeadLine = DeadLine.AddMinutes(-30);           
            this.DateOfReception = DateTime.Now;

            this.OrderLine = new ObservableCollection<OrderLine>();

        }

         

        public int DatesId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartDateWork { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateOfAuthorPaid { get; set; }

        [Column(TypeName = "datetime2")]
        public System.DateTime EndDateWork { get; set; }

        [Column(TypeName = "datetime2")]
        public System.DateTime DayBirth { get; set; }

        [Column(TypeName = "datetime2")]
        public System.DateTime DeadLine { get; set; }

        [Column(TypeName = "datetime2")]
        public System.DateTime DeadLineTime { get; set; }

        [Column(TypeName = "datetime2")]
        public System.DateTime DateDone { get; set; }

        [Column(TypeName = "datetime2")]
        public System.DateTime DateOfPaid { get; set; }

        [Column(TypeName = "datetime2")]
        public System.DateTime AuthorDeadLine { get; set; }

        [Column(TypeName = "datetime2")]
        //date of order reception
        public System.DateTime DateOfReception { get; set; }

        [Column(TypeName = "datetime2")]
        //date of order reception
        public System.DateTime DateOfPrepayment { get; set; }



        public virtual Evaluation Evaluation { get; set; }
        public virtual Persone Persone { get; set; }
        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }


        //пока норм не работает, т.к. нужно решить проблему с инициацией Evaluation
        public object Clone()
        {
            return new Dates()
            {
                DatesId = this.DatesId,
                StartDateWork = this.StartDateWork,
                DateOfAuthorPaid = this.DateOfAuthorPaid,
                EndDateWork = this.EndDateWork,
                DayBirth = this.DayBirth,
                DeadLine = this.DeadLine,
                DateDone = this.DateDone,
                DateOfPaid = this.DateOfPaid,
                AuthorDeadLine = this.AuthorDeadLine,
                DateOfReception = this.DateOfReception,

                Evaluation = (Evaluation)this.Evaluation.Clone(),
                Persone = (Persone)this.Persone.Clone(),
                OrderLine = new ObservableCollection<OrderLine>(OrderLine)
            };
        }
        public bool CompareDate(Dates obj1, Dates obj2)
        {           
            if (obj1.AuthorDeadLine == obj2.AuthorDeadLine &&
                obj1.EndDateWork == obj2.EndDateWork &&
                obj1.StartDateWork == obj2.StartDateWork &&
                obj1.DeadLine == obj2.DeadLine &&
                obj1.DateDone == obj2.DateDone &&
                obj1.DateOfPaid == obj2.DateOfPaid &&
                obj1.DateOfAuthorPaid == obj2.DateOfAuthorPaid &&
                obj1.DateOfReception == obj2.DateOfReception&&
                obj1.DateOfPrepayment == obj2.DateOfPrepayment)
                return true;
            return false;
        }

        //тут мы получаем только год , месяц, число, с нулевыми остальными показателями
        // here we get only the year, month, day, with zero other indicators
        public DateTime ZeroDefaultDate(DateTime date)
        {           
            return date.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second).AddMilliseconds(-DateTime.Now.Millisecond);
        }

      

       
    }
}
