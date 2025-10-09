using DmsApi.Models;
using DmsApi.Helpers;
using Dapper;

namespace DmsApi.Repository
{
    public interface IRegulationGroupRepository
    {
        Task<List<RegulationGroupModel>> GetAllRegulationGroups();
        Task<RegulationGroupModel> GetRegulationGroupByUID(Guid uid);
        Task<RegulationGroupModel> PostRegulationGroup(RegulationGroupModel country);
        Task<bool> PostRegulationGroupApprove(AccessModel access, string ApprovalStatus);
        Task<List<RegulationGroupModel>> GetCountryRegulationGroupMapping();
        Task<RegulationGroupModel> PostCountryRegulationGroupMapping(RegulationGroupModel country);
        Task<List<RegulationGroupModel>> GetRegulationGroupApprovalList(Guid UserUID);
        Task<List<RegulationGroupModel>> GetCountryRegulationGroupMappingApproval(Guid UserUID);
        Task<bool> PostCountryRegulationGroupMappingApprove(AccessModel access, string ApprovalStatus);
    }
    public class RegulationGroupRepository : IRegulationGroupRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        public RegulationGroupRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RegulationGroupModel>> GetAllRegulationGroups()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[product_owner].USP_GETALLREGULATIONGROUP", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<RegulationGroupModel>().ToList();
                mul.Dispose();
                return result;
            }
        }
        public async Task<RegulationGroupModel> GetRegulationGroupByUID(Guid uid)
        {
            //using (var sqlContext = _unitOfWork.ConnectionFactory())
            //{
            //    var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
            //    var Result = mul.Read<RegulationGroupModel>().FirstOrDefault();
            //    mul.Dispose();
            //    return Result;
            //}
            return null;
        }

        public async Task<RegulationGroupModel> PostRegulationGroup(RegulationGroupModel regulation)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<RegulationGroupModel>("[product_owner].USP_POSTREGULATIONGROUP", new
                {
                    Id = regulation.Id,
                    RegulationGroupName = regulation.RegulationGroupName,
                    RegulationGroupCode = regulation.RegulationGroupCode,
                    CreatedBy = regulation.CreatedBy,
                    ModifiedBy = regulation.ModifiedBy,
                    UID = regulation.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }


        public async Task<bool> PostRegulationGroupApprove(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, RegulationGroupId = access.RegulationGroupId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEREGULATIONGROUPAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }

            return true;


        }

        public async Task<List<RegulationGroupModel>> GetCountryRegulationGroupMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYREGULATIONGROUPMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<RegulationGroupModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<RegulationGroupModel> PostCountryRegulationGroupMapping(RegulationGroupModel regulation)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<RegulationGroupModel>("[product_owner].USP_POSTCOUNTRYREGULATIONGROUPMAPPING", new
                {
                    Id = regulation.Id,
                    CountryId = regulation.CountryId,
                    RegulationGroupId = regulation.RegulationGroupId,
                    CreatedBy = regulation.CreatedBy,
                    UID = regulation.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }
        
        public async Task<bool> PostCountryRegulationGroupMappingApprove(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATECOUNTRYREGULATIONGROUPMAPPINGAPPROVAL", new
                {
                    CountryRegulationGroupMappingId = access.CountryRegulationGroupMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                return true;
            }
        }

        public async Task<List<RegulationGroupModel>> GetRegulationGroupApprovalList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETREGULATIONGROUPAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<RegulationGroupModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<RegulationGroupModel>> GetCountryRegulationGroupMappingApproval(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYREGULATIONGROUPMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<RegulationGroupModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }
    }
}
