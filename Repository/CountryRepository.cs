using DmsApi.Models;
using DmsApi.Helpers;
using Dapper;

namespace DmsApi.Repository
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAllCountries();
        Task<Country> GetCountryByUID(Guid uid);
        Task<States> GetStatesByCountry(int CountryCode);
        Task<Country> PostCountry(Country country);
        Task<bool> PostUpdateCountryApproval(AccessModel access, string ApprovalStatus );
        Task<States> PostState(States state);
        Task<bool> PostUpdateStateApproval(AccessModel access, string ApprovalStatus);
        Task<List<States>> GetStateById(int countryId);
        Task<List<CountryStateMapping>> GetCountryStatesMapping();
        Task<CountryStateMapping> PostCountryStateMapping(CountryStateMapping country);
        Task<CountryStateMapping> DeleteCountryStateMapping(CountryStateMapping country);

        Task<List<CountryStateApproval>> GetCountryApprovaList(Guid UserUID);

        Task<List<CountryStateApproval>> GetStateApprovaList(Guid UserUID);

        Task<List<CountryStateApproval>> GetCountryStateMappingApprovaList(Guid UserUID);
        Task<bool> PostCountryStateApprovalMapping(AccessModel access, string ApprovalStatus);
        //Task<bool> PostApproveRejectState(AccessModel access);
    }
    public class CountryRepository : ICountryRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        public CountryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Country>> GetAllCountries()
        {
           
            var query = "SELECT * FROM Country";

            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<Country>(query);
                return result.ToList();
            }
        }
        public async Task<Country> GetCountryByUID(Guid uid)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Country>().FirstOrDefault();
                mul.Dispose();
                return Result;
            }
        }
        public async Task<States> GetStatesByCountry(int countryCode)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {

                var mul = await sqlContext.QueryMultipleAsync("USP_GETSTATEBYCOUNTRY", new { CountryCode = countryCode }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<States>().FirstOrDefault();
                mul.Dispose();
                return Result;
            }


        }

        public async Task<Country> PostCountry(Country country)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<Country>("[product_owner].USP_POSTCOUNTRY", new
                {
                    Id = country.Id,
                    CountryName = country.CountryName,
                    CountryCode = country.CountryCode,
                    CountryCodeNumber = country.CountryCodeNumber,
                    FinancialStartDate = country.FinancialStartDate,
                    FinancialEndDate = country.FinancialEndDate,
                    CreatedBy = country.CreatedBy,
                    ModifiedBy = country.ModifiedBy,
                    UID = country.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }


        public async Task<bool> PostUpdateCountryApproval(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, CountryId = access.CountryId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATECOUNTRYAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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
        public async Task<States> PostState(States state)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<States>("[product_owner].USP_POSTSTATE", new
                {
                    Id = state.Id,
                    CountryId = state.CountryId,
                    StateName = state.StateName,
                    StateCode = state.StateCode,
                    CreatedBy = state.CreatedBy,
                    ModifiedBy = state.ModifiedBy,
                    UID = state.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }


        }

        public async Task<bool> PostUpdateStateApproval(AccessModel access,string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, StateId = access.StateId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATESTATEAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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


        public async Task<List<States>> GetStateById(int countryId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var query = "SELECT * FROM State WHERE countryId = @countryId";

                using (var mul = await connection.QueryMultipleAsync(query, new { countryId = countryId }))
                {
                    // Read the results into a list of FileDetail
                    var result = mul.Read<States>().ToList();
                    return result;
                }
            }
        }

        public async Task<List<CountryStateMapping>> GetCountryStatesMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYSTATEMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateMapping>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<CountryStateMapping> PostCountryStateMapping(CountryStateMapping country)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<CountryStateMapping>("[product_owner].USP_POSTCOUNTRYSTATEMAPPING", new
                {
                    Id = country.Id,
                    CountryId = country.CountryId,
                    StateId = country.StateId,
                    CreatedBy = country.CreatedBy,
                    UID = country.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }

        public async Task<bool> PostCountryStateApprovalMapping(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATECOUNTRYSTATEMAPPINGAPPROVAL", new
                {
                    CountryStateMappingId = access.CountryStateMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                return true;
            }
        }


        public async Task<List<CountryStateApproval>> GetCountryApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<CountryStateApproval>> GetStateApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETSTATEAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }


        public async Task<List<CountryStateApproval>> GetCountryStateMappingApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYSTATEMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }




        public async Task<CountryStateMapping> DeleteCountryStateMapping(CountryStateMapping country)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<CountryStateMapping>("[product_owner].USP_DELETECOUNTRYSTATEMAPPING", new
                {
                    Id = country.Id,
                    ModifiedBy = country.ModifiedBy,
                    UID = country.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }
    }
}
