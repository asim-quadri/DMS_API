using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using ComplianceAPI.Repository;

using Enum = ComplianceAPI.Models.Enums;

namespace ComplianceAPI.Services
{
    public interface ICountryServices
    {
        /// <summary>
        /// Get all countries for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Country>> GetAllCountryMaster(int userId);

        Task<List<Country>> GetAllCountries();

        Task<Country> GetCountryByUID(Guid uid);

        Task<List<Country>> GetCountryByOrgId(int id);

        Task<States> GetStatesByCountry(int CountryCode);

        Task<Country> PostCountry(Country country);

        Task<bool> PostCountryApprove(AccessModel access);

        Task<bool> PostCountryReject(AccessModel access);

        Task<bool> PostCountryForward(AccessModel access);

        Task<States> PostState(States state);

        Task<string> SaveCountryFilesAsync(int countryId, List<string> fileNames, int createdBy);

        /// <summary>
        /// Get all states by country id and user id
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<States>> GetStateById(int countryId, int userId);

        Task<bool> PostStateApprove(AccessModel access);

        Task<bool> PostStateReject(AccessModel access);

        Task<bool> PostStateForward(AccessModel access);

        Task<List<CountryStateMappingModel>> GetCountryStatesMapping();

        Task<CountryStateMappingModel> PostCountryStateMapping(CountryStateMappingModel country);

        Task<List<CountryStateApproval>> GetCountryApprovaList(Guid UserUID);

        Task<List<CountryStateApproval>> GetAllCountryStateApproval();

        Task<List<CountryStateApproval>> GetCountryStateMappingApprovaList(Guid UserUID);

        Task<List<CountryStateApproval>> GetAllCountryStateMappingApprovaList();

        Task<bool> PostCountryStateMappingApprove(AccessModel access);

        Task<bool> PostCountryStateMappingReject(AccessModel access);

        Task<bool> PostCountryStateMappingForward(AccessModel access);

        Task<CountryStateMappingModel> DeleteCountryStateMapping(CountryStateMappingModel country);

        /// <summary>
        /// save user country mapping
        /// </summary>
        /// <param name="userCountryMappings"></param>
        /// <returns></returns>
        Task<bool> PostUserCountryMapping(List<UserCountryMappingModel> userCountryMappings);

        /// <summary>
        /// save user state mapping
        /// </summary>
        /// <param name="userStateMappings"></param>
        /// <returns></returns>
        Task<bool> PostUserStateMapping(List<UserStateMappingModel> userStateMappings);

        /// <summary>
        /// Get user state mapping by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserStateMappingResponse>> GetUserStateMapping(int userId);

        Task<List<CurrencyCodes>> GetAllCurrencyCodes();

        Task<StatusCountModel> GetServiceReqAndBillingDetails(long userId, long countryId);
        Task<StatusCountModel> GetServiceReqAndBillingDetailsByState(long userId, long stateId);
        Task<string> GetLastCountryReferenceCode();
        Task<string> GetLastStateReferenceCode();
        Task<string> GetNextStateCode();
        Task<string> GetNextCountryCode();

    }

    public class CountryService : ICountryServices
    {
        private readonly ICountryRepository _countryRepository;
        private readonly INotificationRepository _notificationRepository;

        public CountryService(ICountryRepository countryRepository, INotificationRepository notificationRepository)
        {
            _countryRepository = countryRepository;
            _notificationRepository = notificationRepository;
        }

        ///<see cref="ICountryServices.GetAllCountryMaster(int)"/>
        public async Task<List<Country>> GetAllCountryMaster(int userId)
        {
            var countries = await _countryRepository.GetAllCountryMaster(userId);
            foreach (var country in countries)
            {
                country.FileNames = string.IsNullOrWhiteSpace(country.FileNamesList)
                    ? new List<string>()
                    : country.FileNamesList.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(f => f.Trim())
                                           .ToList();
            }
            return countries;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _countryRepository.GetAllCountries();
        }

        public async Task<Country> GetCountryByUID(Guid uid)
        {
            return await _countryRepository.GetCountryByUID(uid);
        }

        public async Task<List<Country>> GetCountryByOrgId(int id)
        {
            return await _countryRepository.GetCountryByOrgId(id);
        }

        public async Task<States> GetStatesByCountry(int CountryCode)
        {
            return await _countryRepository.GetStatesByCountry(CountryCode);
        }

