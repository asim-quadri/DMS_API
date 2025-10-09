using DmsApi.Helpers;
using DmsApi.Models;
using DmsApi.Repository;
using System.Diagnostics.Metrics;

namespace DmsApi.Services
{
    public interface ICountryServices
    {
        Task<List<Country>> GetAllCountries();
        Task<Country> GetCountryByUID(Guid uid);
        Task<States> GetStatesByCountry(int CountryCode);
        Task<Country> PostCountry(Country country);
        Task<bool> PostCountryApprove(AccessModel access);
        Task<bool> PostCountryReject(AccessModel access);
        Task<bool> PostCountryForward(AccessModel access);
        Task<States> PostState(States state);
        Task<List<States>> GetStateById(int countryId);
        Task<bool> PostStateApprove(AccessModel access);
        Task<bool> PostStateReject(AccessModel access);
        Task<bool> PostStateForward(AccessModel access);
        Task<List<CountryStateMapping>> GetCountryStatesMapping();
        Task<CountryStateMapping> PostCountryStateMapping(CountryStateMapping country);

        Task<List<CountryStateApproval>> GetCountryApprovaList(Guid UserUID);
        Task<List<CountryStateApproval>> GetCountryStateMappingApprovaList(Guid UserUID);
        Task<bool> PostCountryStateMappingApprove(AccessModel access);
        Task<bool> PostCountryStateMappingReject(AccessModel access);
        Task<bool> PostCountryStateMappingForward(AccessModel access);
        Task<CountryStateMapping> DeleteCountryStateMapping(CountryStateMapping country);
    }
    public class CountryService : ICountryServices
    {
        private readonly ICountryRepository _countryRepository;
        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public async Task<List<Country>> GetAllCountries()
        {
            return await _countryRepository.GetAllCountries();
        }
        public async Task<Country> GetCountryByUID(Guid uid)
        {
            return await _countryRepository.GetCountryByUID(uid);
        }
        public async Task<States> GetStatesByCountry(int CountryCode)
        {
            return await _countryRepository.GetStatesByCountry(CountryCode);
        }
        public async Task<Country> PostCountry(Country country)
        {
            var result = await _countryRepository.PostCountry(country);
            AccessModel access = new AccessModel() { CreatedBy = country.CreatedBy, ManagerId = country.ManagerId, Status = 0, CountryId = result.Id };
            await _countryRepository.PostUpdateCountryApproval(access, RefApprovalStatusU.Pending);
            return result;
        }

        public async Task<States> PostState(States states)
        {
            var result = await _countryRepository.PostState(states);
            AccessModel access = new AccessModel() { CreatedBy = states.CreatedBy, ManagerId = states.ManagerId, Status = 0, StateId = result.Id };
            await _countryRepository.PostUpdateStateApproval(access,RefApprovalStatusU.Pending);
            return result;
        }

        public async Task<bool> PostCountryApprove(AccessModel access)
        {
            return await _countryRepository.PostUpdateCountryApproval(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostCountryReject(AccessModel access)
        {
            return await _countryRepository.PostUpdateCountryApproval(access, RefApprovalStatusU.Rejected);
        }

        public async Task<bool> PostCountryForward(AccessModel access)
        {
            return await _countryRepository.PostUpdateCountryApproval(access, RefApprovalStatusU.Forward);
        }

        public async Task<List<States>> GetStateById(int countryId)
        {
            return await _countryRepository.GetStateById(countryId);
        }

        public async Task<List<CountryStateMapping>> GetCountryStatesMapping()
        {
            return await _countryRepository.GetCountryStatesMapping();
        }

        public async Task<CountryStateMapping> PostCountryStateMapping(CountryStateMapping country)
        {
            var result = await _countryRepository.PostCountryStateMapping(country);
            AccessModel access = new AccessModel() { CreatedBy = country.CreatedBy, ManagerId = country.ManagerId, Status = 0, CountryStateMappingId = result.Id };
            await _countryRepository.PostCountryStateApprovalMapping(access, RefApprovalStatusU.Pending);
            return result;
        }

        public async Task<bool> PostStateApprove(AccessModel access)
        {
            return await _countryRepository.PostUpdateStateApproval(access, RefApprovalStatusU.Approved);
        }
        public async Task<bool> PostStateReject(AccessModel access)
        {
            return await _countryRepository.PostUpdateStateApproval(access, RefApprovalStatusU.Rejected);
        }

        public async Task<bool> PostStateForward(AccessModel access)
        {
            return await _countryRepository.PostUpdateStateApproval(access, RefApprovalStatusU.Forward);
        }


        public async Task<bool> PostCountryStateMappingApprove(AccessModel access)
        {
            return await _countryRepository.PostCountryStateApprovalMapping(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostCountryStateMappingReject(AccessModel access)
        {
            return await _countryRepository.PostCountryStateApprovalMapping(access, RefApprovalStatusU.Rejected);
        }

        public async Task<bool> PostCountryStateMappingForward(AccessModel access)
        {
            return await _countryRepository.PostCountryStateApprovalMapping(access, RefApprovalStatusU.Forward);
        }



        public async Task<CountryStateMapping> DeleteCountryStateMapping(CountryStateMapping country)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CountryStateApproval>> GetCountryApprovaList(Guid UserUID)
        {
            var coutry = await _countryRepository.GetCountryApprovaList(UserUID);
            var states = await _countryRepository.GetStateApprovaList(UserUID);
            return coutry.Concat(states).ToList();
        }

        public async Task<List<CountryStateApproval>> GetCountryStateMappingApprovaList(Guid UserUID)
        {
            return await _countryRepository.GetCountryStateMappingApprovaList(UserUID);
        }
    }
}
