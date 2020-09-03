using DoO_CRM.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoO_CRM.BL.Controller
{
    public static class ProductController
    {
        public static List<Product> GetTop100Products(bool ascOrDesc, DoO_CRMContext context)
        {
            var productsFromDB = new List<Product>();

            productsFromDB.AddRange(context.Products
                          .Where(prod => prod.Id < 101));

            if (ascOrDesc == true) //А почему бы и не добавить эту фичу?
            {
                productsFromDB.OrderByDescending(prod => prod.Id);
            }

            return productsFromDB;
        }
        //public static List<string> ShowProductsOfCart(Cart cart, bool toBeWrite)
        //{
        //    if (cart != null && cart.Sells != null)
        //    {
        //        var sortProducts = new List<string>();

        //        foreach (var product in cart.Products)
        //        {
        //            int count = cart.Products.Count(prod => prod.Equals(product));
        //            string stringWithData = $"{product.Name}, количество - {count} шт.";

        //            if (sortProducts.Any(strig => strig.Equals(stringWithData)))
        //            {
        //                cart.Products.RemoveAll(strig => strig.Equals(stringWithData));
        //            }
        //            else
        //            {
        //                sortProducts.Add(stringWithData);
        //            }
        //        }
        //        if (toBeWrite)
        //        {
        //            foreach (var str in sortProducts)
        //            {
        //                Console.WriteLine(str);
        //            }
        //        }
        //        return sortProducts;
        //    }
        //    return null;
        //}
    }
}
