
using System.Collections.ObjectModel;


namespace STUDENTU_1._06.Model.HelpModelClasses
{
    public class AuthorsRecord
    {
        public AuthorsRecord()
        {
            this.EvaluationRecords = new ObservableCollection<EvaluationRecord>();
        }

        public int AuthorRecordId { get; set; }        
        public Persone Persone { get; set; }
        public string Sourse { get; set; }
        public Contacts Contacts { get; set; }
        public ObservableCollection<EvaluationRecord> EvaluationRecords { get; set; }

    }
}
