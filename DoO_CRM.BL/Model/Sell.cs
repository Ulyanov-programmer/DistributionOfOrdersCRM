using System;

namespace DoO_CRM.BL.Model
{
    public class Sell
    {
        public Sell(Product product, Order order, int countOfProduct)
        {
            ProductId = product.Id;
            Product = product;
            CountOfProduct = countOfProduct;
        }
        public Sell() { }
        public int Id { get; set; }
        public int CountOfProduct { get; set; }

        #region References

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Order Order = new Order();

        #endregion

        public override string ToString()
        {
            return @$"Продукт ""{Product.Name}"", количество - {CountOfProduct} шт.";
        }
    }
}
