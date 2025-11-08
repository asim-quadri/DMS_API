using ComplianceAPI.Helpers;
using dto=ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;
using System.Diagnostics.Metrics;
using System.IO.Pipelines;
using ComplianceAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ComplianceAPI.Services
{
    public interface IIndustryService
    {
        Task<List<dto.MajorIndustry>> GetAllMajorIndustry();
        Task<dto.MajorIndustry> PostMajorIndustry(dto.MajorIndustry majorIndustry);
        Task<bool> PostMajorIndustryForward(dto.AccessModel access);
        Task<dto.MinorIndustry> PostMinorIndustry(dto.MinorIndustry minorIndustry);
        Task<bool> PostMinorIndustryForward(dto.AccessModel access);
        Task<List<dto.MajorIndustry>> GetMajorIndustryById(int countryId);
        Task<List<dto.MinorIndustry>> GetMinorIndustryById(int MajorIndustryId);
        Task<dto.MajorIndustry> GetMajorIndustryByUID(Guid uid);
        Task<List<dto.CountryMajorMapping>> GetCountryMajorMapping();
        Task<List<dto.IndustryrMapping>> GetIndustryMapping();
        Task<List<dto.IndustryrMapping>> GetIndustryMappingByMajor(long? majorInudustryId, long? countryId);
        Task<List<dto.IndustryrMapping>> GetIndustryMappingByCountry(long? countryId);
        Task<List<dto.MajorMinorMapping>> GetMajorMinorMapping();
        Task<dto.MajorMinorMapping> PostMajorMinorMapping(dto.MajorMinorMapping majorMinorMapping);
        Task<List<dto.MajorMinorApproval>> GetMajorIndustryApprovaList(Guid UserUID);
        Task<List<dto.MajorMinorApproval>> GetMinorIndustryApprovaList(Guid UserUID);
        Task<List<dto.CountryMajorApproval>> GetCountryMajorMappingApprovaList(Guid UserUID);
        Task<List<dto.MajorMinorApproval>> GetMajorMinorMappingApprovaList(Guid UserUID);
        Task<bool> PostIndustryApprove(dto.AccessModel access);
        Task<bool> PostIndustryReject(dto.AccessModel access);
        Task<bool> PostMajorIndustryApprove(dto.AccessModel access);
        Task<bool> PostMajorIndustryReject(dto.AccessModel access);
        Task<bool> PostMinorIndustryApprove(dto.AccessModel access);
        Task<bool> PostMinorIndustryReject(dto.AccessModel access);
        Task<bool> PostMajorMinorMappingApprove(dto.AccessModel access);
        Task<bool> PostMajorMinorMappingReject(dto.AccessModel access);
        Task<bool> PostMajorMinorIndustryForward(dto.AccessModel access);
        Task<dto.IndustryrMapping> PostIndustryMapping(dto.IndustryrMapping countryMajorMapping);
        Task<List<dto.IndustryApproval>> GetIndustryMappingApprovaList(Guid UserUID);
        Task<List<dto.MajorMinorApproval>> GetIndustryApprovaList(Guid UserUID);
        Task<bool> PostIndustryMappingApprove(dto.AccessModel access);
        Task<bool> PostIndustryMappingReject(dto.AccessModel access);
        Task<string> GetNextMinorIndustryCode();
        Task<string> GetNextMajorIndustryCode();
    }
    public class IndustryService : IIndustryService
    {
        private readonly IIndustryRepository _industryRepository;
        public IndustryService(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }
        public async Task<List<dto.MajorIndustry>> GetAllMajorIndustry()
        {
            return await _industryRepository.GetAllMajorIndustry();
        }
        public async Task<dto.MajorIndustry> PostMajorIndustry(dto.MajorIndustry majorIndustry)
        {
            var result = await _industryRepository.PostMajorIndustry(majorIndustry);
            if (majorIndustry.ApprovalUID == null && result.ResponseCode != 0)
            {
                dto.AccessModel access = new dto.AccessModel() { CreatedBy = majorIndustry.CreatedBy, ManagerId = majorIndustry.ManagerId, Status = 0, MajorIndustryId = result.Id};
                await _industryRepository.PostUpdateIndustryApproval(access, RefApprovalStatusU.Pending);
            }
            await _industryRepository.AddMajorIndustryApprovalNotification(result.CreatedBy.Value, majorIndustry.MajorIndustryName);
            return result;
        }
        public async Task<bool> PostMajorIndustryForward(dto.AccessModel access)
        {
            var result = await _industryRepository.PostUpdateIndustryApproval(access, RefApprovalStatusU.Forward);
            if (result)
            {
                await AddMajorIndustryApprovalSuccessNotification(access.MajorIndustryId.Value, RefApprovalStatusU.Forward);
            }
            return result;
        }
        public async Task<dto.MinorIndustry> PostMinorIndustry(dto.MinorIndustry minorIndustry)
        {
            try
            {
                var result = await _industryRepository.PostMinorIndustry(minorIndustry);
                if (result == null)
                {
                    // Handle error, log, or return a default response
                    return null;
                }
                if (minorIndustry.ApprovalUID == null && result.ResponseCode != 0)
                {
                    dto.AccessModel access = new dto.AccessModel() { CreatedBy = minorIndustry.CreatedBy, ManagerId = minorIndustry.ManagerId, Status = 0, MinorIndustryId = result.Id };
                    await _industryRepository.PostUpdateMinorIndustryApproval(access, RefApprovalStatusU.Pending);
                }
                if (result.CreatedBy.HasValue && !string.IsNullOrEmpty(minorIndustry.MinorIndustryName))
                {
                    await _industryRepository.AddMinorIndustryApprovalNotification(result.CreatedBy.Value, minorIndustry.MinorIndustryName);
                }
                return result;
            }            
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> PostMinorIndustryForward(dto.AccessModel access)
        {
            var result = await _industryRepository.PostUpdateMinorIndustryApproval(access, RefApprovalStatusU.Forward);
            if (result)
            {
                await AddMinorIndustryApprovalSuccessNotification(access.MinorIndustryId.Value, RefApprovalStatusU.Forward);
            }
            return result;
        }
        public async Task<List<dto.MajorIndustry>> GetMajorIndustryById(int countryId)
        {
            return await _industryRepository.GetMajorIndustryById(countryId);
        }
        public async Task<List<dto.MinorIndustry>> GetMinorIndustryById(int majorIndustryId)
        {
            return await _industryRepository.GetMinorIndustryById(majorIndustryId);
        }
        public async Task<dto.MajorIndustry> GetMajorIndustryByUID(Guid uid)
        {
            return await _industryRepository.GetMajorIndustryByUID(uid);
        }
        public async Task<List<dto.CountryMajorMapping>> GetCountryMajorMapping()
        {
            return await _industryRepository.GetCountryMajorMapping();
        }
        public async Task<dto.IndustryrMapping> PostIndustryMapping(dto.IndustryrMapping industryMapping)
        {
            var result = await _industryRepository.PostIndustryMapping(industryMapping);
            if (result.ResponseCode != 0)
            {
                dto.AccessModel access = new dto.AccessModel() { CreatedBy = industryMapping.CreatedBy, ManagerId = industryMapping.ManagerId, Status = 0, IndustryMappingId = result.Id };
                await _industryRepository.PostIndustryApprovalMapping(access, RefApprovalStatusU.Pending);
            }
            await _industryRepository.AddCountryMajorMappingApprovalNotification(industryMapping.CreatedBy.Value, industryMapping.MinorIndustryId.Value);
            return result;
        }
        public async Task<dto.MajorMinorMapping> PostMajorMinorMapping(dto.MajorMinorMapping majorMinorMapping)
        {
            var result = await _industryRepository.PostMajorMinorMapping(majorMinorMapping);
            if (result.ResponseCode != 0)
            {
                dto.AccessModel access = new dto.AccessModel() { CreatedBy = majorMinorMapping.CreatedBy, ManagerId = majorMinorMapping.ManagerId, Status = 0, MajorMinorIndustryMappingId = result.Id };
                await _industryRepository.PostMajorMinorApprovalMapping(access, RefApprovalStatusU.Pending);
            }
            return result;
        }
        public async Task<List<dto.MajorMinorMapping>> GetMajorMinorMapping()
        {
            return await _industryRepository.GetMajorMinorMapping();
        }
        public async Task<List<dto.MajorMinorApproval>> GetMajorIndustryApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetMajorApprovaList(UserUID);
        }
        public async Task<List<dto.MajorMinorApproval>> GetMinorIndustryApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetMinorApprovaList(UserUID);
        }
        public async Task<List<dto.CountryMajorApproval>> GetCountryMajorMappingApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetCountryMajorMappingApprovaList(UserUID);
        }
        public async Task<List<dto.MajorMinorApproval>> GetMajorMinorMappingApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetMajorMinorMappingApprovaList(UserUID);
        }
        public async Task<bool> PostIndustryApprove(dto.AccessModel access)
        {
            return await _industryRepository.PostUpdateIndustryApproval(access, RefApprovalStatusU.Approved);
        }
        public async Task<bool> PostIndustryReject(dto.AccessModel access)
        {
            return await _industryRepository.PostUpdateIndustryApproval(access, RefApprovalStatusU.Rejected);
        }
        public async Task<bool> PostMajorIndustryApprove(dto.AccessModel access)
        {
            var result = await _industryRepository.PostUpdateMajorIndustryApproval(access, RefApprovalStatusU.Approved);
            if (result)
            {
                await AddMajorIndustryApprovalSuccessNotification(access.MajorIndustryId.Value, RefApprovalStatusU.Approved);
            }
            return result;
        }
        public async Task<bool> PostMajorIndustryReject(dto.AccessModel access)
        {
            var result = await _industryRepository.PostUpdateMajorIndustryApproval(access, RefApprovalStatusU.Rejected);
            if (result)
            {
                await AddMajorIndustryApprovalSuccessNotification(access.MajorIndustryId.Value, RefApprovalStatusU.Rejected);
            }
            return result;
        }
        public async Task<bool> PostMinorIndustryApprove(dto.AccessModel access)
        {
            var result = await _industryRepository.PostUpdateMinorIndustryApproval(access, RefApprovalStatusU.Approved);
            if (result)
            {
                await AddMinorIndustryApprovalSuccessNotification(access.MinorIndustryId.Value, RefApprovalStatusU.Approved);
            }
            return result;
        }
        public async Task<bool> PostMinorIndustryReject(dto.AccessModel access)
        {
            var result = await _industryRepository.PostUpdateMinorIndustryApproval(access, RefApprovalStatusU.Rejected);
            if (result)
            {
                await AddMinorIndustryApprovalSuccessNotification(access.MinorIndustryId.Value, RefApprovalStatusU.Rejected);
            }
            return result;
        }
        public async Task<bool> PostMajorMinorMappingApprove(dto.AccessModel access)
        {
            return await _industryRepository.PostMajorMinorApprovalMapping(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostMajorMinorMappingReject(dto.AccessModel access)
        {
            return await _industryRepository.PostMajorMinorApprovalMapping(access, RefApprovalStatusU.Rejected);
        }
        public async Task<bool> PostMajorMinorIndustryForward(dto.AccessModel access)
        {
            return await _industryRepository.PostMajorMinorApprovalMapping(access, RefApprovalStatusU.Forward);
        }
        public async Task<List<dto.IndustryrMapping>> GetIndustryMapping()
        {
            return await _industryRepository.GetIndustryMapping();
        }
        public async Task<List<dto.IndustryrMapping>> GetIndustryMappingByMajor(long? majorInudustryId, long? countryId)
        {
            return await _industryRepository.GetIndustryMappingByMajor(majorInudustryId, countryId);
        }
        public async Task<List<dto.IndustryrMapping>> GetIndustryMappingByCountry(long? countryId)
        {
            return await _industryRepository.GetIndustryMappingByCountry(countryId);
        }
        public async Task<List<dto.IndustryApproval>> GetIndustryMappingApprovaList(Guid UserUID)
        {
            return await _industryRepository.GetIndustryMappingApprovaList(UserUID);
        }
        public async Task<List<dto.MajorMinorApproval>> GetIndustryApprovaList(Guid UserUID)
        {
            var major = await _industryRepository.GetMajorApprovaList(UserUID);
            var minor = await _industryRepository.GetMinorApprovaList(UserUID);
            return major.Concat(minor).ToList();
        }
        public async Task<bool> PostIndustryMappingApprove(dto.AccessModel access)
        {
            var result = await _industryRepository.PostIndustryApprovalMapping(access, RefApprovalStatusU.Approved);
            if (result)
            {
                await AddMinorIndustryMappingApprovalSuccessNotification(access.MinorIndustryId.Value, RefApprovalStatusU.Approved);
            }
            return result;
        }
        public async Task<bool> PostIndustryMappingReject(dto.AccessModel access)
        {
            var result = await _industryRepository.PostIndustryApprovalMapping(access, RefApprovalStatusU.Rejected);
            if (result)
            {
                await AddMinorIndustryMappingApprovalSuccessNotification(access.MinorIndustryId.Value, RefApprovalStatusU.Rejected);
            }
            return result;
        }

        public async Task<bool> AddMajorIndustryApprovalSuccessNotification(long majorIndustryId, string approveStatus)
        {
            return await _industryRepository.AddMajorIndustryApprovalSuccessNotification(majorIndustryId, approveStatus);
        }

        public async Task<bool> AddMinorIndustryApprovalSuccessNotification(long minorIndustryId, string approveStatus)
        {
            return await _industryRepository.AddMinorIndustryApprovalSuccessNotification(minorIndustryId, approveStatus);
        }

        public async Task<bool> AddMinorIndustryMappingApprovalSuccessNotification(long minorIndustryId, string approveStatus)
        {
            return await _industryRepository.AddMinorIndustryMappingApprovalSuccessNotification(minorIndustryId, approveStatus);
        }
        public async Task<string> GetNextMinorIndustryCode()
        {
            return await _industryRepository.GetNextMinorIndustryCode();
        }
        public async Task<string> GetNextMajorIndustryCode()
        {
            return await _industryRepository.GetNextMajorIndustryCode();
        }

    }
}
