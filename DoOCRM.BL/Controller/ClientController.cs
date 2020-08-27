using DoO_CRM.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoO_CRM.BL.Controller
{
    public static class ClientController
    {
        public static Client GetRegistered(string nameOfClient, DoO_CRMContext context)
        {
            Client clientFromDB = context.Clients.FirstOrDefault(client => client.Name == nameOfClient);

            if (clientFromDB != default)
            {
                return clientFromDB;
            }
            return null;
        }
        public static Client Registration(string nameOfClient, DoO_CRMContext context)
        {
            Client newClient = new Client(nameOfClient, 0);

            context.Clients.Add(newClient);
            context.SaveChanges();

            return newClient;
        }
        public static bool UpBalance(Client client, decimal money, DoO_CRMContext context)
        {
            if (money > 0)
            {
                client.Balance += money;
                Client clientFromDB = context.Clients.FirstOrDefault(clnt => clnt.Name == client.Name);
                if (clientFromDB != default)
                {
                    clientFromDB.Balance += money;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public static bool SendOrder()
        {
            Order order = new Order()
        }
    }
}
