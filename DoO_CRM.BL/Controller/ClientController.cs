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
    public static class ClientController
    {
        public static Client GetRegistered(string nameOfClient, DoO_CRMContext context)
        {
            Client clientFromDB = context.Clients.FirstOrDefault(client => client.Name == nameOfClient);

            if (clientFromDB != default)
            {
                return clientFromDB;
            }
            return null;
        }
        public static Client Registration(string nameOfClient, DoO_CRMContext context, decimal optionalBalanceOfClient = 0)
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

        public static decimal UpBalance(Client client, decimal money, DoO_CRMContext context)
        {
            if (money > 0)
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
        public static bool SendOrder(Client client, Cart cart) 
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

        public static List<Sell> ApplySells(Cart cart, DoO_CRMContext context)
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
                Console.WriteLine("Произошла ошибка при сохранении данных о покупке, с.м ошибку:");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static decimal GetSumCostOfSells(Cart cart)
        {
            return cart.Sells.Sum(prod => prod.Product.Cost);
        }


        public const string IpAddress = "127.0.0.1";
        public const int Port = 8080;
    }
}
