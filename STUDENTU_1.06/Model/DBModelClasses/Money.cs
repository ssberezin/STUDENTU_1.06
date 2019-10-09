
using System.Collections.Generic;


namespace STUDENTU_1._06.Model
{
    public class Money
    {
        public Money()
        {
            this.Price = 0;
            this.Prepayment = 0;
            this.PaidToAuthor = false;
            this.PaidByClient = false;
            this.AuthorPrice = 0;
            this.AuthorEvalPrice = 0;            
            this.OrderLine = new List<OrderLine>();
        }


        public int MoneyId { get; set; }

        public decimal Price { get; set; }
        public decimal Prepayment { get; set; }
        public bool PaidToAuthor { get; set; }
        public bool PaidByClient { get; set; }
        public decimal AuthorPrice { get; set; }
        public decimal AuthorEvalPrice { get; set; }


        public virtual List<OrderLine> OrderLine { get; set; }       
        public virtual Evaluation Evaluation { get; set; }
    }
}
