
using ChatDomain.Models;
using ChatInfrastruncture.Data;
using Microsoft.EntityFrameworkCore;


namespace ChatInfrastruncture
{
   public class Repository<T> : IRepository<T> where T: BaseEntity
    {
       
        private readonly ChatAppDbContext _chatAppDbContext;
        protected readonly DbSet<T> entities;
  

        public Repository(ChatAppDbContext chatAppDbContext)
        {
            _chatAppDbContext = chatAppDbContext;
            entities = _chatAppDbContext.Set<T>();
          
        }
        public async Task Delete(T entity)
        {
            
            entities.Remove(entity);
            await _chatAppDbContext.SaveChangesAsync();
        }

        public async Task<T?> Get(int Id)
        {
            return await entities.SingleOrDefaultAsync(c => c.id == Id);
        }

        public Task<T?> Get(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAll()
        {
            return  await entities.ToListAsync();
        }
        public  IQueryable<T> GetQueryable()
        {
            return entities;
        }
        public async Task Insert(T entity)
        {

             entities.Add(entity);
            await _chatAppDbContext.SaveChangesAsync();
        }

        public async Task Remove(T entity)
        {            
            entities.Remove(entity);
            await _chatAppDbContext.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _chatAppDbContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            entities.Update(entity);
            await _chatAppDbContext.SaveChangesAsync();
        }

    }
}
