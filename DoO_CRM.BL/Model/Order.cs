using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;

namespace DoO_CRM.BL.Model
{
    /// <summary>
    /// Класс-контейнер для сохранения данных в виде заказа.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Создаёт новый экземпляр Order на основе входных аргументов.
        /// </summary>
        /// <param name="client"> Экземпляр Client для сохранения информации о нём и присваивания этого заказа к нему. </param>
        /// <param name="cartWithSells"> Объект Cart, необходимый для подсчёта суммарной стоимости заказа. </param>
        public Order(Client client, Cart cartWithSells)
        {
            Number = Guid.NewGuid();
            SumCost = ClientController.GetSumCostOfSells(cartWithSells);
            ClientId = client.Id;
        }

        /// <summary>
        /// Создаёт новый экземпляр Order на основе входных аргументов.
        /// Данный конструктор создавался только для тестов!
        /// </summary>
        /// <param name="terminal"> Объект Terminal, данные о котором сохранятся в этом объекте Order. </param>
        /// <param name="number"> Значение Guid, использующееся как идентификатор объекта Order. </param>
        /// <param name="dateBuy"> Дата подтверждения заказа оператором и сохранения его в БД. </param>
        /// <param name="sumCost"> Суммарная стоимость этого объекта Order. </param>
        /// <param name="tobeBuy"> Значение, говорящее о том, подтверждён к покупке этот заказ или нет. </param>
        /// <param name="client"> Экземпляр Client для сохранения информации о нём и присваивания этого заказа к нему. </param>
        public Order(Terminal terminal,
                     Guid number,
                     DateTime dateBuy,
                     decimal sumCost,
                     bool tobeBuy,
                     Client client)
        {
            Number = number;
            TerminalId = terminal.TerminalId;
            DateBuy = dateBuy;
            SumCost = sumCost;
            IsBuy = tobeBuy;
            ClientId = client.Id;
        }
        public Order() { }

        #region params

        public int OrderId { get; set; }

        /// <summary>
        /// Значение Guid, использующееся как идентификатор объекта Order.
        /// </summary>
        public Guid Number { get; set; }
        public int TerminalId { get; set; }

        /// <summary>
        /// Дата подтверждения этого заказа оператором и сохранения его в БД.
        /// </summary>
        public DateTime DateBuy { get; set; }

        /// <summary>
        /// Суммарная стоимость этого объекта Order, подсчитываемая на основе привязанного к нему объекта Cart.
        /// </summary>
        public decimal SumCost { get; set; }

        /// <summary>
        /// Значение, говорящее о том, был ли подтверждён к покупке этот заказ или нет.
        /// </summary>
        public bool IsBuy { get; set; }

        #endregion

        #region References

        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public List<Sell> Sells { get; set; }

        #endregion

        #region overrides

        /// <summary>
        /// Возвращает данные об этом экземпляре Order в качестве интерполированной строки.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Заказ {Number}, дата подтверждения: {DateBuy:dd.MM.yyyy hh.mm.ss}";
        }

        /// <summary>
        /// Сравнивает этот объект Order с другим.
        /// </summary>
        /// <param name="otherOrder"> Другой объект Order. </param>
        /// <returns> True - если сравниваемые объекты равны по всем сравниваемым параметрам, иначе - false. </returns>
        public bool Equals(Order otherOrder)
        {
            if (otherOrder.IsBuy == IsBuy ||
                otherOrder.Number == Number ||
                otherOrder.SumCost == SumCost ||
                otherOrder.TerminalId == TerminalId)
            {
                return true;
            }
            return false;
        }

        #endregion


        //TODO: Разделить на слои Логики и данных БД.
    }
}
