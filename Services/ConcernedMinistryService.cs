using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using Db = ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;

using Helper = ComplianceAPI.Helpers;

namespace ComplianceAPI.Services
{
    public interface IConcernedMinistryService
    {
        Task<Result> PostConcernedMinistry(PostConcernedMinistry postConcernedMinistry);

        Task<string> GetNextConcernedMinistryRefCode();

        Task<List<ComplianceAPI.Models.ConcernedMinistry>> GetAllConcernedMinistry();

        Task<List<PostConcernedMinistry>> GetAllPendingConcernedMinistry(long id);

        Task<Result> SubmitConcernedMinistriesApprove(PostConcernedMinistry postConcernedMinistry);

        Task<Result> PostCountryConcernedMinistryMapping(Models.CountryConcernedMinistryMapping countryConcernedMinistry);

        Task<List<Models.CountryConcernedMinistryMapping>> GetAllPendingConcernedMinistryMapping(long id);

        Task<Result> SubmitConcernedMinistriesMappingApprove(Models.CountryConcernedMinistryMapping countryConcernedMinistry);

        Task<List<Models.CountryConcernedMinistryMapping>> GetAllConcernedMinistryMapping();

        Task<List<Models.ConcernedMinistry>> GetConcernedMinistryListCountry(int countryId);
    }

    public class ConcernedMinistryService : IConcernedMinistryService
    {
        private readonly IConcernedMinistryRepository _concernedMinistryRepository;
        private readonly IHelperRepository _helperRepository;

        public ConcernedMinistryService(IConcernedMinistryRepository concernedMinistryRepository, IHelperRepository helperRepository)
        {
            _concernedMinistryRepository = concernedMinistryRepository;
            _helperRepository = helperRepository;
        }

        public async Task<List<CountryConcernedMinistryMapping>> GetAllConcernedMinistryMapping()
        {
            return await _concernedMinistryRepository.GetAllConcernedMinistryMapping();
        }

        public async Task<List<PostConcernedMinistry>> GetAllPendingConcernedMinistry(long id)
        {
            return await _concernedMinistryRepository.GetAllPendingConcernedMinistry(id);
        }

        public async Task<List<CountryConcernedMinistryMapping>> GetAllPendingConcernedMinistryMapping(long id)
        {
            return await _concernedMinistryRepository.GetAllPendingConcernedMinistryMapping(id);
        }

        public async Task<List<ConcernedMinistry>> GetAllConcernedMinistry()
        {
            return await _concernedMinistryRepository.GetAllConcernedMinistry();
        }

        public async Task<string> GetNextConcernedMinistryRefCode()
        {
            return await _concernedMinistryRepository.GetNextConcernedMinistryRefCode();
        }

        public async Task<Result> PostConcernedMinistry(PostConcernedMinistry postConcernedMinistry)
        {
            var isAdmin = _helperRepository.IsSuperAdmin(postConcernedMinistry.CreatedBy);
            int approveStatus = isAdmin ? Helper.RefApprovalStatus.Approved : Helper.RefApprovalStatus.Pending;
            return await _concernedMinistryRepository.PostConcernedMinistry(postConcernedMinistry, approveStatus);
        }

        public async Task<Result> PostCountryConcernedMinistryMapping(CountryConcernedMinistryMapping countryConcernedMinistry)
        {
            var isAdmin = _helperRepository.IsSuperAdmin(countryConcernedMinistry.CreatedBy);

            var approveStatus = isAdmin ? Helper.RefApprovalStatus.Approved : Helper.RefApprovalStatus.Pending;
            return await _concernedMinistryRepository.PostCountryConcernedMinistryMapping(countryConcernedMinistry, approveStatus);
        }

        public async Task<Result> SubmitConcernedMinistriesApprove(PostConcernedMinistry postConcernedMinistry)
        {
            int approveStatus;
            if (postConcernedMinistry.ApproveStatus == RefApprovalStatusU.Approved)
            {
                approveStatus = Helper.RefApprovalStatus.Approved;
            }
            else if (postConcernedMinistry.ApproveStatus == RefApprovalStatusU.Rejected)
            {
                approveStatus = Helper.RefApprovalStatus.Rejected;
            }
            else if (postConcernedMinistry.ApproveStatus == RefApprovalStatusU.Reviewed)
            {
                approveStatus = Helper.RefApprovalStatus.Reviewed;
            }
            else if (postConcernedMinistry.ApproveStatus == RefApprovalStatusU.Forward)
            {
                approveStatus = Helper.RefApprovalStatus.Forward;
            }
            else
            {
                approveStatus = Helper.RefApprovalStatus.Pending;
            }
            return await _concernedMinistryRepository.PostConcernedMinistry(postConcernedMinistry, approveStatus);
        }

        public async Task<Result> SubmitConcernedMinistriesMappingApprove(CountryConcernedMinistryMapping countryConcernedMinistry)
        {
            int approveStatus;
            if (countryConcernedMinistry.ApproveStatus == RefApprovalStatusU.Approved)
            {
                approveStatus = Helper.RefApprovalStatus.Approved;
            }
            else if (countryConcernedMinistry.ApproveStatus == RefApprovalStatusU.Rejected)
            {
                approveStatus = Helper.RefApprovalStatus.Rejected;
            }
            else if (countryConcernedMinistry.ApproveStatus == RefApprovalStatusU.Reviewed)
            {
                approveStatus = Helper.RefApprovalStatus.Reviewed;
            }
            else if (countryConcernedMinistry.ApproveStatus == RefApprovalStatusU.Forward)
            {
                approveStatus = Helper.RefApprovalStatus.Forward;
            }
            else
            {
                approveStatus = Helper.RefApprovalStatus.Pending;
            }
            return await _concernedMinistryRepository.PostCountryConcernedMinistryMapping(countryConcernedMinistry, approveStatus);
        }

        public async Task<List<ConcernedMinistry>> GetConcernedMinistryListCountry(int countryId)
        {
            return await _concernedMinistryRepository.GetConcernedMinistryListCountry(countryId);
        }
    }
}