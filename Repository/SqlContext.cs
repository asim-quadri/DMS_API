using System.Data;

namespace DmsApi.Repository
{
    public interface ISqlContext: IDisposable
    {

        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        void Commit();
        void Rollback();
    }
    public class SqlContext : ISqlContext
    {
        public SqlContext(IDbTransaction transaction)
        {
            
            Transaction = transaction;
            Connection = transaction.Connection;
        }
        public SqlContext(IDbConnection connection)
        { 
            Connection = connection;
        }

        public IDbConnection Connection { get; private set; }

        public IDbTransaction Transaction { get; private set; } = null;

        public void Commit()
        {
            Transaction?.Commit();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();

            Transaction = null;
            Connection = null;
        }

        public void Rollback()
        {
            Transaction?.Rollback();
        }
    }
}
