using DoO_CRM.BL.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace DoO_CRM.BL.Controller
{
    public class DoO_CRMContext : DbContext
    {
        public DoO_CRMContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-N9EORNQ;Database=DoO_CRM;Trusted_Connection=True;");
        }

        internal DbSet<Order> Orders { get; set; }
        internal DbSet<Sell> Sells { get; set; }
        internal DbSet<Client> Clients { get; set; }
        internal DbSet<Product> Products { get; set; }
    }
}
