using EmployeeManagement.Models;
using EmployeeManagement.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.AppDbContext
{
    public class EmployeeMDbContext : DbContext
    {
        public EmployeeMDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<CustomerViewModel> Customers { get; set; }
    }
}
