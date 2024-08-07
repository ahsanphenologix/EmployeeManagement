using EmployeeManagement.API.AppDbContext;
using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repository
{
    public class CustomerImageRepository : IRepository<CustomerImageDbModel>
    {

        private readonly CustomerDbContext _customerDbContext;

        public CustomerImageRepository(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<int> Delete(int id)
        {
            if (id == 0)
                throw new InvalidOperationException($"No id is provided for type {nameof(CustomerRepository)} in delete operation");

            var cust = _customerDbContext.Customers.Single(c => c.Id == id);

            //remove the staff from the database
            _customerDbContext.Customers.Remove(cust);

            var res = await _customerDbContext.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<CustomerImageDbModel>> GetAll()
        {
            var customers = await _customerDbContext.Images.ToListAsync();


            if (!customers.Any())
                return new List<CustomerImageDbModel>();
            //throw new InvalidOperationException($"No id is provided for type {nameof(CustomerRepository)} in delete operation");


            return customers;
        }

        public async Task<CustomerImageDbModel> GetById(int id)
        {
            if (id == 0)
                throw new InvalidOperationException($"No id is provided for type {nameof(CustomerRepository)} in GetById operation");

            var cust = _customerDbContext.Images.Single(c => c.Id == id);

            if (cust == null)
                return null;

            return cust;
        }

        public async Task<int> Insert(CustomerImageDbModel entity)
        {
            if (entity == null)
                return 0;

            _customerDbContext.Images.Add(entity);

            var result = _customerDbContext.SaveChanges();

            return entity.Id;

        }

        public async Task<int> Update(int id, CustomerImageDbModel entity)
        {
            if (entity == null)
                throw new InvalidOperationException($"No entity is provided for type {nameof(CustomerRepository)} in Update operation");


            var oldCust = _customerDbContext.Customers.Single(c => c.Id == id);

            if (oldCust == null)
                return 0;

            _customerDbContext.Entry(oldCust).CurrentValues.SetValues(entity);

            var result = _customerDbContext.SaveChanges();

            //var result = _customerDbContext.Update(entity);

            return result;


        }
    }
}
