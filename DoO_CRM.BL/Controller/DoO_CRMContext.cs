using DoO_CRM.BL.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace DoO_CRM.BL.Controller
{
    public class DoO_CRMContext : DbContext
    {
        public DoO_CRMContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-N9EORNQ;Database=DoO_CRM;Trusted_Connection=True;");
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Sell> Sells { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
