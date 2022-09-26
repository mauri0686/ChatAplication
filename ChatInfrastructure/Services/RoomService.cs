

using ChatDomain.Models;

namespace ChatInfrastruncture.Service
{
    public class RoomService 
    {
        #region Property
        private readonly IRepository<Room?> _repository;
     
        #endregion

        #region Constructor
        public RoomService(IRepository<Room> repository)
        {
            _repository = repository;
         
        }
        #endregion     

        public async Task<Room?> Get(int id)
            {
            return await _repository.Get(id);
        }
        public async Task<List<Room?>> Get()
        {
            return await _repository.GetAll();
        }
        
        public async Task Add(Room? room)
        {
              await _repository.Insert(room);
            
        }
        public async Task Delete(Room? room)
        {
            await _repository.Remove(room);
            
          }

        public async Task SaveAsync()
        {
            await _repository.SaveChanges();
        }
    }
}
