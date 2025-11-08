using ComplianceAPI.Helpers;
using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Dapper.SqlMapper;
using Organization = ComplianceAPI.Models.DataModels.Organization;

namespace ComplianceAPI.Repository
{
    public interface IOrganizationRepository
    {
        Task<List<BillingLevel>> GetAllBillingLevel();

        Task<List<ProductType>> GetAllProductType();

        Task<List<OrganizationDetail>> GetAllOrganization(int? user);
        Task<List<OrganizationDetail>> GetAllOrganizations();

        Task<OrganizationDetail> GetOrganizationById(long Id);

        Task<List<OrganizationDetail>> GetAllOrganizationWithoutStatus(int? user);

        Task<BillingLevel> GetBillingLevelById(int id);

        Task<List<OrganizationApprovalList>> GetOrganizationApprovalList(string userUID);

        Task<PostOrganization> PostOrganization(PostOrganization organization);

        Task<bool> PostUpdateOrganizationApproval(AccessModel access, string ApprovalStatus);

        Task<List<OrganizationEntityList>> GetOrgEntityList(long? userId);

        Task<List<OrganizationEntityList>> GetOrgEntityLists();

        Task<List<OrganizationDetail>> GetUserOrganizationsByUserId(long userId);
        Task<List<OrganizationDetail>> GetOrganizationsByCountryId(long countryId);
        Task<List<OrganizationDetail>> GetOrganizationsByUserandCountry(long userId, long countryId);

    }

    public class OrganizationRepository : IOrganizationRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly ComplianceDbContext dbContext;
        private readonly IEmail email;
        private readonly IHelperRepository helperRepository;

        public OrganizationRepository(IUnitOfWork unitOfWork, ComplianceDbContext dbContext, IEmail email, IHelperRepository helperRepository)
        {
            _unitOfWork = unitOfWork;
            this.dbContext = dbContext;
            this.email = email;
            this.helperRepository = helperRepository;
        }

