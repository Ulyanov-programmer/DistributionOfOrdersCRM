using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoO_CRM.BL.Model
{
    public class Cart
    {
        public Cart(Product product, int countOfProduct, DoO_CRMContext context, Client optionalClient = null)
        {
            Client = optionalClient;
            if (optionalClient is null)
            {
                AddProduct(product, countOfProduct, context);
            }
            else
            {
                AddProduct(product, countOfProduct, context, optionalClient.Id);
            }
        }
        public Cart(Client client = null)
        {
            Client = client;
        }


        public Client Client { get; set; }
        public List<Sell> Sells = new List<Sell>();


        public bool AddProduct(Product product, int countOfProducts, DoO_CRMContext context, int? optionalClientId = null)
        {
            var productFromDB = context.Products.FirstOrDefault(prod => prod.Id == product.Id);

            if (productFromDB != default)
            {
                Sell newSell = new Sell(productFromDB, product.Id, countOfProducts, optionalClientId);

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

        public bool AddProduct(List<Product> inputProducts, int? optionalClientId = null)
        {
            foreach (var product in inputProducts)
            {
                if (product != null)
                {
                    Sell newSell = new Sell(product, product.Id, 1, optionalClientId);

                    Sells.Add(newSell);
                }
            }
            return true;
        }
    }
}
