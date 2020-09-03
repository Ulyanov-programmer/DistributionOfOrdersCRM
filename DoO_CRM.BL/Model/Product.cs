using System;
using System.Collections.Generic;

namespace DoO_CRM.BL
{
    public class Product
    {
        public Product(string name, decimal cost, int count)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Cost = cost;
            Count = count;
        }
        public Product() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }


        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            if (obj is Product product)
            {
                return Name.Equals(product.Name);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
