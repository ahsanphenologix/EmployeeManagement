namespace EmployeeManagement.API.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int id);

        Task<int> Insert(T entity);

        Task<int> Update(int id,T entity);

        Task<int> Delete(int id);
    }
}
