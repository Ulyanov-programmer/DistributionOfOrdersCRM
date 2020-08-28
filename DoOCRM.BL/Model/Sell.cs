using System;

namespace DoO_CRM.BL.Model
{
    public class Sell
    {
        public Sell(int productId, Product product, Order order)
        {
            ProductId = productId;
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Order = order ?? throw new ArgumentNullException(nameof(order));
        }
        public Sell() { }
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Order Order { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
