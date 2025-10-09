using DmsApi.Helpers;
using DmsApi.Models;
using Dapper;

namespace DmsApi.Repository
{
    public interface IIndustryRepository
    {
        Task<List<MajorIndustry>> GetAllMajorIndustry();
        Task<MajorIndustry> PostMajorIndustry(MajorIndustry majorIndustry);
        Task<bool> PostUpdateMajorIndustryApproval(AccessModel access, string ApprovalStatus);
        Task<bool> PostUpdateMinorIndustryApproval(AccessModel access, string ApprovalStatus);
        Task<MinorIndustry> PostMinorIndustry(MinorIndustry minorIndustry);
        Task<List<MajorIndustry>> GetMajorIndustryById(int countryId);
        Task<List<MinorIndustry>> GetMinorIndustryById(int majorIndustryId);
        Task<MajorIndustry> GetMajorIndustryByUID(Guid uid);
        Task<List<CountryMajorMapping>> GetCountryMajorMapping();
        Task<CountryMajorMapping> PostCountryMajorMapping(CountryMajorMapping countryMajorMapping);
        Task<List<MajorMinorMapping>> GetMajorMinorMapping();
        Task<MajorMinorMapping> PostMajorMinorMapping(MajorMinorMapping majorMinorMapping);
        Task<List<CountryMajorApproval>> GetMajorApprovaList(Guid UserUID);
        Task<List<MajorMinorApproval>> GetMinorApprovaList(Guid UserUID);
        Task<List<CountryMajorApproval>> GetCountryMajorMappingApprovaList(Guid UserUID);
        Task<List<MajorMinorApproval>> GetMajorMinorMappingApprovaList(Guid UserUID);
        Task<bool> PostCountryMajorApprovalMapping(AccessModel access, string ApprovalStatus);
        Task<bool> PostMajorMinorApprovalMapping(AccessModel access, string ApprovalStatus);
        //Task<States> GetStatesByCountry(int CountryCode);
    }
    public class IndustryRepository: IIndustryRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        public IndustryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<MajorIndustry>> GetAllMajorIndustry()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[product_owner].USP_GETALLMAJORINDUSTRY", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<MajorIndustry>().ToList();
                mul.Dispose();
                return result;
            }
        }
        public async Task<MajorIndustry> PostMajorIndustry(MajorIndustry majorIndustry)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<MajorIndustry>("[product_owner].USP_POSTMAJORINDUSTRY", new
                {
                    Id = majorIndustry.Id,
                    CountryId = majorIndustry.CountryId,
                    MajorIndustryName = majorIndustry.MajorIndustryName,
                    MajorIndustryCode = majorIndustry.MajorIndustryCode,
                    CreatedBy = majorIndustry.CreatedBy,
                    ModifiedBy = majorIndustry.ModifiedBy,
                    UID = majorIndustry.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                return mul;
            }
        }
        public async Task<MinorIndustry> PostMinorIndustry(MinorIndustry minorIndustry)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<MinorIndustry>("[product_owner].USP_POSTMINORINDUSTRY", new
                {
                    Id = minorIndustry.Id,
                    MajorIndustryId = minorIndustry.MajorIndustryId,
                    MinorIndustryName = minorIndustry.MinorIndustryName,
                    MinorIndustryCode = minorIndustry.MinorIndustryCode,
                    CreatedBy = minorIndustry.CreatedBy,
                    ModifiedBy = minorIndustry.ModifiedBy,
                    UID = minorIndustry.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }


        }
        public async Task<List<MajorIndustry>> GetMajorIndustryById(int countryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORINDUSTRYBYID", new { CountryId = countryId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<MajorIndustry>().ToList();
                mul.Dispose();
                return Result;
            }
        }
        public async Task<List<MinorIndustry>> GetMinorIndustryById(int majorIndustryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMINORINDUSTRYBYID", new { MajorIndustryId = majorIndustryId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<MinorIndustry>().ToList();
                mul.Dispose();
                return Result;
            }
        }
        public async Task<MajorIndustry> GetMajorIndustryByUID(Guid uid)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORINDUSTRYBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<MajorIndustry>().FirstOrDefault();
                mul.Dispose();
                return Result;
            }
        }
        public async Task<bool> PostUpdateMajorIndustryApproval(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, MajorIndustryId = access.MajorIndustryId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEMAJORINDUSTRYAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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
        public async Task<bool> PostUpdateMinorIndustryApproval(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, MinorIndustryId = access.MinorIndustryId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEMINORINDUSTRYAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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
        public async Task<List<CountryMajorMapping>> GetCountryMajorMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYMAJORINDUSTRYMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryMajorMapping>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<CountryMajorMapping> PostCountryMajorMapping(CountryMajorMapping countryMajorMapping)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<CountryMajorMapping>("[product_owner].USP_POSTCOUNTRYMAJORINDUSTRYMAPPING", new
                {
                    Id = countryMajorMapping.Id,
                    CountryId = countryMajorMapping.CountryId,
                    MajorIndustryId = countryMajorMapping.MajorIndustryId,
                    CreatedBy = countryMajorMapping.CreatedBy,
                    UID = countryMajorMapping.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }
        public async Task<bool> PostCountryMajorApprovalMapping(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATECOUNTRYMAJORMAPPINGAPPROVAL", new
                {
                    CountryMajorIndustryMappingId = access.CountryMajorIndustryMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                return true;
            }
        }
        public async Task<bool> PostMajorMinorApprovalMapping(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEMAJORMINORMAPPINGAPPROVAL", new
                {
                    MajorMinorIndustryMappingId = access.MajorMinorIndustryMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                return true;
            }
        }
        public async Task<List<MajorMinorMapping>> GetMajorMinorMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORMINORINDUSTRYMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<MajorMinorMapping>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<MajorMinorMapping> PostMajorMinorMapping(MajorMinorMapping majorMinorMapping)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<MajorMinorMapping>("[product_owner].USP_POSTMAJORMINORINDUSTRYMAPPING", new
                {
                    Id = majorMinorMapping.Id,
                    MajorIndustryId = majorMinorMapping.MajorIndustryId,
                    MinorIndustryId = majorMinorMapping.MinorIndustryId,
                    CreatedBy = majorMinorMapping.CreatedBy,
                    UID = majorMinorMapping.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }
        public async Task<List<CountryMajorApproval>> GetMajorApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORINDUSTRYAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryMajorApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<MajorMinorApproval>> GetMinorApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMINORINDUSTRYAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<MajorMinorApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<List<CountryMajorApproval>> GetCountryMajorMappingApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYMAJORINDUSTRYMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryMajorApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<List<MajorMinorApproval>> GetMajorMinorMappingApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORMINORINDUSTRYMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<MajorMinorApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }
    }
}
