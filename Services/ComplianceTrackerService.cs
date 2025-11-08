using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface IComplianceTrackerService
    {
        Task<List<ComplianceRegulationGroupModel>> GetAllRegulationGroups();

        Task<List<TOCListModel>> GetTOCList(long complianceId);

        Task<List<TOCListModel>> GetRegulationTOCList(long regulationId);

        Task<List<RegulationListModel>> GetRegComplianceDetails();

        Task<List<RegulationListModel>> GetRegComplianceDetailsByEntityId(long entityId);

        Task<List<Models.Entity>> GetEntities();

        Task<Models.Entity> GetEntityById(long? entityId);

        Task<bool> PostComplianceTracker(ComplianceTracker compliance);

        Task<List<RegulationSetupCompliance>> GetAllCompliances();
    }

    public class ComplianceTrackerService : IComplianceTrackerService
    {
        public readonly IComplianceTrackerRepository _complianceTrackerRepository;

        public ComplianceTrackerService(IComplianceTrackerRepository complianceTrackerRepository)
        {
            this._complianceTrackerRepository = complianceTrackerRepository;
        }

        public async Task<List<ComplianceRegulationGroupModel>> GetAllRegulationGroups()
        {
            return await _complianceTrackerRepository.GetAllRegulationGroups();
        }

        public async Task<List<TOCListModel>> GetTOCList(long complianceId)
        {
            return await _complianceTrackerRepository.GetTOCList(complianceId);
        }

        public async Task<List<TOCListModel>> GetRegulationTOCList(long regulationId)
        {
            return await _complianceTrackerRepository.GetRegulationTOCList(regulationId);
        }

        public async Task<List<RegulationListModel>> GetRegComplianceDetails()
        {
            return await _complianceTrackerRepository.GetRegComplianceDetails();
        }

        public async Task<List<RegulationListModel>> GetRegComplianceDetailsByEntityId(long entityId)
        {
            return await _complianceTrackerRepository.GetRegComplianceDetailsByEntityId(entityId);
        }

        public async Task<List<Models.Entity>> GetEntities()
        {
            return await _complianceTrackerRepository.GetEntities();
        }

        public async Task<Models.Entity> GetEntityById(long? entityId)
        {
            return await _complianceTrackerRepository.GetEntityById(entityId);
        }

        public async Task<bool> PostComplianceTracker(ComplianceTracker compliance)
        {
            return await _complianceTrackerRepository.PostComplianceTracker(compliance);
        }

        public async Task<List<RegulationSetupCompliance>> GetAllCompliances()
        {
            return await _complianceTrackerRepository.GetAllCompliances();
        }
    }
}