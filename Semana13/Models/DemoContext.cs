using Microsoft.EntityFrameworkCore;

namespace Semana13.Models
{
    public class DemoContext:DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=JOHAN\\SQLEXPRESS; " + "Initial Catalog=DemoDB; Integrated Security=True; trustservercertificate=True");
        }
    }
}
