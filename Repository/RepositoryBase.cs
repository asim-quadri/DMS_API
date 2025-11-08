using AutoMapper;

namespace ComplianceAPI.Repository
{
    public class RepositoryBase
    {
        protected readonly string ConnectionString;
        protected readonly IMapper Mapper = null;

        protected RepositoryBase(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("MasterConnection");
        }

        protected RepositoryBase(IConfiguration configuration, IMapper mapper) : this(configuration)
        {
            Mapper = mapper;
        }

        //protected SqlConnection ConnectionFactory()
        //{
        //    return new SqlConnection(ConnectionString);
        //}
    }
}