using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;

using Helper = ComplianceAPI.Helpers;

namespace ComplianceAPI.Services
{
    public interface IRegulatoryAuthorityService
    {
        Task<Result> PostRegulatoryAuthority(PostRegulatoryAuthorities regulatoryAuthorities);

        Task<string> GetNextRegulatoryAuthRefCode();

        Task<List<ComplianceAPI.Models.RegulatoryAuthorities>> GetAllRegulatoryAuthorities();

        Task<List<PostRegulatoryAuthorities>> GetAllPendingRegAuth(long id);

        Task<Result> SubmitRegulatoryAuthoritiesApprove(PostRegulatoryAuthorities regulatoryAuthorities);

        Task<Result> PostCountryRegulatoryAuthorityMapping(Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorityMapping);

        Task<List<Models.CountryRegulatoryAuthorityMapping>> GetAllPendingRegAuthMapping(long id);

        Task<Result> SubmitRegulatoryAuthoritiesMappingApprove(Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorities);

        Task<List<Models.CountryRegulatoryAuthorityMapping>> GetAllRegAuthMapping();

        Task<List<Models.RegulatoryAuthorities>> GetRegulatoryAuthoritiesListCountry(int countryId);
    }

    public class RegulatoryAuthorityService : IRegulatoryAuthorityService
    {
        private readonly IRegulatoryAuthorityRepository _regulatoryAuthorityRepository;
        private readonly IHelperRepository _helperRepository;

        public RegulatoryAuthorityService(IRegulatoryAuthorityRepository regulatoryAuthorityRepository, IHelperRepository helperRepository)
        {
            _regulatoryAuthorityRepository = regulatoryAuthorityRepository;
            _helperRepository = helperRepository;
        }

        public async Task<List<PostRegulatoryAuthorities>> GetAllPendingRegAuth(long id)
        {
            return await _regulatoryAuthorityRepository.GetAllPendingRegAuth(id);
        }

        public async Task<List<Models.CountryRegulatoryAuthorityMapping>> GetAllPendingRegAuthMapping(long id)
        {
            return await _regulatoryAuthorityRepository.GetAllPendingRegAuthMapping(id);
        }

        public async Task<List<Models.CountryRegulatoryAuthorityMapping>> GetAllRegAuthMapping()
        {
            return await _regulatoryAuthorityRepository.GetAllRegAuthMapping();
        }

        public async Task<List<Models.RegulatoryAuthorities>> GetAllRegulatoryAuthorities()
        {
            return await _regulatoryAuthorityRepository.GetAllRegulatoryAuthorities();
        }

        public async Task<string> GetNextRegulatoryAuthRefCode()
        {
            return await _regulatoryAuthorityRepository.GetNextRegulatoryAuthRefCode();
        }

        public async Task<List<Models.RegulatoryAuthorities>> GetRegulatoryAuthoritiesListCountry(int countryId)
        {
            return await _regulatoryAuthorityRepository.GetRegulatoryAuthoritiesListCountry(countryId);
        }

        public async Task<Result> PostCountryRegulatoryAuthorityMapping(Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorityMapping)
        {
            var isAdmin = _helperRepository.IsSuperAdmin(countryRegulatoryAuthorityMapping.CreatedBy);

            var approveStatus = isAdmin ? Helper.RefApprovalStatus.Approved : Helper.RefApprovalStatus.Pending;
            return await _regulatoryAuthorityRepository.PostCountryRegulatoryAuthorityMapping(countryRegulatoryAuthorityMapping, approveStatus);
        }

        public async Task<Result> PostRegulatoryAuthority(PostRegulatoryAuthorities regulatoryAuthorities)
        {
            var isAdmin = _helperRepository.IsSuperAdmin(regulatoryAuthorities.CreatedBy);

            var approveStatus = isAdmin ? Helper.RefApprovalStatus.Approved : Helper.RefApprovalStatus.Pending;
            return await _regulatoryAuthorityRepository.PostRegulatoryAuthority(regulatoryAuthorities, approveStatus);
        }

        public async Task<Result> SubmitRegulatoryAuthoritiesApprove(PostRegulatoryAuthorities regulatoryAuthorities)
        {
            int approveStatus;
            if (regulatoryAuthorities.ApproveStatus == RefApprovalStatusU.Approved)
            {
                approveStatus = Helper.RefApprovalStatus.Approved;
            }
            else if (regulatoryAuthorities.ApproveStatus == RefApprovalStatusU.Rejected)
            {
                approveStatus = Helper.RefApprovalStatus.Rejected;
            }
            else if (regulatoryAuthorities.ApproveStatus == RefApprovalStatusU.Reviewed)
            {
                approveStatus = Helper.RefApprovalStatus.Reviewed;
            }
            else if (regulatoryAuthorities.ApproveStatus == RefApprovalStatusU.Forward)
            {
                approveStatus = Helper.RefApprovalStatus.Forward;
            }
            else
            {
                approveStatus = Helper.RefApprovalStatus.Pending;
            }
            return await _regulatoryAuthorityRepository.PostRegulatoryAuthority(regulatoryAuthorities, approveStatus);
        }

        public async Task<Result> SubmitRegulatoryAuthoritiesMappingApprove(Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorities)
        {
            int approveStatus;
            if (countryRegulatoryAuthorities.ApproveStatus == RefApprovalStatusU.Approved)
            {
                approveStatus = Helper.RefApprovalStatus.Approved;
            }
            else if (countryRegulatoryAuthorities.ApproveStatus == RefApprovalStatusU.Rejected)
            {
                approveStatus = Helper.RefApprovalStatus.Rejected;
            }
            else if (countryRegulatoryAuthorities.ApproveStatus == RefApprovalStatusU.Reviewed)
            {
                approveStatus = Helper.RefApprovalStatus.Reviewed;
            }
            else if (countryRegulatoryAuthorities.ApproveStatus == RefApprovalStatusU.Forward)
            {
                approveStatus = Helper.RefApprovalStatus.Forward;
            }
            else
            {
                approveStatus = Helper.RefApprovalStatus.Pending;
            }
            return await _regulatoryAuthorityRepository.PostCountryRegulatoryAuthorityMapping(countryRegulatoryAuthorities, approveStatus);
        }
    }
}