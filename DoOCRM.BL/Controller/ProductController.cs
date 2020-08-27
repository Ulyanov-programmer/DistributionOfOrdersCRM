using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoO_CRM.BL.Controller
{
    public static class ProductController
    {
        public static List<Product> GetTop100Products(bool ascOrDesc, DoO_CRMContext context)
        {
            List<Product> productsFromDB = new List<Product>();

            productsFromDB.AddRange(context.Products
                          .Where(prod => prod.Id < 101));

            if (ascOrDesc == true) //А почему бы и не добавить эту фичу?
            {
                productsFromDB.OrderByDescending(prod => prod.Id);
            }

            return productsFromDB;
        }
    }
}
