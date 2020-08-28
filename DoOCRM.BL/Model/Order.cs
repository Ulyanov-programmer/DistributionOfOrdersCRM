using System;
using System.Collections.Generic;

namespace DoO_CRM.BL.Model
{
    public class Order
    {
        public long Id { get; set; }
        public Guid Number { get; set; }
        public byte CassId { get; set; }
        public decimal SumCost { get; set; }

        public int? ClientId { get; set; }
        public Client Client { get; set; }

        public DateTime DateBuy { get; set; }

        public ICollection<Sell> Sells { get; set; }

        public override string ToString()
        {
            return $"{Number}, дата создания: {DateBuy:dd.MM.yyyy hh.mm.ss}";
        }
        //TODO: Разделить на слои Логики и данных БД.
    }
}