        public async Task<Country> PostCountry(Country country)
        {
            try
            {
                var result = await _countryRepository.PostCountry(country);
                if (country.ApprovalUID == null && result.ResponseCode != 0)
                {
                    AccessModel access = new AccessModel() { CreatedBy = country.CreatedBy, ManagerId = country.ManagerId == 0 ? 1 : country.ManagerId, Status = 0, CountryId = result.Id };
                    await _countryRepository.PostUpdateCountryApproval(access, RefApprovalStatusU.Pending);
                }
                if (result.Id != 0 && country.FileNames != null && country.FileNames.Count > 0)
                {
                    //add filenames to country
                    await _countryRepository.SaveFileNamesAsync((int)result.Id, country.FileNames, country.CreatedBy);
                    if (country.ApprovalUID == null)
                    {
                        AccessModel access = new AccessModel() { CreatedBy = country.CreatedBy, ManagerId = country.ManagerId == 0 ? 1 : country.ManagerId, Status = 0, CountryId = result.Id };
                        await _countryRepository.PostUpdateCountryFileApproval(access, Models.Enums.RefApprovalStatus.Pending);
                    }
                }

                await _countryRepository.AddCountryApprovalNotification((long)country.CreatedBy!, country.CountryName!);
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }            
        }

        public async Task<string> SaveCountryFilesAsync(int countryId, List<string> fileNames, int createdBy)
        {
            return await _countryRepository.SaveFileNamesAsync(countryId, fileNames, createdBy);
        }

        public async Task<States> PostState(States states)
        {
            var result = await _countryRepository.PostState(states);
            AccessModel access = new AccessModel() { CreatedBy = states.CreatedBy, ManagerId = states.ManagerId == 0 ? 1 : states.ManagerId, Status = 0, StateId = result.Id, ReferenceCode = states.StateReferenceCode };
            await _countryRepository.PostUpdateStateApproval(access, RefApprovalStatusU.Pending);
            await _countryRepository.AddStateApprovalNotification(result.CreatedBy.Value, result.StateName);
            return result;
        }

        public async Task<bool> PostCountryApprove(AccessModel access)
        {
            var result = await _countryRepository.PostUpdateCountryApproval(access, RefApprovalStatusU.Approved);
            await _countryRepository.PostUpdateCountryFileApproval(access, Models.Enums.RefApprovalStatus.Approved);
            if (result)
            {
                await AddCountryApprovalSuccessNotification(access.CountryId.Value, RefApprovalStatusU.Approved.ToString());
            }
            return result;
        }

        public async Task<bool> PostCountryReject(AccessModel access)
        {
            var result = await _countryRepository.PostUpdateCountryApproval(access, RefApprovalStatusU.Rejected);
            await _countryRepository.PostUpdateCountryFileApproval(access, Models.Enums.RefApprovalStatus.Rejected);
            if (result)
            {
                await AddCountryApprovalSuccessNotification(access.CountryId.Value, RefApprovalStatusU.Rejected.ToString());
            }
            return result;
        }

        public async Task<bool> PostCountryForward(AccessModel access)
        {
            var result = await _countryRepository.PostUpdateCountryApproval(access, RefApprovalStatusU.Forward);
            if (result)
            {
                await AddCountryApprovalSuccessNotification(access.CountryId.Value, RefApprovalStatusU.Forward.ToString());
            }
            return result;
        }

        /// <see cref="ICountryServices.GetStateById(int, int)"/>
        public async Task<List<States>> GetStateById(int countryId, int userId)
        {
            return await _countryRepository.GetStateById(countryId, userId);
        }

        public async Task<List<CountryStateMappingModel>> GetCountryStatesMapping()
        {
            return await _countryRepository.GetCountryStatesMapping();
        }

        public async Task<CountryStateMappingModel> PostCountryStateMapping(CountryStateMappingModel country)
        {
            var result = await _countryRepository.PostCountryStateMapping(country);
            if (result.ResponseCode != 0)
            {
                AccessModel access = new AccessModel() { CreatedBy = country.CreatedBy, ManagerId = country.ManagerId == 0 ? 1 : country.ManagerId, Status = 0, CountryStateMappingId = result.Id };
                await _countryRepository.PostCountryStateApprovalMapping(access, RefApprovalStatusU.Pending);
            }
            await _countryRepository.AddCountryStateMappingApprovalNotification(result.CreatedBy, result.CountryName, result.StateName);
            return result;
        }

        public async Task<bool> PostStateApprove(AccessModel access)
        {
            var result = await _countryRepository.PostUpdateStateApproval(access, RefApprovalStatusU.Approved);
            if (result)
            {
                await AddStateApprovalSuccessNotification(access.StateId.Value, RefApprovalStatusU.Approved.ToString());
            }
            return result;
        }

        public async Task<bool> PostStateReject(AccessModel access)
        {
            var result = await _countryRepository.PostUpdateStateApproval(access, RefApprovalStatusU.Rejected);
            if (result)
            {
                await AddStateApprovalSuccessNotification(access.StateId.Value, RefApprovalStatusU.Rejected.ToString());
            }
            return result;
        }

        public async Task<bool> PostStateForward(AccessModel access)
        {
            var result = await _countryRepository.PostUpdateStateApproval(access, RefApprovalStatusU.Forward);
            if (result)
            {
                await AddStateApprovalSuccessNotification(access.StateId.Value, RefApprovalStatusU.Forward.ToString());
            }
            return result;
        }

