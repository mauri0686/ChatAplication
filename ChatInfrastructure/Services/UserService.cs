

using ChatDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatInfrastruncture.Service
{
    public class UserService 
    {
        #region Property
        private readonly IRepository<User?> _repository;
     
        #endregion

        #region Constructor
        public UserService(IRepository<User?> repository)
        {
            _repository = repository;
         
        }
        #endregion     
        public async Task<List<User?>> Get()
        {
            return await _repository.GetAll();
        }
        public async Task<User> GetByEmail( string email)
        {
            return await _repository.GetQueryable().FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<User?> Get(string id)
        {
            return await _repository.Get(id);
        }

        public Task<User> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Add(User? user)
        {
              await _repository.Insert(user);
           
        }
        public async Task Update(User? user)
        {
              await _repository.Update(user);
            
        }

        public async Task Delete(User? user)
        {
            await _repository.Remove(user);
            
          }

        public async Task SaveAsync()
        {
            await _repository.SaveChanges();
        }
    }
}
