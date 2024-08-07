using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.Models.DatabaseModels;

namespace EmployeeManagement.API.AppDbContext
{
    public class CustomerDbContext : DbContext
    { 

        public CustomerDbContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<CustomerDbModel> Customers { get; set; }
        public DbSet<CustomerImageDbModel> Images { get; set; }
    }
}
