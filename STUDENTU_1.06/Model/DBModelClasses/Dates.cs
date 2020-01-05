
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class Dates
    {

        public Dates ()
            {

            // this.Evaluations = new List<Evaluation>();
            this.OrderLine = new List<OrderLine>();
            this.StartDateWork = ZeroDefaultDate( DateTime.Now);
            this.EndDateWork = new DateTime(1900, 1, 1);
            this.DayBirth = new DateTime(1900, 1, 1);
            this.DeadLine = ZeroDefaultDate(DateTime.Now).AddDays(1).AddHours(9);            
            this.DateDone = new DateTime(1900, 1, 1);
            this.DateOfPaid = new DateTime(1900, 1, 1);
            this.DateOfAuthorPaid = new DateTime(1900, 1, 1);
            this.AuthorDeadLine = DeadLine.AddMinutes(-30);           
            this.DateOfReception = DateTime.Now;
           


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

        
       


        public virtual Evaluation Evaluation { get; set; }
        public virtual Persone Persone { get; set; }
        public virtual List<OrderLine> OrderLine { get; set; }


        //тут мы получаем только год , месяц, число, с нулевыми остальными показателями
        // here we get only the year, month, day, with zero other indicators
        public DateTime ZeroDefaultDate(DateTime date)
        {           
            return date.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second).AddMilliseconds(-DateTime.Now.Millisecond);
        }
    }
}
