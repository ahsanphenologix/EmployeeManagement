using EmployeeManagement.API.AppDbContext;
using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace EmployeeManagement.API.Repository
{
    public class CustomerRepository : IRepository<CustomerDbModel>
    {
        private readonly CustomerDbContext _customerDbContext;


        public CustomerRepository(CustomerDbContext customerDbContext) 
        { 
            _customerDbContext = customerDbContext;
        }

        public async Task<int> Delete(int id)
        {
            if (id == 0)
                throw new InvalidOperationException($"No id is provided for type {nameof(CustomerRepository)} in delete operation");

            var cust = _customerDbContext.Customers.Single(c => c.Id == id);
            
            var image = _customerDbContext.Images.
                Where(img => img.Id == cust.ImageId).FirstOrDefault();
                
            //Single(img => img.Id == cust.ImageId)
            //remove the staff from the database
            
            if(image != null)
            {
                _customerDbContext.Images.Remove(image);
                await _customerDbContext.SaveChangesAsync();

            }

            
            _customerDbContext.Customers.Remove(cust);
            
            var res = await _customerDbContext.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<CustomerDbModel>> GetAll()
        {
            var customers = await _customerDbContext.Customers.ToListAsync();
            
            
            if(!customers.Any())
                return new List<CustomerDbModel>();
                //throw new InvalidOperationException($"No id is provided for type {nameof(CustomerRepository)} in delete operation");


            return customers;
        }

        public async Task<CustomerDbModel> GetById(int id)
        {
            if (id == 0)
                throw new InvalidOperationException($"No id is provided for type {nameof(CustomerRepository)} in GetById operation");

            var cust = _customerDbContext.Customers.Single(c => c.Id == id);

            if(cust == null)
                return null;

            return cust;
        }

        public async Task<int> Insert(CustomerDbModel entity)
        {
            if (entity == null)
                return 0;

            _customerDbContext.Customers.Add(entity);

            var result = _customerDbContext.SaveChanges();

            return result;

        }

        public async Task<int> Update(int id, CustomerDbModel entity)
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
    
        public async Task<int> SaveImageInDb(CustomerImageDbModel image)
        {
            if (!(string.IsNullOrEmpty(image.Name) && string.IsNullOrEmpty(image.Path)))
                return 0;


            _customerDbContext.Images.Add(image);
            
            await _customerDbContext.SaveChangesAsync();

            return image.Id;
        }
    }
}