        public async Task<List<BillingLevel>> GetAllBillingLevel()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[organizations].USP_GETALLBILLINGLEVEL", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<BillingLevel>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<List<ProductType>> GetAllProductType()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[organizations].USP_GETALLPRODUCTTYPES", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<ProductType>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<List<OrganizationDetail>> GetAllOrganization(int? user)
        {
            try
            {
                // Get organization IDs the user has access to
                var userOrgIds = await dbContext.UserOrganizations
                    .Select(uo => uo.OrganizationId)
                    .ToListAsync();

                // Get organization details
                var organizations = await dbContext.Organizations
                    .Where(o => userOrgIds.Contains(o.Id))
                    .Select(o => new OrganizationDetail
                    {
                        Id = o.Id,
                        OrganizationName = o.OrganizationName,
                        CountryId = o.CountryId,
                        CountryDDId = o.CountryDDId,
                        StateId = o.StateId,
                        City = o.City,
                        Address = o.Address,
                        Pin = o.Pin
                    })
                    .ToListAsync();

                // Get all entities for these organizations
                var orgIds = organizations.Select(o => o.Id).ToList();
                var entities = await dbContext.Entity
                    .Where(e => orgIds.Contains(e.OrganizationId ?? 0))
                    .ToListAsync();

                var entityIds = entities.Select(e => e.Id).ToList();

                // Get billing details and service requests for these entities
                var billingDetailsEntityIds = await dbContext.BillingDetails
                    .Where(b => b.EntityId.HasValue && entityIds.Contains(b.EntityId.Value))
                    .Select(b => b.EntityId.Value)
                    .Distinct()
                    .ToListAsync();

                var serviceRequestEntityIds = await dbContext.ServiceRequests
                    .Where(sr => entityIds.Contains(sr.EntityId))
                    .Select(sr => sr.EntityId)
                    .Distinct()
                    .ToListAsync();

                // Map entities with HasBillingDetails and HasServiceRequests
                var entityMap = entities
                    .GroupBy(e => e.OrganizationId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => new ComplianceAPI.Models.Entity
                        {
                            Id = e.Id,
                            EntityName = e.EntityName,
                            OrganizationId = e.OrganizationId,
                            OrganizationName = null,
                            MajorIndustryId = e.MajorIndustryId,
                            MinorIndustryId = e.MinorIndustryId,
                            City = e.City,
                            Address = e.Address,
                            Pin = e.Pin,
                            Status = e.Status,
                            CreatedOn = e.CreatedOn,
                            ModifiedOn = e.ModifiedOn,
                            HasBillingDetails = billingDetailsEntityIds.Contains(e.Id),
                            HasServiceRequests = serviceRequestEntityIds.Contains(e.Id)
                        }).ToList()
                    );

                // Attach entities to each organization
                foreach (var org in organizations)
                {
                    org.Entities = entityMap.TryGetValue(org.Id, out var orgEntities) ? orgEntities : new List<ComplianceAPI.Models.Entity>();
                }

                return organizations;
            }
            catch
            {
                return new List<OrganizationDetail>();
            }
        }

        public async Task<List<OrganizationDetail>> GetAllOrganizations()
        {
            try
            {
                // Get organization details
                var organizations = await dbContext.Organizations
                    .Select(o => new OrganizationDetail
                    {
                        Id = o.Id,
                        OrganizationName = o.OrganizationName,
                        CountryId = o.CountryId,
                        CountryDDId = o.CountryDDId,
                        StateId = o.StateId,
                        StateName = null, 
                        City = o.City,
                        Address = o.Address,
                        Pin = o.Pin,
                        TypeOfProduct = o.TypeOfProduct,
                        NumberOfEntities = o.NumberOfEntities,
                        NumberOfUsers = o.NumberOfUsers,
                        Status = o.Status,
                        CreatedOn = o.CreatedOn,
                        CreatedBy = o.CreatedBy,
                        ModifiedOn = o.ModifiedOn,
                        ModifiedBy = o.ModifiedBy,
                        ManagerId = o.ManagerId,
                        EntityTypeId = o.EntityTypeId,
                        PrimaryEntity = o.PrimaryEntity,
                        MajorIndustryId = o.MajorIndustryId,
                        MinorIndustryId = o.MinorIndustryId,
                        BillingLevelId = o.BillingLevelId,
                        BillingLevel = null,
                        FullName = o.FullName,
                        EmailID = o.EmailID,
                        Designation = o.Designation,
                        Password = o.Password,
                        FinancialYearStart = o.FinancialYearStart,
                        FinancialYearEnd = o.FinancialYearEnd,
                        FromMonth = o.FromMonth,
                        ToMonth = o.ToMonth,
                        NoofBranches = o.NoofBranches,
                        RoleId = o.RoleId,
                        Entities = null 
                    })
                    .ToListAsync();

                // Get all entities for these organizations
                var orgIds = organizations.Select(o => o.Id).ToList();
                var entities = await dbContext.Entity
                    .Where(e => orgIds.Contains(e.OrganizationId ?? 0))
                    .ToListAsync();

                var entityIds = entities.Select(e => e.Id).ToList();

                // Get billing details and service requests for these entities
                var billingDetailsEntityIds = await dbContext.BillingDetails
                    .Where(b => b.EntityId.HasValue && entityIds.Contains(b.EntityId.Value))
                    .Select(b => b.EntityId.Value)
                    .Distinct()
                    .ToListAsync();

                var serviceRequestEntityIds = await dbContext.ServiceRequests
                    .Where(sr => entityIds.Contains(sr.EntityId))
                    .Select(sr => sr.EntityId)
                    .Distinct()
                    .ToListAsync();

                // Map entities with HasBillingDetails and HasServiceRequests
                var entityMap = entities
                    .GroupBy(e => e.OrganizationId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => new ComplianceAPI.Models.Entity
                        {
                            Id = e.Id,
                            EntityName = e.EntityName,
                            OrganizationId = e.OrganizationId,
                            OrganizationName = null,
                            MajorIndustryId = e.MajorIndustryId,
                            MinorIndustryId = e.MinorIndustryId,
                            City = e.City,
                            Address = e.Address,
                            Pin = e.Pin,
                            Status = e.Status,
                            CreatedOn = e.CreatedOn,
                            ModifiedOn = e.ModifiedOn,
                            HasBillingDetails = billingDetailsEntityIds.Contains(e.Id),
                            HasServiceRequests = serviceRequestEntityIds.Contains(e.Id)
                        }).ToList()
                    );

                // Attach entities to each organization
                foreach (var org in organizations)
                {
                    org.Entities = entityMap.TryGetValue(org.Id, out var orgEntities) ? orgEntities : new List<ComplianceAPI.Models.Entity>();
                }

                return organizations;
            }
            catch
            {
                return new List<OrganizationDetail>();
            }
        }


        public async Task<OrganizationDetail> GetOrganizationById(long Id)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var parameters = new { Id = Id };
                var mul = await connection.QueryMultipleAsync("[organizations].[USP_GETORGANIZATIONBYID]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<OrganizationDetail>().ToList().FirstOrDefault();
                mul.Dispose();
                return result;
            }
        }

        public async Task<List<OrganizationDetail>> GetAllOrganizationWithoutStatus(int? user)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var parameters = new { userId = user };
                var mul = await connection.QueryMultipleAsync("[organizations].USP_GETALLORGANIZATIONWITHOUTSTATUS", parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<OrganizationDetail>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<List<OrganizationApprovalList>> GetOrganizationApprovalList(string userUID)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var parameters = new { UserUID = userUID };
                var mul = await connection.QueryMultipleAsync("[organizations].USP_GETORGANIZATIONAPPROVALLIST", parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<OrganizationApprovalList>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<BillingLevel> GetBillingLevelById(int id)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var parameters = new { Id = id };
                var result = await connection.QuerySingleOrDefaultAsync<BillingLevel>("[organizations].USP_GETBILLINGLEVELBYID", parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString().Trim();
        }

        public async Task<PostOrganization> PostOrganization(PostOrganization organization)
        {
            //Get Approval StatusId
            var approvedStatusId = Convert.ToByte(dbContext.RefApprovalStatus
           .Where(s => s.Status == RefApprovalStatusU.Approved.ToString())
           .Select(s => s.Id)
           .FirstOrDefault());

            //Get Pending StatusId
            var pendingStatusId = Convert.ToByte(dbContext.RefApprovalStatus
           .Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
           .Select(s => s.Id)
           .FirstOrDefault());

            var roleId = dbContext.UserRoleMapping
                .Where(mapping => mapping.UserId == organization.CreatedBy)
                .Select(mapping => mapping.RoleId)
                .FirstOrDefault();

            var existingOrganization = dbContext.Organizations.Where(org => org.Id == organization.Id).FirstOrDefault();
            var managerId = dbContext.Users.Where(manager => manager.Id == organization.ManagerId).FirstOrDefault();
            var existingOrganizationHistory = new OrganizationHistory();
            try
            {
                existingOrganizationHistory = dbContext.OrganizationHistory.Where(org => org.OrganizationName == organization.OrganizationName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string pwd = CreatePassword(8);
            if (existingOrganization != null)
            {
                var existingApproval = dbContext.OrganizationApproval.Where(org => org.OrganizationId == existingOrganization.Id).FirstOrDefault();
                var existingEntity = dbContext.Entity.Where(e => e.OrganizationId == organization.Id).FirstOrDefault();
                // Update existing organization
                existingOrganization.OrganizationName = organization.OrganizationName;
                existingOrganization.Id = organization.Id;
                existingOrganization.PrimaryEntity = organization.PrimaryEntity;
                existingOrganization.EntityTypeId = organization.EntityType;
                existingOrganization.CountryId = organization.CountryId;
                existingOrganization.CountryDDId = organization.CountryDDId;
                existingOrganization.StateId = organization.StateId;
                existingOrganization.City = organization.City;
                existingOrganization.Address = organization.Address;
                existingOrganization.Pin = organization.Pin;
                existingOrganization.TypeOfProduct = organization.TypeOfProduct;
                existingOrganization.NumberOfEntities = organization.NumberOfEntities;
                existingOrganization.NumberOfUsers = organization.NumberOfUsers;
                existingOrganization.Status = roleId == 1 ? 2 : pendingStatusId;
                existingOrganization.ModifiedBy = organization.ModifiedBy;
                existingOrganization.ModifiedOn = DateTime.Now;
                existingOrganization.MajorIndustryId = organization.MajorIndustry;
                existingOrganization.MinorIndustryId = organization.MinorIndustry;
                existingOrganization.BillingLevelId = organization.BillingLevelId;
                existingOrganization.FullName = organization.FullName;
                existingOrganization.EmailID = organization.EmailID;
                existingOrganization.Designation = organization.Designation;
                existingOrganization.Password = organization.Password;
                existingOrganization.FinancialYearStart = organization.FinancialYearStart;
                existingOrganization.FinancialYearEnd = organization.FinancialYearEnd;
                existingOrganization.FromMonth = organization.FromMonth;
                existingOrganization.ToMonth = organization.ToMonth;
                existingOrganization.NoofBranches = organization.NoofBranches;
                existingOrganization.RoleId = organization.RoleId;

                existingOrganizationHistory.OrganizationName = organization.OrganizationName;
                existingOrganizationHistory.Id = organization.Id;
                existingOrganizationHistory.PrimaryEntity = organization.PrimaryEntity;
                existingOrganizationHistory.EntityTypeId = organization.EntityType;
                existingOrganizationHistory.CountryId = organization.CountryId;
                existingOrganizationHistory.CountryDDId = organization.CountryDDId;
                existingOrganizationHistory.StateId = organization.StateId;
                existingOrganizationHistory.City = organization.City;
                existingOrganizationHistory.Address = organization.Address;
                existingOrganizationHistory.Pin = organization.Pin;
                existingOrganizationHistory.TypeOfProduct = organization.TypeOfProduct;
                existingOrganizationHistory.NumberOfEntities = organization.NumberOfEntities;
                existingOrganizationHistory.NumberOfUsers = organization.NumberOfUsers;
                existingOrganizationHistory.Status = roleId == 1 ? 2 : pendingStatusId;
                existingOrganizationHistory.ModifiedBy = organization.ModifiedBy;
                existingOrganizationHistory.ModifiedOn = DateTime.Now;
                existingOrganizationHistory.MajorIndustryId = organization.MajorIndustry;
                existingOrganizationHistory.MinorIndustryId = organization.MinorIndustry;
                existingOrganizationHistory.BillingLevelId = organization.BillingLevelId;
                existingOrganizationHistory.FullName = organization.FullName;
                existingOrganizationHistory.EmailID = organization.EmailID;
                existingOrganizationHistory.Designation = organization.Designation;
                existingOrganizationHistory.Password = organization.Password;
                existingOrganizationHistory.FinancialYearStart = organization.FinancialYearStart;
                existingOrganizationHistory.FinancialYearEnd = organization.FinancialYearEnd;
                existingOrganizationHistory.FromMonth = organization.FromMonth;
                existingOrganizationHistory.ToMonth = organization.ToMonth;
                existingOrganizationHistory.NoofBranches = organization.NoofBranches;
                existingOrganizationHistory.RoleId = organization.RoleId;

                if (existingApproval != null)
                {
                    existingApproval.ApprovalStatus = roleId == 1 ? 2 : pendingStatusId;
                    existingApproval.ModifiedBy = organization.ModifiedBy;
                    existingApproval.ModifiedOn = DateTime.Now;
                    dbContext.OrganizationApproval.Update(existingApproval);
                }

                // Update other properties as needed
                dbContext.Organizations.Update(existingOrganization);
                dbContext.OrganizationHistory.Update(existingOrganizationHistory);
                dbContext.SaveChanges();
                if (existingEntity != null)
                {
                    existingEntity.EntityName = organization.PrimaryEntity;
                    existingEntity.OrganizationId = organization.Id;
                    existingEntity.EntityTypeId = organization.EntityType;
                    existingEntity.CountryId = organization.CountryDDId;
                    existingEntity.StateId = organization.StateId;
                    existingEntity.City = organization.City;
                    existingEntity.Address = organization.Address;
                    existingEntity.Pin = organization.Pin;
                    existingEntity.MajorIndustryId = organization.MajorIndustry;
                    existingEntity.MinorIndustryId = organization.MinorIndustry;
                    existingEntity.FinancialYearStart = organization?.FinancialYearStart;
                    existingEntity.FinancialYearEnd = organization?.FinancialYearEnd;
                    existingEntity.FromMonth = organization?.FromMonth;
                    existingEntity.ToMonth = organization?.ToMonth;
                    existingEntity.ModifiedBy = organization.ModifiedBy;
                    existingEntity.ModifiedOn = DateTime.Now;
                    dbContext.Entity.Update(existingEntity);
                    dbContext.SaveChanges();
                }

                var result = dbContext.Organizations
                    .Where(u => u.Id == existingOrganization.Id)
                    .Select(u => new PostOrganization
                    {
                        Id = u.Id,
                        OrganizationName = existingOrganization.OrganizationName,
                        EntityType = existingOrganization.EntityTypeId,
                        PrimaryEntity = existingOrganization.PrimaryEntity,
                        CountryId = existingOrganization.CountryId,
                        CountryDDId = existingOrganization.CountryDDId,
                        StateId = existingOrganization.StateId,
                        City = existingOrganization.City,
                        Address = existingOrganization.Address,
                        Pin = existingOrganization.Pin,
                        TypeOfProduct = existingOrganization.TypeOfProduct,
                        NumberOfEntities = existingOrganization.NumberOfEntities,
                        NumberOfUsers = existingOrganization.NumberOfUsers,
                        Status = existingOrganization.Status,
                        CreatedOn = existingOrganization.CreatedOn,
                        CreatedBy = existingOrganization.CreatedBy,
                        MajorIndustry = existingOrganization.MajorIndustryId,
                        MinorIndustry = existingOrganization.MinorIndustryId,
                        BillingLevelId = existingOrganization.BillingLevelId,
                        ManagerId = existingOrganization.ManagerId,
                        FullName = organization.FullName,
                        EmailID = organization.EmailID,
                        Designation = organization.Designation,
                        Password = organization.Password,
                        FinancialYearStart = organization.FinancialYearStart,
                        FinancialYearEnd = organization.FinancialYearEnd,
                        FromMonth = organization.FromMonth,
                        ToMonth = organization.ToMonth,
                        NoofBranches = organization.NoofBranches,
                        RoleId = organization.RoleId,
                    })
                    .FirstOrDefault();
                return result;
            }
            else
            {
                ComplianceAPI.Models.DataModels.Organization organizationobject = null;

                var generatedUID = Guid.NewGuid();

                organizationobject = new ComplianceAPI.Models.DataModels.Organization
                {
                    OrganizationName = organization.OrganizationName,
                    EntityTypeId = organization.EntityType,
                    PrimaryEntity = organization.PrimaryEntity,
                    CountryId = organization.CountryId,
                    CountryDDId = organization.CountryDDId,
                    StateId = organization.StateId,
                    City = organization.City,
                    Address = organization.Address,
                    Pin = organization.Pin,
                    TypeOfProduct = organization.TypeOfProduct,
                    NumberOfEntities = organization.NumberOfEntities,
                    NumberOfUsers = organization.NumberOfUsers,
                    Status = roleId == 1 ? 2 : pendingStatusId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = organization.CreatedBy,
                    MajorIndustryId = organization.MajorIndustry,
                    MinorIndustryId = organization.MinorIndustry,
                    BillingLevelId = organization.BillingLevelId,
                    ManagerId = organization.ManagerId,
                    FullName = organization.FullName,
                    EmailID = organization.EmailID,
                    Designation = organization.Designation,
                    Password = pwd, // organization.Password,
                    FinancialYearStart = organization.FinancialYearStart,
                    FinancialYearEnd = organization.FinancialYearEnd,
                    FromMonth = organization.FromMonth,
                    ToMonth = organization.ToMonth,
                    NoofBranches = organization.NoofBranches,
                    RoleId = organization.RoleId,
                };

                dbContext.Organizations.Add(organizationobject);

                dbContext.SaveChanges();

                // Create an OrganizationHistory object
                var organizationHistoryObject = new ComplianceAPI.Models.DataModels.OrganizationHistory
                {
                    OrganizationName = organizationobject.OrganizationName,
                    Id = organizationobject.Id,
                    PrimaryEntity = organizationobject.PrimaryEntity,
                    EntityTypeId = organizationobject.EntityTypeId,
                    CountryId = organizationobject.CountryId,
                    CountryDDId = organizationobject.CountryDDId,
                    StateId = organizationobject.StateId,
                    City = organizationobject.City,
                    Address = organizationobject.Address,
                    Pin = organizationobject.Pin,
                    TypeOfProduct = organizationobject.TypeOfProduct,
                    NumberOfEntities = organizationobject.NumberOfEntities,
                    NumberOfUsers = organizationobject.NumberOfUsers,
                    Status = roleId == 1 ? 2 : pendingStatusId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = organizationobject.CreatedBy,
                    MajorIndustryId = organizationobject.MajorIndustryId,
                    MinorIndustryId = organizationobject.MinorIndustryId,
                    BillingLevelId = organizationobject.BillingLevelId,
                    ManagerId = organizationobject.ManagerId,
                    UID = Guid.NewGuid(), // Generate new GUID for history record
                    FullName = organizationobject.FullName,
                    EmailID = organizationobject.EmailID,
                    Designation = organizationobject.Designation,
                    Password = pwd, // organizationobject.Password,
                    FinancialYearStart = organization.FinancialYearStart,
                    FinancialYearEnd = organization.FinancialYearEnd,
                    FromMonth = organization.FromMonth,
                    ToMonth = organization.ToMonth,
                    NoofBranches = organizationobject.NoofBranches,
                    RoleId = organizationobject.RoleId,
                };

                // Add OrganizationHistory to DbContext and save changes
                dbContext.OrganizationHistory.Add(organizationHistoryObject);
                dbContext.SaveChanges();

                var organizationHistory = dbContext.OrganizationHistory.Where(org => org.OrganizationName == organization.OrganizationName).FirstOrDefault();

                //Insert Into Approval
                ComplianceAPI.Models.DataModels.OrganizationApproval organizationAppobject = null;
                organizationAppobject = new ComplianceAPI.Models.DataModels.OrganizationApproval
                {
                    ApprovalStatus = roleId == 1 ? 2 : pendingStatusId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = organization.CreatedBy,
                    ManagerId = Convert.ToInt64(organization.ManagerId),
                    OrganizationId = organizationHistory.HistoryId,
                    Id = organization.Id,
                    UID = generatedUID
                };
                dbContext.OrganizationApproval.Add(organizationAppobject);

                dbContext.SaveChanges();

                await AddOrganizationApprovalNotification(organization.CreatedBy, organization.OrganizationName, roleId);

                var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)organization.CreatedBy);
                if (isSuperAdmin)
                {
                    email.Sendemail(organization.EmailID, organization.FullName, organization.EmailID, pwd);
                }
                // Insert into Entity table
                ComplianceAPI.Models.DataModels.Entity entityobject = null;

                entityobject = new ComplianceAPI.Models.DataModels.Entity
                {
                    EntityName = organization.PrimaryEntity,
                    OrganizationId = organizationobject.Id,
                    EntityTypeId = organization.EntityType,
                    CountryId = organization.CountryDDId,
                    StateId = organization.StateId,
                    City = organization.City,
                    Address = organization.Address,
                    Pin = organization.Pin,
                    Status = roleId == 1 ? 2 : pendingStatusId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = organization.CreatedBy,
                    MajorIndustryId = organization.MajorIndustry,
                    MinorIndustryId = organization.MinorIndustry,
                    ManagerId = organization.ManagerId,
                    UID = organization?.UID,
                    FinancialYearStart = organization?.FinancialYearStart,
                    FinancialYearEnd = organization?.FinancialYearEnd,
                    FromMonth = organization?.FromMonth,
                    ToMonth = organization?.ToMonth
                };

                dbContext.Entity.Add(entityobject);

                dbContext.SaveChanges();

                // Create an EntityHistory object
                var entityHistoryObject = new ComplianceAPI.Models.DataModels.EntityHistory
                {
                    EntityName = organizationobject.PrimaryEntity,
                    Id = entityobject.Id,
                    EntityTypeId = organizationobject.EntityTypeId,
                    OrganizationId = organizationobject.Id,
                    CountryId = organizationobject.CountryDDId,
                    StateId = organizationobject.StateId,
                    City = organizationobject.City,
                    Address = organizationobject.Address,
                    Pin = organizationobject.Pin,
                    Status = roleId == 1 ? 2 : 1,
                    CreatedOn = DateTime.Now,
                    CreatedBy = organizationobject.CreatedBy,
                    MajorIndustryId = organizationobject.MajorIndustryId,
                    MinorIndustryId = organizationobject.MinorIndustryId,
                    ManagerId = organizationobject.ManagerId,
                    UID = Guid.NewGuid(),
                    FinancialYearStart = organizationobject?.FinancialYearStart,
                    FinancialYearEnd = organizationobject?.FinancialYearEnd,
                    FromMonth = organizationobject?.FromMonth,
                    ToMonth = organizationobject?.ToMonth
                };
                // Add history to DbContext and save changes
                dbContext.EntityHistory.Add(entityHistoryObject);
                dbContext.SaveChanges();

                var entityHistory = dbContext.EntityHistory.Where(ent => ent.EntityName == organization.PrimaryEntity).FirstOrDefault();

                //Insert Into Approval
                var entityAppobject = new ComplianceAPI.Models.DataModels.EntityApproval
                {
                    ApprovalStatus = roleId == 1 ? 2 : pendingStatusId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = organization.CreatedBy,
                    ManagerId = Convert.ToInt64(organization.ManagerId),
                    EntityId = entityHistoryObject.HistoryId,
                    UID = generatedUID
                };
                dbContext.EntityApproval.Add(entityAppobject);
                dbContext.SaveChanges();

                var result = dbContext.OrganizationHistory
                .Where(u => u.Id == organizationobject.Id)
                .Select(u => new PostOrganization
                {
                    Id = u.Id,
                    OrganizationName = organizationobject.OrganizationName,
                    PrimaryEntity = organizationobject.PrimaryEntity,
                    EntityType = organizationobject.EntityTypeId,
                    CountryId = organizationobject.CountryId,
                    CountryDDId = organizationobject.CountryDDId,
                    StateId = organizationobject.StateId,
                    City = organizationobject.City,
                    Address = organizationobject.Address,
                    Pin = organizationobject.Pin,
                    TypeOfProduct = organizationobject.TypeOfProduct,
                    NumberOfEntities = organizationobject.NumberOfEntities,
                    NumberOfUsers = organizationobject.NumberOfUsers,
                    Status = organizationobject.Status,
                    CreatedOn = organizationobject.CreatedOn,
                    CreatedBy = organizationobject.CreatedBy,
                    MajorIndustry = organizationobject.MajorIndustryId,
                    MinorIndustry = organizationobject.MinorIndustryId,
                    BillingLevelId = organizationobject.BillingLevelId,
                    ManagerId = organizationobject.ManagerId,
                    FullName = organizationobject.FullName,
                    EmailID = organizationobject.EmailID,
                    Designation = organizationobject.Designation,
                    Password = organizationobject.Password,
                    FinancialYearStart = organization.FinancialYearStart,
                    FinancialYearEnd = organization.FinancialYearEnd,
                    FromMonth = organization.FromMonth,
                    ToMonth = organization.ToMonth,
                    NoofBranches = organizationobject.NoofBranches,
                    RoleId = organizationobject.RoleId,
                })
                .FirstOrDefault();
                return result;
            }
        }

        public async Task<bool> PostUpdateOrganizationApproval(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, OrganizationId = access.OrganizationId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[organizations].USP_UPDATEORGANIZATIONAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();

                    var org = dbContext.Organizations.FirstOrDefault(f => f.Id == access.Id);
                    if (org != null)
                    {
                        email.Sendemail(org.EmailID, org.FullName, org.EmailID, org.Password);
                    }
                    if (ApprovalStatus != "Pending" && access.UID != null)
                    {
                        await OrganizationApprovalResponseNotification(access.UID.Value, ApprovalStatus);
                    }
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }

            return true;
        }

        public async Task<List<OrganizationEntityList>> GetOrgEntityLists()
        {
            try
            {
                List<Organization> organizations;

                organizations = await dbContext.Organizations.Where(a => a.Status == 2).ToListAsync();

                var entities = await dbContext.Entity.ToListAsync();

                List<EntityList> GetEntities(long orgId)
                {
                    return entities
                        .Where(c => c.OrganizationId == orgId && c.Status == 2)
                        .Select(c => new EntityList
                        {
                            EntityId = c.Id,
                            EntityName = c.EntityName
                        }).ToList();
                }

                var result = organizations.Select(r => new OrganizationEntityList
                {
                    Id = r.Id,
                    OrganizationName = r.OrganizationName,
                    IsOrganization = r.BillingLevelId == 1 ? true : false,
                    EntityList = GetEntities(r.Id)
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<OrganizationEntityList>();
            }
        }

        public async Task<List<OrganizationEntityList>> GetOrgEntityList(long? userId)
        {
            try
            {
                List<Organization> organizations;
                if (userId.HasValue)
                {
                    // Get organization IDs for the user
                    var userOrgIds = await dbContext.UserOrganizations
                        .Select(uo => uo.OrganizationId)
                        .ToListAsync();

                    organizations = await dbContext.Organizations
                        .Where(a => a.Status == 2 && userOrgIds.Contains(a.Id))
                        .ToListAsync();
                }
                else
                {
                    organizations = await dbContext.Organizations.Where(a => a.Status == 2).ToListAsync();
                }

                organizations = await dbContext.Organizations.Where(a => a.Status == 2).ToListAsync();

                var entities = await dbContext.Entity.ToListAsync();

                List<EntityList> GetEntities(long orgId)
                {
                    return entities
                        .Where(c => c.OrganizationId == orgId && c.Status == 2)
                        .Select(c => new EntityList
                        {
                            EntityId = c.Id,
                            EntityName = c.EntityName
                        }).ToList();
                }

                var result = organizations.Select(r => new OrganizationEntityList
                {
                    Id = r.Id,
                    OrganizationName = r.OrganizationName,
                    IsOrganization = r.BillingLevelId == 1 ? true : false,
                    EntityList = GetEntities(r.Id)
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<OrganizationEntityList>();
            }
        }

        public bool IsSuperAdmin(int userId)
        {
            var isSuperAdmin = (from user in dbContext.Users
                                join userRoleMapping in dbContext.UserRoleMapping on user.Id equals userRoleMapping.UserId
                                join refRole in dbContext.RefRoles on userRoleMapping.RoleId equals Convert.ToInt32(refRole.Id)
                                where user.Id == userId && (refRole.RoleName == "SuperAdmin" || refRole.RoleName == "ITSupportAdmin")
                                select user).Any();

            return isSuperAdmin;
        }

        public async Task<List<OrganizationDetail>> GetUserOrganizationsByUserId(long userId)
        {
            try
            {
                // Get organization IDs the user has access to
                var userOrgIds = await dbContext.UserOrganizations
                    .Where(uo => uo.UserId == userId)
                    .Select(uo => uo.OrganizationId)
                    .ToListAsync();

                // Get organization details
                var organizations = await dbContext.Organizations
                    .Where(o => userOrgIds.Contains(o.Id))
                    .Select(o => new OrganizationDetail
                    {
                        Id = o.Id,
                        OrganizationName = o.OrganizationName,
                        CountryId = o.CountryId,
                        CountryDDId = o.CountryDDId,
                        StateId = o.StateId,
                        City = o.City,
                        Address = o.Address,
                        Pin = o.Pin
                    })
                    .ToListAsync();

                // Get all entities for these organizations
                var orgIds = organizations.Select(o => o.Id).ToList();
                var entities = await dbContext.Entity
                    .Where(e => orgIds.Contains(e.OrganizationId ?? 0))
                    .ToListAsync();

                var entityIds = entities.Select(e => e.Id).ToList();

                // Get billing details and service requests for these entities
                var billingDetailsEntityIds = await dbContext.BillingDetails
                    .Where(b => b.EntityId.HasValue && entityIds.Contains(b.EntityId.Value))
                    .Select(b => b.EntityId.Value)
                    .Distinct()
                    .ToListAsync();

                var serviceRequestEntityIds = await dbContext.ServiceRequests
                    .Where(sr => entityIds.Contains(sr.EntityId))
                    .Select(sr => sr.EntityId)
                    .Distinct()
                    .ToListAsync();

                // Map entities with HasBillingDetails and HasServiceRequests
                var entityMap = entities
                    .GroupBy(e => e.OrganizationId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => new ComplianceAPI.Models.Entity
                        {
                            Id = e.Id,
                            EntityName = e.EntityName,
                            OrganizationId = e.OrganizationId,
                            OrganizationName = null,
                            MajorIndustryId = e.MajorIndustryId,
                            MinorIndustryId = e.MinorIndustryId,
                            City = e.City,
                            Address = e.Address,
                            Pin = e.Pin,
                            Status = e.Status,
                            CreatedOn = e.CreatedOn,
                            ModifiedOn = e.ModifiedOn,
                            HasBillingDetails = billingDetailsEntityIds.Contains(e.Id),
                            HasServiceRequests = serviceRequestEntityIds.Contains(e.Id)
                        }).ToList()
                    );

                // Attach entities to each organization
                foreach (var org in organizations)
                {
                    org.Entities = entityMap.TryGetValue(org.Id, out var orgEntities) ? orgEntities : new List<ComplianceAPI.Models.Entity>();
                }

                return organizations;
            }
            catch
            {
                return new List<OrganizationDetail>();
            }
        }

        public async Task<List<OrganizationDetail>> GetOrganizationsByCountryId(long countryId)
        {
            try
            {
                string countryIdStr = countryId.ToString();
                string patternStart = countryIdStr + ",%";
                string patternMiddle = "%," + countryIdStr + ",%";
                string patternEnd = "%," + countryIdStr;
                string patternExact = countryIdStr;

                var listOfOrganizations = await dbContext.Organizations
                    .Where(o =>
                        EF.Functions.Like(o.CountryId, patternStart) ||
                        EF.Functions.Like(o.CountryId, patternMiddle) ||
                        EF.Functions.Like(o.CountryId, patternEnd) ||
                        o.CountryId == patternExact
                    )
                    .Select(o => new OrganizationDetail
                    {
                        Id = o.Id,
                        OrganizationName = o.OrganizationName,
                        CountryId = o.CountryId,
                        CountryDDId = o.CountryDDId,
                        StateId = o.StateId,
                        City = o.City,
                        Address = o.Address
                    })
                    .ToListAsync();

                return listOfOrganizations;
            }
            catch (Exception ex)
            {
                return new List<OrganizationDetail>();
            }
        }

        public async Task<List<OrganizationDetail>> GetOrganizationsByUserandCountry(long userId, long countryId)
        {
            // Prepare countryId patterns for LIKE search
            string countryIdStr = countryId.ToString();
            string patternStart = countryIdStr + ",%";
            string patternMiddle = "%," + countryIdStr + ",%";
            string patternEnd = "%," + countryIdStr;
            string patternExact = countryIdStr;

            // Get organization IDs the user has access to
            var userOrgIds = await dbContext.UserOrganizations
                .Where(uo => uo.UserId == userId)
                .Select(uo => uo.OrganizationId)
                .ToListAsync();

            // Filter organizations by both user access and country
            var listOfOrganizations = await dbContext.Organizations
                .Where(o =>
                    userOrgIds.Contains(o.Id) &&
                    (
                        EF.Functions.Like(o.CountryId, patternStart) ||
                        EF.Functions.Like(o.CountryId, patternMiddle) ||
                        EF.Functions.Like(o.CountryId, patternEnd) ||
                        o.CountryId == patternExact
                    )
                )
                .Select(o => new OrganizationDetail
                {
                    Id = o.Id,
                    OrganizationName = o.OrganizationName,
                    CountryId = o.CountryId,
                    CountryDDId = o.CountryDDId,
                    StateId = o.StateId,
                    City = o.City,
                    Address = o.Address
                })
                .ToListAsync();

            // Get all entities for these organizations
            var orgIds = listOfOrganizations.Select(o => o.Id).ToList();
            var entities = await dbContext.Entity
                .Where(e => orgIds.Contains(e.OrganizationId ?? 0))
                .ToListAsync();

            var entityIds = entities.Select(e => e.Id).ToList();

            // Get billing details and service requests for these entities
            var billingDetailsEntityIds = await dbContext.BillingDetails
                .Where(b => b.EntityId.HasValue && entityIds.Contains(b.EntityId.Value))
                .Select(b => b.EntityId.Value)
                .Distinct()
                .ToListAsync();

            var serviceRequestEntityIds = await dbContext.ServiceRequests
                .Where(sr => entityIds.Contains(sr.EntityId))
                .Select(sr => sr.EntityId)
                .Distinct()
                .ToListAsync();

            // Map entities with HasBillingDetails and HasServiceRequests
            var entityMap = entities
                .GroupBy(e => e.OrganizationId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => new ComplianceAPI.Models.Entity
                    {
                        Id = e.Id,
                        EntityName = e.EntityName,
                        OrganizationId = e.OrganizationId,
                        OrganizationName = null, // Can be set if needed
                        MajorIndustryId = e.MajorIndustryId,
                        MinorIndustryId = e.MinorIndustryId,
                        City = e.City,
                        Address = e.Address,
                        Pin = e.Pin,
                        Status = e.Status,
                        CreatedOn = e.CreatedOn,
                        ModifiedOn = e.ModifiedOn,
                        HasBillingDetails = billingDetailsEntityIds.Contains(e.Id),
                        HasServiceRequests = serviceRequestEntityIds.Contains(e.Id)
                    }).ToList()
                );

            // Attach entities to each organization
            foreach (var org in listOfOrganizations)
            {
                org.Entities = entityMap.TryGetValue(org.Id, out var orgEntities) ? orgEntities : new List<ComplianceAPI.Models.Entity>();
            }

            return listOfOrganizations;
        }
            
        private async Task<bool> AddOrganizationApprovalNotification(long? createdBy, string organizationName, int? createdByRoleId)
        {
            try
            {
                var createdByDetails = await helperRepository.GetUserAndManagerInfoAsync(createdBy.Value);
                if (createdByDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        NotificationTitle=createdByRoleId==1
                        ?
                        string.Format(ApiConstants.NewOrganizationBySuperAdminNotificationTitleTemplate, organizationName)
                        :
                        string.Format(ApiConstants.NewOrganizationNotificationTitleTemplate, createdByDetails.UserName, organizationName),


                        NotificationMessage = createdByRoleId==1
                        ?
                        string.Format(ApiConstants.NewOrganizationBySuperAdminNotificationMessageTemplate, organizationName)
                        :
                        string.Format(ApiConstants.NewOrganizationNotificationMessageTemplate,createdByDetails.UserName, organizationName),
                        
                        SenderUserId = createdByDetails.UserId,
                        SenderUserName = createdByDetails.UserName,
                        RecipientUserId = createdByDetails.ManagerId,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        Status = createdByRoleId==1
                        ?
                        Models.Enums.RefApprovalStatus.Approved
                        :
                        Models.Enums.RefApprovalStatus.Pending,

                        ModuleType = Models.Enums.ModuleType.Organization,
                        RecipientUserName = createdByDetails?.ManagerName,
                        MarkAsRead=false
                    };
                    dbContext.Notifications.Add(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> OrganizationApprovalResponseNotification(Guid uid, string approvalStatus)
        {
            try
            {
                var organizationApprovalDetails = await dbContext.OrganizationApproval.FirstOrDefaultAsync(x => x.UID == uid);
                if (organizationApprovalDetails != null)
                {
                    var userDetails = await helperRepository.GetUserAndManagerInfoAsync(organizationApprovalDetails.CreatedBy.Value);
                    if (userDetails != null)
                    {
                        var notification = new Notification
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            SenderUserId = userDetails.ManagerId,
                            SenderUserName = userDetails.ManagerName,
                            RecipientUserId = userDetails.UserId,
                            RecipientUserName = userDetails.UserName,
                            ModuleType = Models.Enums.ModuleType.Organization,
                            CreatedDate = DateTime.UtcNow,
                            ReadDate = null,
                            MarkAsRead = false
                        };
                        if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Approved;
                            notification.NotificationTitle = $"<b>{userDetails.ManagerName}</b> approved a <b>New Organization</b>";
                            notification.NotificationMessage = $"<b>{userDetails.ManagerName}</b> approved a <b>New Organization</b>";
                        }
                        else if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                            notification.NotificationTitle = $"<b>{userDetails.ManagerName}</b> rejected the <b>New Organization</b>";
                            notification.NotificationMessage = $"<b>{userDetails.ManagerName}</b> rejected the <b>New Organization</b>";
                        }
                        else if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Forward;
                            notification.NotificationTitle = $"<b>{userDetails.ManagerName}</b> forward the <b>New Organization</b>";
                            notification.NotificationMessage = $"<b>{userDetails.ManagerName}</b> forward the <b>New Organization</b>";
                        }
                        await dbContext.AddAsync(notification);
                        await dbContext.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
