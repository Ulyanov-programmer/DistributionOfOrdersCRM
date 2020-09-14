using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;

namespace DoO_CRM.BL.Model
{
    public class Order
    {
        public Order(Client client, Cart cartWithSells)
        {
            Number = Guid.NewGuid();
            SumCost = ClientController.GetSumCostOfSells(cartWithSells);
            ClientId = client.Id;
        }

        public Order(int cassId,
                     Guid number,
                     DateTime dateBuy,
                     decimal sumCost,
                     bool tobeBuy,
                     Client client)
        {
            Number = number;
            TerminalId = cassId;
            DateBuy = dateBuy;
            SumCost = sumCost;
            IsBuy = tobeBuy;
            ClientId = client.Id;
        }
        public Order() { }


        public int OrderId { get; set; }
        public Guid Number { get; set; }
        public int TerminalId { get; set; }
        

        public DateTime DateBuy { get; set; }
        public decimal SumCost { get; set; }
        public bool IsBuy { get; set; }

        #region References

        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public List<Sell> Sells { get; set; }

        #endregion

        public override string ToString()
        {
            return $"Заказ {Number}, дата подтверждения: {DateBuy:dd.MM.yyyy hh.mm.ss}";
        }
        public bool Equals(Order otherOrder)
        {
            if (otherOrder.IsBuy != IsBuy ||
                otherOrder.Number != Number ||
                otherOrder.SumCost != SumCost ||
                otherOrder.TerminalId != TerminalId
                )
            {
                return false;
            }
            return true;
        }
        //TODO: Разделить на слои Логики и данных БД.
    }
}
