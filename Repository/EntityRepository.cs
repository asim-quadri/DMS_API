using Dapper;
using DmsApi.Models;
using DmsApi.Repository;
using System.Data.SqlClient;
using System.Data;

namespace Dms_Api.Repository
{
    public interface IEntityRepository
    {
        Task<IEnumerable<Entity>> GetAllEntities();
        //Task<Entity> GetEntityByIdAsync(int id);
        //Task<Entity> AddEntityAsync(Entity entity);
        //Task UpdateEntityAsync(Entity entity);
        //Task DeleteEntityAsync(int id);
    }

    public class EntityRepository : IEntityRepository
    {
        private readonly IWebHostEnvironment _environment;

        protected readonly IUnitOfWork _unitOfWork;

        public EntityRepository(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<IEnumerable<Entity>> GetAllEntities()
        {
            var query = "SELECT * FROM Entity";

            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<Entity>(query);
                return result.ToList();
            }
        }
            

        //public async Task<Entity> GetEntityByIdAsync(int id)
        //{
        //    using (var connection = CreateConnection())
        //    {
        //        var sql = "SELECT * FROM Entities WHERE Id = @Id";
        //        var entity = await connection.QueryFirstOrDefaultAsync<Entity>(sql, new { Id = id });
        //        return entity;
        //    }
        //}

        //public async Task<Entity> AddEntityAsync(Entity entity)
        //{
        //    using (var connection = CreateConnection())
        //    {
        //        var sql = @"
        //        INSERT INTO Entities (EntityName, OrganizationId, CountryId, StateId, City, Address, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
        //        VALUES (@EntityName, @OrganizationId, @CountryId, @StateId, @City, @Address, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy);
        //        SELECT CAST(SCOPE_IDENTITY() as int)";

        //        var id = await connection.QuerySingleAsync<int>(sql, entity);
        //        entity.Id = id;
        //        return entity;
        //    }
        //}

        //public async Task UpdateEntityAsync(Entity entity)
        //{
        //    using (var connection = CreateConnection())
        //    {
        //        var sql = @"
        //        UPDATE Entities 
        //        SET EntityName = @EntityName, OrganizationId = @OrganizationId, CountryId = @CountryId, 
        //            StateId = @StateId, City = @City, Address = @Address, 
        //            CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, 
        //            ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy
        //        WHERE Id = @Id";

        //        await connection.ExecuteAsync(sql, entity);
        //    }
        //}

        //public async Task DeleteEntityAsync(int id)
//        {
//            using (var connection = CreateConnection())
//            {
//                var sql = "DELETE FROM Entities WHERE Id = @Id";
//                await connection.ExecuteAsync(sql, new { Id = id });
//            }
//        }
//    }


}

}
