using Dapper;

namespace ComplianceAPI.Repository
{
    public interface ISqlHelperRepository
    {
        Task<List<dynamic>> GetDataSet(string sp, object? param = null);
        Task<List<dynamic>> GetDataTable(string sp, object? param = null);
        Task<int> ExcuteNonQuery(string sp, object? param = null);
    }

    public class SqlHelperRepository : ISqlHelperRepository
    {
        protected readonly IUnitOfWork _unitOfWork;


        public SqlHelperRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<dynamic>> GetDataSet(string sp, object? param = null)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                List<dynamic> list = new List<dynamic>();
                var result = await connection.QueryMultipleAsync(sp, param, commandType: System.Data.CommandType.StoredProcedure);
                while (!result.IsConsumed)
                {
                    list.Add(result.Read<dynamic>().AsList());
                }
                return list;
            }
        }

        public async Task<List<dynamic>> GetDataTable(string sp, object? param = null)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                List<dynamic> list = new List<dynamic>();
                var result = await connection.QueryAsync(sp, param, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<int> ExcuteNonQuery(string sp, object? param = null)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                List<dynamic> list = new List<dynamic>();
                var result = await connection.ExecuteAsync(sp, param, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }
    }
}