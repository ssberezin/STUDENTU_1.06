using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model.HelpModelClasses
{
    //это вспомогателеьный класс для работы _Evaluations.cs
    // this is a helper class for _Evaluations.cs
   public  class EvaluationRecord : Helpes.ObservableObject
    {
        public EvaluationRecord()
        {
            this.FinalEvaluation = false;            
        }
       // public string EvaluateDescription { get; set; }
        //public DateTime DeadLine { get; set; }       
       // public decimal Price { get; set; }
        public bool FinalEvaluation { get; set; }

        private string evaluateDescription;
        public string EvaluateDescription
        {
            get { return evaluateDescription; }
            set
            {
                if (evaluateDescription != value)
                {
                    evaluateDescription = value;
                    OnPropertyChanged(nameof(EvaluateDescription));
                }
            }
        }

        private decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        private DateTime deadLine;
        public DateTime DeadLine
        {
            get { return deadLine; }
            set
            {
                if (deadLine != value)
                {
                    deadLine = value;
                    OnPropertyChanged(nameof(DeadLine));
                }
            }
        }


    }
}
