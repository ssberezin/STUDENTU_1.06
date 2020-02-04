
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace STUDENTU_1._06.Model
{
    public class Money : ICloneable
    {
        public Money()
        {
            this.Price = 0;
            this.Prepayment = 0;
            this.PaidToAuthor = false;
            this.PaidByClient = false;
            this.AuthorPrice = 0;
            this.AuthorEvalPrice = 0;            
            this.OrderLine = new ObservableCollection<OrderLine>();
        }


        public int MoneyId { get; set; }

        public decimal Price { get; set; }
        public decimal Prepayment { get; set; }
        public bool PaidToAuthor { get; set; }
        public bool PaidByClient { get; set; }
        public decimal AuthorPrice { get; set; }
        public decimal AuthorEvalPrice { get; set; }


        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }       
        public virtual Evaluation Evaluation { get; set; }

        public bool CompareMoney(Money obj1, Money obj2)
        {
            if (obj1.Prepayment == obj2.Prepayment &&
                obj1.Price == obj2.Price &&
                obj1.PaidToAuthor == obj2.PaidToAuthor &&
                obj1.PaidByClient == obj2.PaidByClient &&
                obj1.AuthorEvalPrice == obj2.AuthorPrice &&
                obj1.AuthorPrice == obj2.AuthorPrice)
                return true;
            return false;
        }

        public object Clone()
        {
            return new Money()
            {
            //Price= this.Price,
            //Prepayment= this.Prepayment,
            //PaidToAuthor= this.PaidToAuthor,
            //PaidByClient= this.PaidByClient,
            //AuthorPrice= this.AuthorPrice,
            //AuthorEvalPrice= this.AuthorEvalPrice,
            //OrderLine = new List<OrderLine>(this.OrderLine),
            //Evaluation=(Evaluation)this.Evaluation.Clone()
            };
        }
    }
}
