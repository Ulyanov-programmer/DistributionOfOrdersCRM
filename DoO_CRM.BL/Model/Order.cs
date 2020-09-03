using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoO_CRM.BL.Model
{
    public class Order
    {
        public Order(decimal sumCost, Client client, Cart cartWithSells)
        {
            Number = Guid.NewGuid();
            SumCost = ClientController.GetSumCostOfSells(cartWithSells); //TODO: не работает метод.
            ClientId = client.Id;
            Client = client;

            Sells = new List<Sell>();
            foreach (var sell in cartWithSells.Sells)
            {
                sell.Order = this;
                Sells.Add(sell);
            }
        }

        public Order(int cassId,
                     Guid number,
                     DateTime dateBuy,
                     decimal sumCost,
                     bool tobeBuy,
                     Client client,
                     Cart cartWithSells)
        {
            Number = number;
            TerminalId = cassId;
            DateBuy = dateBuy;
            SumCost = sumCost;
            IsBuy = tobeBuy;
            ClientId = client.Id;
            Client = client;

            Sells = new List<Sell>();
            foreach (var sell in cartWithSells.Sells)
            {
                sell.Order = this;
                Sells.Add(sell);
            }
        }
        public Order() { }


        public long Id { get; set; }
        public Guid Number { get; set; }
        public int TerminalId { get; set; }
        

        public DateTime DateBuy { get; set; }
        public decimal SumCost { get; set; }
        public bool IsBuy { get; set; }

        #region References

        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public ICollection<Sell> Sells { get; set; }

        #endregion

        public override string ToString()
        {
            return $"Заказ {Number}, дата подтверждения: {DateBuy:dd.MM.yyyy hh.mm.ss}";
        }
        public bool Equals(Order otherOrder)
        {
            bool equal = true;
            if (otherOrder.ClientId != ClientId ||
                otherOrder.IsBuy != IsBuy ||
                otherOrder.Number != Number ||
                otherOrder.SumCost != SumCost ||
                otherOrder.TerminalId != TerminalId ||
                otherOrder.Sells.SequenceEqual(Sells) == false
                )
            {
                return false;
            }
            return true;
        }
        //TODO: Разделить на слои Логики и данных БД.
    }
}
