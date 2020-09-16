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
            AddProduct(product.Id, countOfProduct, client.Id, context);
        }
        public Cart(Client client)
        {
            Client = client;
        }
        
        public Client Client { get; set; }
        public List<Sell> Sells = new List<Sell>();

        public bool AddProduct(int productId, int countOfProducts, int clientId, DoO_CRMContext context)
        {
            var productFromDB = context.Products.FirstOrDefault(prod => prod.Id == productId);
            if (productFromDB != default)
            {
                Sell newSell = new Sell(productFromDB,productId, countOfProducts, clientId);

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
