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
        public static List<Product> GetTop10Products(bool ascOrDesc, DoO_CRMContext context)
        {
            if (context.Products.Count() > 0)
            {
                var productsFromDB = new List<Product>();

                productsFromDB.AddRange(context.Products
                              .Where(prod => prod.Id < 11));

                if (ascOrDesc == true) //А почему бы и не добавить эту фичу?
                {
                    productsFromDB.OrderByDescending(prod => prod.Id);
                }
                return productsFromDB;
            }
            return null;
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

        public static List<string> ShowProductsInCart(Cart cart, DoO_CRMContext context, bool toBeWrite = false)
        {
            if (cart != null && cart.Sells != null)
            {
                var data = new List<string>();

                foreach (var sell in cart.Sells)
                {
                    var product = context.Products.FirstOrDefault(prod => prod.Id == sell.ProductId);

                    if (product != default)
                    {
                        data.Add($"{product.Name}, стоимость: {product.Cost}");
                    }
                }
                if (toBeWrite)
                {
                    foreach (var str in data)
                    {
                        Console.WriteLine(str);
                    }
                }
                return data;
            }
            return null;
        }
    }
}
