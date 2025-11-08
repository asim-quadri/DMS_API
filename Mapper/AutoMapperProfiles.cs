using AutoMapper;
namespace ComplianceAPI.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Models.DataModels.Entity, Models.Entity>();
        }
    }
}
