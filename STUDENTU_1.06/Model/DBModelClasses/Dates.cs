
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
            this.StartDateWork = DateTime.Now;
            this.EndDateWork = new DateTime(2100, 1, 1);
            this.DayBirth = new DateTime(1900, 1, 1);
            this.DeadLine = DateTime.Now.AddDays(1);
            DeadLine=DeadLine.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddHours(8);
            this.DateDone = new DateTime(1900, 1, 1);
            this.DateOfPaid = new DateTime(1900, 1, 1);
            this.AuthorDeadLine = DeadLine;
            this.DateOfReception = DateTime.Now;
            
        }

         

        public int DatesId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartDateWork { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ReceptionTime { get; set; }

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
        public System.DateTime DateOfReception { get; set; }

        public virtual Evaluation Evaluation { get; set; }
        public virtual Persone Persone { get; set; }
        public virtual List<OrderLine> OrderLine { get; set; }
        
    }
}
