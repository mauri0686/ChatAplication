

using ChatDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatInfrastruncture.Service
{


    public class MessageService 
    {
        #region Property
        private IRepository<Message> _repository;
     
        #endregion

        #region Constructor
        public MessageService(IRepository<Message> repository)
        {
            _repository = repository;
         
        }
        #endregion     

        public async Task<Message> Get(int id)
        {
            return await _repository.Get(id);
        }
        public  IQueryable<Message> GetRoomMessageLimit(int roomId, int limit )
        {
            return _repository.GetQueryable().Include(m=> m.user).Where(x=> x.roomId == roomId).OrderBy(x=> x.createdAt).Take(limit);
        }
        
        public async Task Add(Message message)
        {
            await _repository.Insert(message);
           
        }
        public async Task Delete(Message message)
        {
            await _repository.Remove(message);
            await _repository.SaveChanges();
          }
        public async Task SaveAsync()
        {
            await _repository.SaveChanges();
        }
    }
}
