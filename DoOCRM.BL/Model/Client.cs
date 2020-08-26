using DoO_CRM.BL.Model;
using System.Collections.Generic;

namespace DoO_CRM.BL
{
    class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return Name; 
        }
    }
}
