using DoO_CRM.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

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

        public static Order ReceivingAnOrder(NetworkStream stream)
        {
            byte[] data = new byte[512];
            int bytesOfData = stream.Read(data, 0, data.Length);
            var orderData = new StringBuilder();

            do
            {
                orderData.Append(Encoding.UTF8.GetString(data, 0, bytesOfData));
            }
            while (stream.DataAvailable);

            Order sendedOrder = JsonSerializer.Deserialize<Order>(orderData.ToString());
            return sendedOrder;
        }
        public static bool SendAnswer(NetworkStream stream, bool isOrderInAnQueue)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(isOrderInAnQueue, options);

            stream.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);


            return true;
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
