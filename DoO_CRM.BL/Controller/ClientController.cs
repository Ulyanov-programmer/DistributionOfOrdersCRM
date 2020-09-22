using DoO_CRM.BL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace DoO_CRM.BL.Controller
{
    /// <summary>
    /// Класс-контроллер для работы с клиентами с необходимыми для них методами.
    /// Так же хранит константы IP-адреса и порта.
    /// </summary>
    public static class ClientController
    {
        /// <summary>
        /// Проверяет наличие клиента с указанным именем в БД.
        /// </summary>
        /// <param name="nameOfClient"> Имя пользователя, необходимое для поиска. </param>
        /// <param name="context"> Класс контекста для подключения к БД. </param>
        /// <returns>
        /// Возвращает данные о найденном клиенте в виде класса Client. Иначе - возвращает null.
        /// </returns>
        public static Client GetRegistered(string nameOfClient, DoO_CRMContext context)
        {
            if (string.IsNullOrWhiteSpace(nameOfClient) is false && context != null)
            {
                Client clientFromDB = context.Clients.FirstOrDefault(client => client.Name == nameOfClient);

                if (clientFromDB != default)
                {
                    return clientFromDB;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Регистрирует клиента в БД.
        /// </summary>
        /// <param name="nameOfClient"> Имя нового клиента. </param>
        /// <param name="context"> Класс контекста для подключения к БД. </param>
        /// <param name="optionalBalanceOfClient"> Необязательный параметр, указывающий стартовый баланс
        ///                                        нового пользователя. </param>
        /// <returns>
        /// При удачном добавлении и сохранении клиента, возвращает его данные в виде класса Client. Иначе - null.
        /// </returns>
        public static Client Registration(string nameOfClient, DoO_CRMContext context, decimal optionalBalanceOfClient = 0)
        {
            if (string.IsNullOrWhiteSpace(nameOfClient) is false && context != null)
            {
                Client newClient = new Client(nameOfClient, optionalBalanceOfClient);

                if (context.Clients.FirstOrDefault(clnt => clnt.Name == newClient.Name) == default)
                {
                    context.Clients.Add(newClient);
                    context.SaveChanges();

                    return newClient;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Регистрирует клиента в БД.
        /// </summary>
        /// <param name="newClient"> Класс нового клиента, на основе которого будет добавлен пользователь. </param>
        /// <param name="context"> Класс контекста для подключения к БД. </param>
        /// <returns>
        /// При удачном добавлении и сохранении клиента, возвращает его данные в виде класса Client. Иначе - null.
        /// </returns>
        public static Client Registration(Client newClient, DoO_CRMContext context)
        {
            if (context.Clients.FirstOrDefault(clnt => clnt.Name == newClient.Name) == default)
            {
                context.Clients.Add(newClient);
                context.SaveChanges();

                return newClient;
            }
            return null;
        }

        /// <summary>
        /// Метод для увеличения баланса входного пользователя.
        /// </summary>
        /// <param name="client"> Клиент в БД, на основе класса Client, которому будет пополнен баланс. </param>
        /// <param name="money"> Сума пополнения. </param>
        /// <param name="context"> Класс контекста для подключения к БД. </param>
        /// <returns> Возвращает обновлённый баланс пользователя из БД на основе входного класса Client.
        ///           Если операция не была успешно совершена, возвращает -1 (как константу неверного результата).
        /// </returns>
        public static decimal UpBalance(Client client, decimal money, DoO_CRMContext context)
        {
            if (money > 0 && client != null && context != null)
            {
                Client clientFromDB = context.Clients.FirstOrDefault(clnt => clnt.Name == client.Name);

                if (clientFromDB != default)
                {
                    clientFromDB.Balance += money;
                    context.SaveChanges();

                    return clientFromDB.Balance;
                }
            }
            return -1;
        }

        /// <summary>
        /// Отправляет экземпляр класса Order, заполняемый на основе входных аргументов на сервер и ожидает от него ответа.
        /// </summary>
        /// <param name="client"> Экземпляр класса Client, для заполнения класса Order. </param>
        /// <param name="cart"> Экземпляра класса Cart, для заполнения класса Order. </param>
        /// <returns>
        /// Если данные были успешно отправлены, возвращает ответ от сервера в виде значения bool. 
        /// Иначе - возвращает false.
        /// </returns>
        public static bool SendOrder(Client client, Cart cart)
        {
            if (client != null && cart != null)
            {
                var tcpClient = new TcpClient();
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                bool answer = false;

                try
                {
                    tcpClient.Connect(IpAddress, Port);
                    using var stream = tcpClient.GetStream();

                    // Sending of Order
                    Order order = new Order(client, cart);
                    #region OptionWriter

                    //BinaryWriter writer = new BinaryWriter(stream);

                    //writer.Write(order.ClientId);
                    //writer.Write(order.Client.Id);
                    //writer.Write(order.Client.Name);
                    //writer.Write(order.Client.Balance);
                    //writer.Write(order.SumCost);
                    //foreach (var sell in cart.Sells)
                    //{
                    //    writer.Write(sell.ProductId);
                    //    writer.Write(sell.CountOfProduct);
                    //    writer.Write(sell.Product.Name);
                    //    writer.Write(sell.Product.Cost);
                    //    writer.Write(sell.Product.Count);
                    //}
                    //writer.Flush();

                    //Maybe it won't work.
                    #endregion

                    byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(order, options);
                    stream.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);


                    // Waiting an Answer
                    byte[] data = new byte[256];
                    var buildingAnswer = new StringBuilder();

                    do
                    {
                        int bytesOfData = stream.Read(data, 0, data.Length);
                        buildingAnswer.Append(Encoding.UTF8.GetString(data, 0, bytesOfData));
                    }
                    while (stream.DataAvailable);

                    answer = JsonSerializer.Deserialize<bool>(buildingAnswer.ToString());
                }
                catch (SocketException)
                {
                    Console.WriteLine("Произошла ошибка при работе с сервером, сервер не доступен!");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    tcpClient.Close();
                }

                return answer;
            }
            return false;
        }

        /// <summary>
        /// Сохраняет экземпляры Sell в БД, заполняя их на основе входных аргументов.
        /// </summary>
        /// <param name="cart"> Экземпляр класса Cart, из которого будут использованы экземпляры Sell. </param>
        /// <param name="context"> Класс контекста для подключения к БД. </param>
        /// <returns>
        /// При удачном сохранении всех экземпляров Sell, возвращает их в виде списка, с заполненными параметрами.
        /// </returns>
        public static List<Sell> ApplySells(Cart cart, DoO_CRMContext context)
        {
            if (cart != null && context != null)
            {
                List<Sell> savedSells = new List<Sell>();
                try
                {
                    foreach (var sell in cart.Sells)
                    {
                        int savedProductId = sell.ProductId;
                        int? savedClientId = sell.ClientId;

                        sell.ProductId = 0;
                        sell.ClientId = null;


                        context.Sells.Add(sell);
                        context.SaveChanges();

                        sell.ClientId = savedClientId;
                        context.SaveChanges();

                        sell.ProductId = savedProductId;
                        context.SaveChanges();

                        savedSells.Add(sell);
                    }

                    cart.Sells.Clear();
                    return savedSells;
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Произошла ошибка при сохранении данных о покупке:\n ");
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает суммарную стоимость всех экземпляров Sell в экземпляре Cart.
        /// </summary>
        /// <param name="cart"> Экземпляр класса Cart, сумма стоимости Sell-ов которого будет возвращена. </param>
        /// <returns></returns>
        public static decimal GetSumCostOfSells(Cart cart)
        {
            return cart.Sells.Sum(prod => prod.Product.Cost);
        }

        #region Useful constants

        /// <summary>
        /// Константа IP адреса по умолчанию.
        /// </summary>
        public const string IpAddress = "127.0.0.1";

        /// <summary>
        /// Константа порта по умолчанию.
        /// </summary>
        public const int Port = 8080;

        #endregion
    }
}
