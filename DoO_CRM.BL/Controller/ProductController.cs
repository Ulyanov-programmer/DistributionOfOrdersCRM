using DoO_CRM.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace DoO_CRM.BL.Controller
{
    /// <summary>
    /// Класс-контроллер, для работы с таблицей продуктов
    /// </summary>
    public static class ProductController
    {
        /// <summary>
        /// Возвращает 10 экземпляров Product из БД. Иначе возвращает null.
        /// </summary>
        /// <param name="context"> Экземпляр класса контекста, необходимый для подключения к БД. </param>
        /// <param name="ascOrDesc"> True (по умолчанию) - возвращает первые 10 экземпляров, false - последние 10. </param>
        /// <returns></returns>
        public static List<Product> GetTop10Products(DoO_CRMContext context, bool ascOrDesc = true)
        {
            if (context.Products.Count() > 0 && context != null)
            {
                var productsFromDB = new List<Product>();

                productsFromDB.AddRange(context.Products
                              .Where(prod => prod.Id < 11));

                if (ascOrDesc is false) //А почему бы и не добавить эту фичу?
                {
                    productsFromDB.OrderByDescending(prod => prod.Id);
                }
                return productsFromDB;
            }
            return null;
        }

        /// <summary>
        /// Читает из входного потока экземпляр класса Order.
        /// </summary>
        /// <param name="stream"> Поток, из которого будет проводится чтение. </param>
        /// <returns> Если данные были успешно прочитаны, возвращает прочитанный экземпляр класса Order.
        ///           Иначе - возвращает null.
        /// </returns>
        public static Order ReceivingAnOrder(NetworkStream stream)
        {
            if (stream != null && stream.CanRead is true && stream.Length > 0)
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
            return null;
        }

        /// <summary>
        /// Отправляет через входной поток массив байтов,
        /// содержащих значение того, был ли сохранён экземпляр Order в очереди или нет.
        /// </summary>
        /// <param name="stream"> Поток, через который будет отправлено сообщение. </param>
        /// <param name="isOrderInAnQueue"> Значение того, был ли сохранён экземпляр Order в очереди или нет. </param>
        /// <returns> В случае успешной отправки - возвращает true, иначе - false. </returns>
        public static bool SendAnswer(NetworkStream stream, bool isOrderInAnQueue)
        {
            if (stream != null && stream.CanRead is true && stream.Length > 0)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(isOrderInAnQueue, options);

                    stream.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка:\n " + ex.Message);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Считывает информацию о экземплярах Sell в объекте Cart.
        /// </summary>
        /// <param name="cart"> Объект Cart, из списка экземпляров Sell будут считываться данные. </param>
        /// <param name="context"> Экземпляр класса контекста, необходимый для подключения к БД. </param>
        /// <param name="toBeWrite"> True - данные из возвращаемого списка будут выведены на экран консоли. </param>
        /// <returns> В случае успешного считывания - возвращает информацию в виде списка строк.
        ///           Иначе - null.
        /// </returns>
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
