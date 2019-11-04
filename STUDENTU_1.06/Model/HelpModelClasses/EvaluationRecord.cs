using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model.HelpModelClasses
{
    //это вспомогателеьный класс для работы _Evaluations.cs
    // this is a helper class for _Evaluations.cs
   public  class EvaluationRecord        
    {
        public EvaluationRecord()
        {
            this.FinalEvaluation = false;            
        }
        public string EvaluateDescription { get; set; }
        public DateTime DeadLine { get; set; }       
        public decimal Price { get; set; }
        public bool FinalEvaluation { get; set; }



    }
}
