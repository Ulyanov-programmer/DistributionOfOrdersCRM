using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoO_CRM.BL.Model
{
    public class Cart
    {
        public Cart(Client client, string productName, List<Product> productsFromDB)
        {
            Client = client;
            AddProduct(productName, productsFromDB);
        }
        public Cart(Client client)
        {
            Client = client;
            Products = new List<Product>();
        }
        
        public Client Client { get; set; }
        public List<Product> Products { get; set; }

        public bool AddProduct(string productName, List<Product> productsFromDB)
        {
            var product = productsFromDB.FirstOrDefault(prod => prod.Name == productName);
            if (product != default)
            {
                Products.Add(product);
                return true;
            }
            return false;
        }
        public bool AddProduct(List<Product> inputProducts)
        {
            Products.AddRange(inputProducts);
            return true;
        }
    }
}
