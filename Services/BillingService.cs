using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;
using ComplianceAPI.Services;

namespace ComplianceAPI.Services
{
    public interface IBillingService
    {
        Task<List<BillingLevel>> GetAllBillingLevel();
        Task<List<BillingFrequency>> GetAllBillingFrequency();
        Task<List<Models.ServiceProvider>> GetAllServiceProvider();
        Task<List<BillStatus>> GetAllBillStatus();
        Task<List<DeliveryStatus>> GetAllDeliveryStatus();
        Task<bool> PostBillingDetails(PostBillingDetails billingDetails);
        Task<bool> PostBillingApprove(AccessModel access);
        Task<bool> PostBillingReject(AccessModel access);
        Task<bool> PostBillingForward(AccessModel access);
        Task<BillingDetailsView> GetBillingDetailsView(long billingDetailId);
        Task<List<BillingDetailsView>> GetBillingDetailsViewByOrgId(long organizationId);
        Task<List<BillingBasicDetails>> GetBillingDetailsByEntityAsync(long? entityId);
    }
}
public class BillingService : IBillingService
{
    public readonly IBillingRepository _billingRepository;
    public BillingService(IBillingRepository billingRepository)
    {
        this._billingRepository = billingRepository;
    }

    public async Task<List<BillingLevel>> GetAllBillingLevel()
    {
        return await _billingRepository.GetAllBillingLevel();
    }
    public async Task<List<BillingFrequency>> GetAllBillingFrequency()
    {
        return await _billingRepository.GetAllBillingFrequency();
    }
    public async Task<List<ComplianceAPI.Models.ServiceProvider>> GetAllServiceProvider()
    {
        return await _billingRepository.GetAllServiceProvider();
    }
    public async Task<List<BillStatus>> GetAllBillStatus()
    {
        return await _billingRepository.GetAllBillStatus();
    }
    public async Task<List<DeliveryStatus>> GetAllDeliveryStatus()
    {
        return await _billingRepository.GetAllDeliveryStatus();
    }
    public async Task<bool> PostBillingDetails(PostBillingDetails billingDetails)
    {
        return await _billingRepository.PostBillingDetails(billingDetails);
    }

    public async Task<BillingDetailsView> GetBillingDetailsView(long billingDetailId)
    {
        return await _billingRepository.GetBillingDetailsView(billingDetailId);
    }

    public async Task<List<BillingDetailsView>> GetBillingDetailsViewByOrgId(long organizationId)
    {
        return await _billingRepository.GetBillingDetailsViewByOrgId(organizationId);
    }
    public async Task<bool> PostBillingApprove(AccessModel access)
    {
        return await _billingRepository.UpdateBillingApproval(access, RefApprovalStatusU.Approved);
    }

    public async Task<bool> PostBillingReject(AccessModel access)
    {
        return await _billingRepository.UpdateBillingApproval(access, RefApprovalStatusU.Rejected);
    }

    public async Task<bool> PostBillingForward(AccessModel access)
    {
        return await _billingRepository.UpdateBillingApproval(access, RefApprovalStatusU.Forward);
    }
    public async Task<List<BillingBasicDetails>> GetBillingDetailsByEntityAsync(long? entityId)
    {
        return await _billingRepository.GetBillingDetailsByEntityAsync(entityId);
    }
}

