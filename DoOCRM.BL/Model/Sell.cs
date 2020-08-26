using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoO_CRM.BL.Model
{
    class Sell
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Order Order { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
