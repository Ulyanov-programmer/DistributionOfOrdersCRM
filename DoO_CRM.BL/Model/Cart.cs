using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoO_CRM.BL.Model
{
    public class Cart
    {
        public Cart(Client client, Product product, int countOfProduct, DoO_CRMContext context)
        {
            Client = client;
            AddProduct(product, countOfProduct, context);
        }
        public Cart(Client client)
        {
            Client = client;
        }
        
        public Client Client { get; set; }
        public List<Sell> Sells = new List<Sell>();

        public bool AddProduct(Product product, int countOfProducts, DoO_CRMContext context)
        {
            var productFromDB = context.Products.FirstOrDefault(prod => prod.Name == product.Name);
            if (productFromDB != default)
            {
                Sell newSell = new Sell(product, null, countOfProducts);

                Sells.Add(newSell);
                return true;
            }
            return false;
        }
        public bool AddProduct(List<Sell> inputSells)
        {
            Sells.AddRange(inputSells);
            return true;
        }
    }
}
