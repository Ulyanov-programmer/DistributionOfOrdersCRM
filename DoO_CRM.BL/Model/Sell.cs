using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoO_CRM.BL.Model
{
    public class Sell
    {
        public Sell(Product product, int productId, int countOfProduct, int clientId)
        {
            Product = product;
            ProductId = productId;
            CountOfProduct = countOfProduct;
            ClientId = clientId;
        }
        public Sell() { }


        public int SellId { get; set; }
        public int CountOfProduct { get; set; }

        #region References

        public int? ClientId { get; set; }
        public int ProductId { get; set; }
        public Client Client { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }

        #endregion

        #region overrides
        public override string ToString()
        {
            return @$"Продукт ""{Product.Name}"", количество - {CountOfProduct} шт.";
        }
        #endregion
    }
}
