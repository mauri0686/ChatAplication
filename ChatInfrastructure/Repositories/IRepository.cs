

using ChatDomain.Models;

namespace ChatInfrastruncture
{
   public interface IRepository<T>
   {
        Task<List<T>> GetAll();
        Task<T?> Get(int Id);
        Task<T?> Get(string Id);
         IQueryable<T> GetQueryable();
        Task Insert(T entity);
        Task Update(T entity);
        Task Remove(T entity);
        Task SaveChanges();
    }
}
