﻿using DoO_CRM.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoO_CRM.BL.Controller
{
    public static class ProductController
    {
        public static List<Product> GetTop100Products(bool ascOrDesc, DoO_CRMContext context)
        {
            List<Product> productsFromDB = new List<Product>();

            productsFromDB.AddRange(context.Products
                          .Where(prod => prod.Id < 101));

            if (ascOrDesc == true) //А почему бы и не добавить эту фичу?
            {
                productsFromDB.OrderByDescending(prod => prod.Id);
            }

            return productsFromDB;
        }
        public static List<string> ShowProducts(Cart cart, bool toBeWrite)
        {
            if (cart != null && cart.Products != null)
            {
                var sortProducts = new List<string>();

                foreach (var product in cart.Products)
                {
                    var countOfProd = cart.Products.Count(prod => prod.Name == product.Name);

                    sortProducts.Add($"{product.Name}, количество - {countOfProd} шт.");
                }
                if (toBeWrite)
                {
                    foreach (var str in sortProducts)
                    {
                        Console.WriteLine(str);
                    }
                }
                return sortProducts;
            }
            return null;
        }
    }
}