        public async Task<bool> PostCountryStateMappingApprove(AccessModel access)
        {
            var result = await _countryRepository.PostCountryStateApprovalMapping(access, RefApprovalStatusU.Approved);
            if (result)
            {
                await AddCountryStateMappingApprovalSuccessNotification(access.StateId.Value, RefApprovalStatusU.Approved.ToString());
            }
            return result;
        }

        public async Task<bool> PostCountryStateMappingReject(AccessModel access)
        {
            var result = await _countryRepository.PostCountryStateApprovalMapping(access, RefApprovalStatusU.Rejected);
            if (result)
            {
                await AddCountryStateMappingApprovalSuccessNotification(access.StateId.Value, RefApprovalStatusU.Approved.ToString());
            }
            return result;
        }

        public async Task<bool> PostCountryStateMappingForward(AccessModel access)
        {
            var result = await _countryRepository.PostCountryStateApprovalMapping(access, RefApprovalStatusU.Forward);
            if (result)
            {
                await AddCountryStateMappingApprovalSuccessNotification(access.StateId.Value, RefApprovalStatusU.Approved.ToString());
            }
            return result;
        }

        public async Task<CountryStateMappingModel> DeleteCountryStateMapping(CountryStateMappingModel country)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CountryStateApproval>> GetCountryApprovaList(Guid UserUID)
        {
            var coutry = await _countryRepository.GetCountryApprovaList(UserUID);
            var states = await _countryRepository.GetStateApprovaList(UserUID);
            return coutry.Concat(states).ToList();
        }

        public async Task<List<CountryStateApproval>> GetAllCountryStateApproval()
        {
            var coutry = await _countryRepository.GetAllCountryApprovaList();
            var states = await _countryRepository.GetAllStateApprovaList();
            return coutry.Concat(states).ToList();
        }

        public async Task<List<CountryStateApproval>> GetCountryStateMappingApprovaList(Guid UserUID)
        {
            return await _countryRepository.GetCountryStateMappingApprovaList(UserUID);
        }

        public async Task<List<CountryStateApproval>> GetAllCountryStateMappingApprovaList()
        {
            return await _countryRepository.GetAllCountryStateMappingApprovaList();
        }

        ///<see cref="ICountryServices.PostUserCountryMapping(List{UserCountryMappingModel})"/>
        public async Task<bool> PostUserCountryMapping(List<UserCountryMappingModel> userCountryMappings)
        {
            return await _countryRepository.PostUserCountryMapping(userCountryMappings);
        }

        ///<see cref="ICountryServices.PostUserStateMapping(List{UserStateMappingModel})"/>
        public async Task<bool> PostUserStateMapping(List<UserStateMappingModel> userStateMappings)
        {
            return await _countryRepository.PostUserStateMapping(userStateMappings);
        }

        ///<see cref="ICountryServices.GetUserStateMapping(int)"/>
        public async Task<List<UserStateMappingResponse>> GetUserStateMapping(int userId)
        {
            return await _countryRepository.GetUserStateMapping(userId);
        }

        ///<see cref="ICountryServices.GetAllCurrencyCodes()"/>"/>
        public async Task<List<CurrencyCodes>> GetAllCurrencyCodes()
        {
            return await _countryRepository.GetAllCurrencyCodes();
        }

        public async Task<StatusCountModel> GetServiceReqAndBillingDetails(long userId, long countryId)
        {
           return await _countryRepository.GetServiceReqAndBillingDetails(userId, countryId);

        }
        public async Task<StatusCountModel> GetServiceReqAndBillingDetailsByState(long userId, long stateId)
        {
            return await _countryRepository.GetServiceReqAndBillingDetailsByState(userId, stateId);
        }
        public async Task<bool> AddCountryApprovalSuccessNotification(long countryId, string approvalStatus)
        {
            return await _countryRepository.AddCountryApprovalSuccessNotification(countryId, approvalStatus);
        }
        public async Task<bool> AddStateApprovalSuccessNotification(long stateId, string approvalStatus)
        {
            return await _countryRepository.AddStateApprovalSuccessNotification(stateId , approvalStatus);
        }
        public async Task<bool> AddCountryStateMappingApprovalSuccessNotification(long stateId, string approvalStatus)
        {
            return await _countryRepository.AddCountryStateMappingApprovalSuccessNotification(stateId, approvalStatus);
        }
        public async Task<string> GetNextStateCode()
        {
            return await _countryRepository.GetNextStateCode();
        }
        public async Task<string> GetNextCountryCode()
        {
            return await _countryRepository.GetNextCountryCode();
        }


        public Task<string> GetLastCountryReferenceCode()
        {
            return _countryRepository.GetLastCountryReferenceCode();
        }

        public Task<string> GetLastStateReferenceCode()
        {
            return _countryRepository.GetLastStateReferenceCode();
        }
    }
}