using DoO_CRM.BL.Model;
using System;
using System.Linq;
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
        public static Client Registration(string nameOfClient, decimal balanceOfClient, DoO_CRMContext context)
        {
            Client newClient = new Client(nameOfClient, balanceOfClient);

            if (context.Clients.FirstOrDefault(clnt => clnt.Name == newClient.Name) == default)
            {
                context.Clients.Add(newClient);
                context.SaveChanges();
                return newClient;
            }
            else
            {
                return null;
            }
        }
        public static bool UpBalance(Client client, decimal money, DoO_CRMContext context)
        {
            if (money > 0)
            {
                Client clientFromDB = context.Clients.FirstOrDefault(clnt => clnt.Name == client.Name);

                if (clientFromDB != default)
                {
                    clientFromDB.Balance += money;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public static bool SendOrder(Client client, Cart cart) //TODO: Можно переделать на возврат сообщения о результате.
        {
            var tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect("127.0.0.1", 8080);
                using (var stream = tcpClient.GetStream())
                {
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

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(order, options);
                    stream.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);
                }
                return true;
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine("Произошла ошибка при подключении к удалённому серверу: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public static decimal GetSumCostOfSells(Cart cart)
        {
            return cart.Sells.Sum(prod => prod.Product.Cost); //TODO: Не работает.
        }
    }
}
