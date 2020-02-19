using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STUDENTU_1._06.Model.HelpModelClasses
{
    //это вспомогателеьный класс для работы _Evaluations.cs
    // this is a helper class for _Evaluations.cs
   public  class EvaluationRecord : Helpes.ObservableObject
    {
        public EvaluationRecord()
        {
            this.FinalEvaluation = false;
            this.EvalCopyId = 0;
        }
      
        public int EvalCopyId { get; set; }
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


        public bool CompareEvaluationRecords (EvaluationRecord obj1, EvaluationRecord obj2)
        {
            if (obj1.EvalCopyId == obj2.EvalCopyId && obj1.EvaluateDescription == obj2.EvaluateDescription &&
                obj1.Price == obj2.Price && obj1.DeadLine == obj2.DeadLine)
                return true;
            else
                return false;
        }

        public bool CompareEvaluationRecordsWNotId(EvaluationRecord obj1, EvaluationRecord obj2)
        {
            if (obj1.EvaluateDescription == obj2.EvaluateDescription &&
                obj1.Price == obj2.Price && obj1.DeadLine == obj2.DeadLine)
                return true;
            else
                return false;
        }
        //не всегда корректно работает, пришлось отказаться      
        //public ObservableCollection<EvaluationRecord>  GetEvaluationRecordCollection(OrderLine order)
        //{
        //    using (StudentuConteiner db = new StudentuConteiner())
        //    {
        //        try
        //        {
        //            EvaluationRecord evRecord;
        //            ObservableCollection<EvaluationRecord> evRecords = new ObservableCollection<EvaluationRecord>(); 
        //            foreach (var item in order.Evaluations)
        //            {
        //                int i = 0;
        //                evRecord = new EvaluationRecord()
        //                {
        //                    EvalCopyId = item.EvaluationId,
        //                    DeadLine = item.Dates[i].AuthorDeadLine,
        //                    Price = item.Moneys[i].AuthorPrice,
        //                    EvaluateDescription = item.Description,
        //                    FinalEvaluation = item.Winner
        //                };
        //                evRecords.Add(evRecord);
        //                i++;
        //            }
        //            return evRecords;
        //        }
        //        catch (ArgumentNullException ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        catch (System.Data.SqlClient.SqlException ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        catch (System.Data.Entity.Core.EntityException ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        return null;
        //    }
        //}

        public object Clone( DateTime date, Decimal price)
        {
            return new EvaluationRecord()
            {
                EvalCopyId = this.EvalCopyId,
                DeadLine = date,
                Price = price,
                EvaluateDescription = this.EvaluateDescription,
                FinalEvaluation = this.FinalEvaluation              
            };
        }

    }
}
