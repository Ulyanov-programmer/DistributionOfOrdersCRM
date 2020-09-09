using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int SellId { get; set; }
        public int CountOfProduct { get; set; }

        #region References

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }

        #endregion

        public override string ToString()
        {
            return @$"Продукт ""{Product.Name}"", количество - {CountOfProduct} шт.";
        }
    }
}
