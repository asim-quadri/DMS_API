using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface IReportService
    {
        Task<List<Country>> GetAllCountriesAsync();

        Task<List<ReportMaster>> GetAllReportMaster();

        Task<List<RegulationStupDetails>> GetAllRegulationWithCountriesAsync();

        Task<List<RegulationComplianceWithCountryModel>> GetAllComplianceWithCountryAsync();

        Task<List<EntityTypeModel>> GetAllEntityTypes();

        Task<List<IndustryrMapping>> GetIndustryMapping();
    }

    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<RegulationComplianceWithCountryModel>> GetAllComplianceWithCountryAsync()
        {
            return await _reportRepository.GetAllComplianceWithCountryAsync();
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            return await _reportRepository.GetAllCountriesAsync();
        }

        public Task<List<EntityTypeModel>> GetAllEntityTypes()
        {
            return _reportRepository.GetAllEntityTypes();
        }

        public async Task<List<RegulationStupDetails>> GetAllRegulationWithCountriesAsync()
        {
            return await _reportRepository.GetAllRegulationWithCountriesAsync();
        }

        public async Task<List<ReportMaster>> GetAllReportMaster()
        {
            return await _reportRepository.GetAllReportMaster();
        }

        public async Task<List<IndustryrMapping>> GetIndustryMapping()
        {
            return await _reportRepository.GetIndustryCountryMapping();
        }
    }
}