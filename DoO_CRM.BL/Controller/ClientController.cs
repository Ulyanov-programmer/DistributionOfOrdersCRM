using DoO_CRM.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public static Client Registration(string nameOfClient, decimal balanceOfClient, DoO_CRMContext context)
        {
            Client newClient = new Client(nameOfClient, balanceOfClient);

            if (context.Clients.FirstOrDefault(clnt => clnt.Name == newClient.Name) == default)
            {
                context.Clients.Add(newClient);
                context.SaveChanges();
                return newClient;
            }
            else
            {
                return null;
            }
        }
        public static bool UpBalance(Client client, decimal money, DoO_CRMContext context)
        {
            if (money > 0)
            {
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

        public static decimal GetSumCostOfSells(Cart cart)
        {
            return cart.Sells.Sum(prod => prod.Product.Cost);
        }
    }
}
