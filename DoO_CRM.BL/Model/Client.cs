using DoO_CRM.BL.Model;
using System.Collections.Generic;

namespace DoO_CRM.BL
{
    public class Client
    {
        public Client(string name, decimal optionalBalance = 0)
        {
            Name = name;
            Balance = optionalBalance;
        }
        public Client() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public List<Order> Orders { get; set; }

        #region overrides

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Client otherClient)
        {
            if (Name == otherClient.Name &&
                Balance == otherClient.Balance)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
