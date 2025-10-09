using Dms_Api.Repository;
using DmsApi.Models;

namespace Dms_Api.Services
{
    public interface IEntityService
    {
        Task<IEnumerable<Entity>> GetAllEntities();
        //Task<Entity> GetEntityByIdAsync(int id);
        //Task<Entity> AddEntityAsync(Entity entity);
        //Task UpdateEntityAsync(Entity entity);
        //Task DeleteEntityAsync(int id);
    }

    public class EntityService : IEntityService
    {
        private readonly IEntityRepository _entityRepository;

        public EntityService(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<IEnumerable<Entity>> GetAllEntities()
        {
           return await _entityRepository.GetAllEntities();
        }

        //public async Task<Entity> GetEntityByIdAsync(int id)
        //{
        //    return await _entityRepository.GetEntityByIdAsync(id);
        //}

        //public async Task<Entity> AddEntityAsync(Entity entity)
        //{
        //    return await _entityRepository.AddEntityAsync(entity);
        //}

        //public async Task UpdateEntityAsync(Entity entity)
        //{
        //    await _entityRepository.UpdateEntityAsync(entity);
        //}

        //public async Task DeleteEntityAsync(int id)
        //{
        //    await _entityRepository.DeleteEntityAsync(id);
        //}
    }

}
