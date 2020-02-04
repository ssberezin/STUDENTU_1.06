
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.DBModelClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class Author : Helpes.ObservableObject, ICloneable
    {

      
        public Author()
        {
            this.Punctually = 0;
            this.WorkQuality = 0;
            this.Responsibility = 0;
            this.CompletionCompliance = 0;
            this.Rating =0;

            this.Subject = new ObservableCollection<Subject>();
            this.Direction = new ObservableCollection<Direction>();
            this.Evaluation = new ObservableCollection<Evaluation>();
            this.OrderLine = new ObservableCollection<OrderLine>();
            
            // this.Sourse = "1000";
        }

        [Column("Sourse", TypeName = "ntext")]
        [MaxLength(2000)]
        public string Source { get; set; }

        //properties for forming a retention rate
        public int Punctually { get; set; }
        public int WorkQuality { get; set; }
        public int Responsibility { get; set; }
        public int CompletionCompliance { get; set; }//сговорчивость по доработкам

        private double rating;
        public double Rating
        {
            get { return rating; }
            set
            {
                if (rating != value)
                {
                    rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        public int AuthorId { get; set; }        

        public virtual Persone Persone { get; set; }
        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }
        public virtual ObservableCollection<Subject> Subject { get; set; }
        public virtual ObservableCollection<Direction> Direction { get; set; }
        public virtual ObservableCollection<Evaluation> Evaluation { get; set; }
        public virtual AuthorStatus AuthorStatus { get; set; }


        public double RatingCreate()
        {
            return (Punctually + WorkQuality + Responsibility + CompletionCompliance) / 4.0;
        }

        public Evaluation GetWinnerEvaluation(Author author)
        {

            foreach (var item in author.Evaluation)
            {
                if (item.Winner == true)
                        return item;
            }
            return null;
        }

        public object Clone()
        {
            return new Author()
            {
                //AuthorId= this.AuthorId,
                //Source= this.Source,
                //Punctually= this.Punctually,
                //WorkQuality= this.WorkQuality,
                //Responsibility= this.Responsibility,
                //CompletionCompliance= this.CompletionCompliance,
                //Rating= this.Rating,
                //Persone=(Persone)this.Persone.Clone(this.Persone),
                //OrderLine=new ObservableCollection<OrderLine>(this.OrderLine),
                //Subject=new ObservableCollection<Subject>(this.Subject),
                //Direction=new ObservableCollection<Direction>(this.Direction),
                //Evaluation=new ObservableCollection<Evaluation>(this.Evaluation),
                //AuthorStatus=(AuthorStatus)this.AuthorStatus.Clone()
            };
          
        }
    }
}
