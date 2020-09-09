using DoO_CRM.BL.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoO_CRM.BL
{
    public class Client
    {
        public Client(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
        }
        public Client() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public List<Order> Orders { get; set; }

        public override string ToString()
        {
            return Name; 
        }
    }
}
