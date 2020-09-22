using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DoO_CRM.BL.Model
{
    /// <summary>
    /// Виртуальная сущность терминала. Содержит свойства и методы для работы с экземплярами Order.
    /// </summary>
    public class Terminal
    {
        /// <summary>
        /// Создаёт новый экземпляр Terminal.
        /// </summary>
        /// <param name="terminalId"> Идентификатор терминала. </param>
        public Terminal(int terminalId)
        {
            TerminalId = terminalId;
        }

        #region params

        public int TerminalId { get; set; }

        /// <summary>
        /// Класс Queue (очередь), содержащий объекты Order.
        /// </summary>
        public Queue<Order> QueueOrders = new Queue<Order>();

        const int MaxLenghtOfQueue = 30;

        #endregion

        #region methods

        /// <summary>
        /// Пополняет свойство QueueOrders новым экземпляром Order для этого объекта Terminal.
        /// </summary>
        /// <param name="order"> Экземпляр Order. </param>
        /// <returns> True - если пополнение было совершено успешно, иначе - false. </returns>
        public bool Enqueue(Order order)
        {
            if (QueueOrders.Count < MaxLenghtOfQueue && order != null)
            {
                QueueOrders.Enqueue(order);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Удаляет экземпляр Order из свойства QueueOrders этого объекта Terminal.
        /// </summary>
        /// <param name="confirmed"> Объект Order в свойстве IsBuy получает это значение. </param>
        /// <param name="terminalId"> Идентификатор терминала. </param>
        /// <param name="context"> Объект контекста, необходимый для подключения к БД. </param>
        /// <returns> Возвращает строку с описанием результата операции. </returns>
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
                    if (confirmed)
                    {
                        clientFromDB.Balance -= newOrder.SumCost;
                        context.SaveChanges();
                    }

                    clientFromDB.Orders.Add(newOrder);
                    context.SaveChanges();

                    transaction.Commit();

                    QueueOrders.Dequeue();

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

        /// <summary>
        /// Выставляет в БД экземплярам Sell входные значения.
        /// </summary>
        /// <param name="clientId"> Экземпляры Sell, которым нужно установить значения, определяется по этому идентификатору. </param>
        /// <param name="order"> Объект Order, к которому будут присвоены экземпляры Sell, </param>
        /// <param name="context"> Объект контекста, необходимый для подключения к БД. </param>
        /// <returns> Если значения были присвоены - возвращает True. Иначе - false. </returns>
        private bool SetValuesOrderInSells(int clientId, Order order, DoO_CRMContext context)
        {
            if (clientId >= 0 &&
                order != null &&
                order.OrderId >= 0 &&
                context != null)
            {
                List<Sell> sells = context.Sells.Where(sell =>
                                                       sell.ClientId == clientId &&
                                                       sell.Order != null)
                                                .ToList();

                foreach (var sell in sells)
                {
                    sell.Order = order;
                }
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Метод ожидания экземпляра Order. При успешном чтении сохраняет его в свойство QueueOrders этого объекта Terminal.
        /// </summary>
        /// <param name="server"> Объект сервера. </param>
        /// <param name="terminal"> Объект терминала, используемый для сохранения получаемых экземпляров Order. </param>
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
                }

                ProductController.SendAnswer(stream, orderIsntInTheQueue);
            }
        }

        /// <summary>
        /// Асинхронный метод ожидания экземпляра Order.
        /// При успешном чтении сохраняет его в свойство QueueOrders этого объекта Terminal.
        /// </summary>
        /// <param name="server"> Объект сервера. </param>
        /// <param name="terminal"> Объект терминала, используемый для сохранения получаемых экземпляров Order. </param>
        /// <returns></returns>
        public async Task WaitingOfOrderAsync(TcpListener server, Terminal terminal)
        {
            await Task.Run(() => WaitingOfOrder(server, terminal));
        }

        #endregion
    }
}
