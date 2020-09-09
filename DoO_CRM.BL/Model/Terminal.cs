using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DoO_CRM.BL.Model
{
    public class Terminal
    {
        public int TerminalId { get; set; }
        public Queue<Order> QueueOrders = new Queue<Order>();

        const int MaxLenghtOfQueue = 30;
        public int ActualLenghtOfQueue { get; private set; }


        public bool Enqueue(Order order)
        {
            if (ActualLenghtOfQueue < MaxLenghtOfQueue && order != null)
            {
                QueueOrders.Enqueue(order);
                ActualLenghtOfQueue++;
                return true;
            }
            return false;
        }

        public string Dequeue(bool confirmed, int terminalId, DoO_CRMContext context)
        {
            if (QueueOrders.Count > 0)
            {
                try
                {
                    Order newOrder = QueueOrders.Peek(); //TODO: разбить на транзакции.
                    int? identityOfClient = newOrder.ClientId;
                    newOrder.ClientId = null;

                    context.Orders.Add(newOrder);
                    context.SaveChanges();

                    newOrder.ClientId = identityOfClient;
                    context.SaveChanges();

                    newOrder.DateBuy = DateTime.Now;
                    newOrder.IsBuy = confirmed;
                    newOrder.TerminalId = terminalId;
                    context.SaveChanges();


                    Client clientFromDB = context.Clients.Find(newOrder.ClientId);
                    clientFromDB.Balance -= newOrder.SumCost;
                    context.SaveChanges();

                    QueueOrders.Dequeue();

                    if (context.Orders.OrderByDescending(order => order.OrderId)
                                      .FirstOrDefault() != default)
                    {
                        return "Заказ был успешно сохранён.";
                    }
                    else
                    {
                        return "Заказ не был сохранён! Обратитесь за помощью к ближайшему айтишнику, или пните это устройство!";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");

                    QueueOrders.Dequeue();
                    Console.WriteLine("Заказ был удалён из очереди.");
                }
            }
            return "На данный момент заказов нет в очереди!";
        }

        private int WaitingOfOrder(TcpListener server, Terminal terminal)
        {
            while (true)
            {
                Console.WriteLine("Ожидание заказа...");
                var tcpClient = server.AcceptTcpClient();

                Console.WriteLine("Чтение запроса...");
                var stream = tcpClient.GetStream();

                byte[] data = new byte[512];
                int bytesOfData = stream.Read(data, 0, data.Length);
                StringBuilder orderData = new StringBuilder();
                do
                {
                    orderData.Append(Encoding.UTF8.GetString(data, 0, bytesOfData));
                }
                while (stream.DataAvailable);

                Order sendedOrder = JsonSerializer.Deserialize<Order>(orderData.ToString());
                terminal.Enqueue(sendedOrder);

                Console.WriteLine($"Чтение завершено.");
                return ActualLenghtOfQueue;
            }
        }

        public async Task<int> WaitingOfOrderAsync(TcpListener server, Terminal terminal)
        {
            var result = await Task.Run(() => WaitingOfOrder(server, terminal));
            return result;
        }
    }
}
