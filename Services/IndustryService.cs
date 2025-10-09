using DmsApi.Helpers;
using DmsApi.Models;
using DmsApi.Repository;
using System.Diagnostics.Metrics;

namespace DmsApi.Services
{
    public interface IIndustryService
    {
        Task<List<MajorIndustry>> GetAllMajorIndustry();
        Task<MajorIndustry> PostMajorIndustry(MajorIndustry majorIndustry);
        Task<bool> PostMajorIndustryForward(AccessModel access);
        Task<MinorIndustry> PostMinorIndustry(MinorIndustry minorIndustry);
        Task<bool> PostMinorIndustryForward(AccessModel access);
        Task<List<MajorIndustry>> GetMajorIndustryById(int countryId);
        Task<List<MinorIndustry>> GetMinorIndustryById(int MajorIndustryId);
        Task<MajorIndustry> GetMajorIndustryByUID(Guid uid);
        Task<List<CountryMajorMapping>> GetCountryMajorMapping();
        Task<CountryMajorMapping> PostCountryMajorMapping(CountryMajorMapping countryMajorMapping);
        Task<List<MajorMinorMapping>> GetMajorMinorMapping();
        Task<MajorMinorMapping> PostMajorMinorMapping(MajorMinorMapping majorMinorMapping);
        Task<List<CountryMajorApproval>> GetMajorIndustryApprovaList(Guid UserUID);
        Task<List<MajorMinorApproval>> GetMinorIndustryApprovaList(Guid UserUID);
        Task<List<CountryMajorApproval>> GetCountryMajorMappingApprovaList(Guid UserUID);
        Task<List<MajorMinorApproval>> GetMajorMinorMappingApprovaList(Guid UserUID);
        Task<bool> PostMajorIndustryApprove(AccessModel access);
        Task<bool> PostMajorIndustryReject(AccessModel access);
        Task<bool> PostMinorIndustryApprove(AccessModel access);
        Task<bool> PostMinorIndustryReject(AccessModel access);
        Task<bool> PostCountryMajorMappingApprove(AccessModel access);
        Task<bool> PostCountryMajorMappingReject(AccessModel access);
        Task<bool> PostMajorMinorMappingApprove(AccessModel access);
        Task<bool> PostMajorMinorMappingReject(AccessModel access);
        Task<bool> PostCountryMajorIndustryForward(AccessModel access);
        Task<bool> PostMajorMinorIndustryForward(AccessModel access);
    }
    public class IndustryService : IIndustryService
    {
        private readonly IIndustryRepository _industryRepository;
        public IndustryService(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }
        public async Task<List<MajorIndustry>> GetAllMajorIndustry()
        {
            return await _industryRepository.GetAllMajorIndustry();
        }
        public async Task<MajorIndustry> PostMajorIndustry(MajorIndustry majorIndustry)
        {
            var result = await _industryRepository.PostMajorIndustry(majorIndustry);
            AccessModel access = new AccessModel() { CreatedBy = majorIndustry.CreatedBy, ManagerId = majorIndustry.ManagerId, Status = 0, MajorIndustryId = result.Id, CountryId=result.CountryId };
            await _industryRepository.PostUpdateMajorIndustryApproval(access, RefApprovalStatusU.Pending);
            return result;
        }
        public async Task<bool> PostMajorIndustryForward(AccessModel access)
        {
            return await _industryRepository.PostUpdateMajorIndustryApproval(access, RefApprovalStatusU.Forward);
        }
        public async Task<MinorIndustry> PostMinorIndustry(MinorIndustry minorIndustry)
        {
            var result = await _industryRepository.PostMinorIndustry(minorIndustry);
            AccessModel access = new AccessModel() { CreatedBy = minorIndustry.CreatedBy, ManagerId = minorIndustry.ManagerId, Status = 0, MinorIndustryId = result.Id, MajorIndustryId = result.MajorIndustryId };
            await _industryRepository.PostUpdateMinorIndustryApproval(access, RefApprovalStatusU.Pending);
            return result;
        }
        public async Task<bool> PostMinorIndustryForward(AccessModel access)
        {
            return await _industryRepository.PostUpdateMinorIndustryApproval(access, RefApprovalStatusU.Forward);
        }
        public async Task<List<MajorIndustry>> GetMajorIndustryById(int countryId)
        {
            return await _industryRepository.GetMajorIndustryById(countryId);
        }
        public async Task<List<MinorIndustry>> GetMinorIndustryById(int majorIndustryId)
        {
            return await _industryRepository.GetMinorIndustryById(majorIndustryId);
        }
        public async Task<MajorIndustry> GetMajorIndustryByUID(Guid uid)
        {
            return await _industryRepository.GetMajorIndustryByUID(uid);
        }
        public async Task<List<CountryMajorMapping>> GetCountryMajorMapping()
        {
            return await _industryRepository.GetCountryMajorMapping();
        }
        public async Task<CountryMajorMapping> PostCountryMajorMapping(CountryMajorMapping countryMajorMapping)
        {
            var result = await _industryRepository.PostCountryMajorMapping(countryMajorMapping);
            AccessModel access = new AccessModel() { CreatedBy = countryMajorMapping.CreatedBy, ManagerId = countryMajorMapping.ManagerId, Status = 0, CountryMajorIndustryMappingId = result.Id };
            await _industryRepository.PostCountryMajorApprovalMapping(access, RefApprovalStatusU.Pending);
            return result;
        }
        public async Task<MajorMinorMapping> PostMajorMinorMapping(MajorMinorMapping majorMinorMapping)
        {
            var result = await _industryRepository.PostMajorMinorMapping(majorMinorMapping);
            AccessModel access = new AccessModel() { CreatedBy = majorMinorMapping.CreatedBy, ManagerId = majorMinorMapping.ManagerId, Status = 0, MajorMinorIndustryMappingId = result.Id };
            await _industryRepository.PostMajorMinorApprovalMapping(access, RefApprovalStatusU.Pending);
            return result;
        }
        public async Task<List<MajorMinorMapping>> GetMajorMinorMapping()
        {
            return await _industryRepository.GetMajorMinorMapping();
        }
        public async Task<List<CountryMajorApproval>> GetMajorIndustryApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetMajorApprovaList(UserUID);
        }
        public async Task<List<MajorMinorApproval>> GetMinorIndustryApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetMinorApprovaList(UserUID);
        }
        public async Task<List<CountryMajorApproval>> GetCountryMajorMappingApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetCountryMajorMappingApprovaList(UserUID);
        }
        public async Task<List<MajorMinorApproval>> GetMajorMinorMappingApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetMajorMinorMappingApprovaList(UserUID);
        }
        public async Task<bool> PostMajorIndustryApprove(AccessModel access)
        {
            return await _industryRepository.PostUpdateMajorIndustryApproval(access, RefApprovalStatusU.Approved);
        }
        public async Task<bool> PostMajorIndustryReject(AccessModel access)
        {
            return await _industryRepository.PostUpdateMajorIndustryApproval(access, RefApprovalStatusU.Rejected);
        }
        public async Task<bool> PostMinorIndustryApprove(AccessModel access)
        {
            return await _industryRepository.PostUpdateMinorIndustryApproval(access, RefApprovalStatusU.Approved);
        }
        public async Task<bool> PostMinorIndustryReject(AccessModel access)
        {
            return await _industryRepository.PostUpdateMinorIndustryApproval(access, RefApprovalStatusU.Rejected);
        }
        public async Task<bool> PostCountryMajorMappingApprove(AccessModel access)
        {
            return await _industryRepository.PostCountryMajorApprovalMapping(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostCountryMajorMappingReject(AccessModel access)
        {
            return await _industryRepository.PostCountryMajorApprovalMapping(access, RefApprovalStatusU.Rejected);
        }
        public async Task<bool> PostMajorMinorMappingApprove(AccessModel access)
        {
            return await _industryRepository.PostMajorMinorApprovalMapping(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostMajorMinorMappingReject(AccessModel access)
        {
            return await _industryRepository.PostMajorMinorApprovalMapping(access, RefApprovalStatusU.Rejected);
        }
        public async Task<bool> PostCountryMajorIndustryForward(AccessModel access)
        {
            return await _industryRepository.PostCountryMajorApprovalMapping(access, RefApprovalStatusU.Forward);
        }
        public async Task<bool> PostMajorMinorIndustryForward(AccessModel access)
        {
            return await _industryRepository.PostMajorMinorApprovalMapping(access, RefApprovalStatusU.Forward);
        }
    }
}
