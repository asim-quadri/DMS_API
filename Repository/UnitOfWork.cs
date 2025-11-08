using System.Data;
using System.Data.SqlClient;

namespace ComplianceAPI.Repository
{
    public interface IUnitOfWork
    {
        IDbConnection ConnectionFactory();
        ISqlContext ContextFactory();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly string _connection;

        public UnitOfWork(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("MasterConnection");
        }

        public IDbConnection ConnectionFactory()
        {
            var connection = new SqlConnection(_connection);
            connection.Open();
            return connection;
        }

        public ISqlContext ContextFactory()
        {
            var connection = ConnectionFactory();
            var transaction = connection.BeginTransaction();

            return new SqlContext(transaction);
        }
    }
}
