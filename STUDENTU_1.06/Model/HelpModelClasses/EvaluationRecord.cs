using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model.HelpModelClasses
{
   public  class EvaluationRecord        
    {
        public EvaluationRecord()
        {
            this.FinalEvaluation = false;
        }

        //public int EvaluationRecordId { get; set; }
        //public int OrderNumber { get; set; }
        public string EvaluateDescription { get; set; }
        public DateTime DeadLine { get; set; }       
        public decimal Price { get; set; }
        public bool FinalEvaluation { get; set; }



    }
}
