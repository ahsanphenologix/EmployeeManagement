using EmployeeManagement.API.AppDbContext;
using EmployeeManagement.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Repository
{
    public class Repository<T> //: IRepository<T> where T : class
    {

        private readonly CustomerDbContext _customerDbContext;

        public Repository(CustomerDbContext customerDbContext) 
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {
            // Get the property from the DbContext that matches the type name
            var property = _customerDbContext.GetType().GetProperties()
                .SingleOrDefault(prop => prop.PropertyType == typeof(DbSet<T>));

            if (property == null)
            {
                throw new InvalidOperationException($"No DbSet<T> property found for type {typeof(T).Name}.");
            }

            // Get the value of the property, which should be a DbSet<T>
            var propertyValue = property.GetValue(_customerDbContext) as DbSet<T>;

            if (propertyValue == null)
            {
                throw new InvalidOperationException($"The property value is not of type DbSet<{typeof(T).Name}.");
            }

            // Fetch and return all entities from the DbSet<T>
            return await propertyValue.ToListAsync();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        //public async Task<string> GetById(int id) //where T : class
        //{
        //    // Get the property from the DbContext that matches the type name
        //    var property = _customerDbContext.GetType().GetProperties()
        //        .SingleOrDefault(prop => prop.PropertyType == typeof(DbSet<T>));

        //    if (property == null)
        //    {
        //        throw new InvalidOperationException($"No DbSet<T> property found for type {typeof(T).Name}.");
        //    }

        //    // Get the value of the property, which should be a DbSet<T>
        //    var propertyValue = property.GetValue(_customerDbContext) as DbSet<T>;

        //    if (propertyValue == null)
        //    {
        //        throw new InvalidOperationException($"The property value is not of type DbSet<{typeof(T).Name}.");
        //    }

        //    // Fetch and return all entities from the DbSet<T>
        //    //return await propertyValue.SingleAsync( cust => cust.Id == id);

        //    return string.Empty;
                
        //}

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
