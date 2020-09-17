using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DoO_CRM.BL.Model
{
    public class Terminal
    {
        public Terminal(int terminalId)
        {
            TerminalId = terminalId;
        }

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
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    Order newOrder = QueueOrders.Peek();
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

                    clientFromDB.Orders.Add(newOrder);
                    context.SaveChanges();

                    transaction.Commit();

                    QueueOrders.Dequeue();
                    ActualLenghtOfQueue--;

                    if (context.Orders.Any(order => order.Number == newOrder.Number))
                    {
                        SetValuesOrderInSells(clientFromDB.Id, newOrder, context);

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
                    transaction.Rollback();

                    Console.WriteLine("Заказ был удалён из очереди, изменения не были сохранены.");
                }
            }
            return "На данный момент заказов нет в очереди!";
        }

        private bool SetValuesOrderInSells(int clientId, Order order, DoO_CRMContext context)
        {
            List<Sell> sells = context.Sells.Where(sell => sell.ClientId == clientId
                                                                         && sell.Order != null)
                                            .ToList();

            foreach (var sell in sells)
            {
                sell.Order = order;
            }
            context.SaveChanges();
            return true;
        }

        private void WaitingOfOrder(TcpListener server, Terminal terminal)
        {
            while (true)
            {
                var tcpClient = server.AcceptTcpClient();

                Console.WriteLine("Чтение запроса...");
                using var stream = tcpClient.GetStream();

                // Receiving an Order
                Order sendedOrder = ProductController.ReceivingAnOrder(stream);
                Console.WriteLine("Чтение завершено.");

                // Saving the order and sending the answer
                bool orderIsntInTheQueue = terminal.QueueOrders.All(order => order.ClientId != sendedOrder.ClientId);

                if (orderIsntInTheQueue)
                {
                    QueueOrders.Enqueue(sendedOrder);
                    ActualLenghtOfQueue++;
                }

                ProductController.SendAnswer(stream, orderIsntInTheQueue);
            }
        }

        public async Task WaitingOfOrderAsync(TcpListener server, Terminal terminal)
        {
            await Task.Run(() => WaitingOfOrder(server, terminal));
        }
    }
}
