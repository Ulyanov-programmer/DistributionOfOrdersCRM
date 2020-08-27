using System;
using System.Collections.Generic;

namespace DoO_CRM.BL
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
