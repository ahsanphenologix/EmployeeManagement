using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.Models.DatabaseModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EmployeeManagement.API.AppDbContext
{
    public class CustomerDbContext : IdentityDbContext<ApplicationUser>
    { 

        public CustomerDbContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<CustomerDbModel> Customers { get; set; }
        public DbSet<CustomerImageDbModel> Images { get; set; }
    }
}
