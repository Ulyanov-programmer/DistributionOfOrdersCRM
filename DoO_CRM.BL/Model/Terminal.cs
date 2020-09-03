using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoO_CRM.BL.Model
{
    public class Terminal
    {
        public int TerminalId { get; set; }
        public Queue<Order> QueueOrders = new Queue<Order>();

        public const int MaxLenghtOfQueue = 30;
        public int ActualLenghtOfQueue { get; set; }


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
        public Order Dequeue(bool confirmed, int terminalId, DoO_CRMContext context)
        {
            if (QueueOrders.Count > 0
            &&  QueueOrders.Peek().Client.Balance >= QueueOrders.Peek().SumCost)
            {
                try
                {
                    Order newOrder = QueueOrders.Peek();
                    newOrder.DateBuy = DateTime.Now;
                    newOrder.IsBuy = confirmed;
                    newOrder.TerminalId = terminalId;

                    context.Orders.Add(newOrder);

                    context.Clients.Find(newOrder.ClientId).Balance -= newOrder.SumCost;

                    context.SaveChanges();

                    QueueOrders.Dequeue();
                    Order lastOrder = context.Orders.OrderByDescending(order => order.Id).First();
                    return lastOrder;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");

                    QueueOrders.Dequeue();
                    Console.WriteLine("Заказ был удалён из очереди.");
                }
            }
            return null;
        }
    }
}
