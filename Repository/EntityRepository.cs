using AutoMapper;
using ComplianceAPI.Helpers;
using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ComplianceAPI.Repository
{
    public interface IEntityRepository
    {
        Task<List<EntityApprovalList>> GetEntityApprovalList(int userId);

        Task<List<Models.Entity>> GetAllEntitiesByOrgId(int orgId);

        Task<Models.Entity> GetEntityDetails(int entityId);

        Task<PostEntity> PostEntity(PostEntity entity);

        Task<Models.Entity> GetEntityView(int entityId);

        Task<bool> UpdateEntityApproval(AccessModel access, string ApprovalStatus);

        Task<FinancialMonth> GetStartAndEndMonthsByCountryId(int countryId);
        Task<List<Models.Entity>> GetEntitiesByOrganizationId(long organizationId);
        Task<List<Models.Entity>> GetEntitiesByOrganizationAndCountryId(long organizationId,long countryId);
        Task<List<ComplianceAPI.Models.Entity>> GetClientEntitiesLocations(int organizationId);

    }

    public class EntityRepository : IEntityRepository
    {
        //private readonly IMapper _mapper;
        private readonly IGetCoordinates _getCoordinates;
        protected readonly IUnitOfWork _unitOfWork;
        private readonly ComplianceDbContext dbContext;
        private readonly IHelperRepository _helperRepository;

        public EntityRepository(IUnitOfWork unitOfWork, ComplianceDbContext dbContext, IHelperRepository helperRepository, IGetCoordinates getCoordinates)
        {
            //_mapper = mapper;
            _unitOfWork = unitOfWork;
            this.dbContext = dbContext;
            _helperRepository = helperRepository;
            _getCoordinates = getCoordinates;
        }

        public async Task<List<EntityApprovalList>> GetEntityApprovalList(int userId)
        {
            // 1. Organization Approvals
            var orgApprovals = await (
                from oa in dbContext.OrganizationApproval
                join u in dbContext.Users on oa.ManagerId equals u.Id
                join uk in dbContext.Users on oa.CreatedBy equals uk.Id
                join ras in dbContext.RefApprovalStatus on oa.ApprovalStatus equals ras.Id
                join oh in dbContext.OrganizationHistory on oa.OrganizationId equals oh.HistoryId
                where oa.ManagerId == userId || oa.CreatedBy == userId
                select new EntityApprovalList
                {
                    OrganizationId = oh.Id,
                    EntityId = null,
                    BillingDetilsId = null,
                    Type = "Organization",
                    Name = oh.OrganizationName,
                    CustomerId = u.EmpId,
                    PointOfContact = uk.FullName,
                    StatusId = oa.ApprovalStatus,
                    Status = ras.Status,
                    OnboardingStage = "Add Organization",
                    CreatedBy = (int?)oa.CreatedBy,
                    Approvedby = u.FullName,
                    UID = oa.UID
                }
            ).ToListAsync();

            // 2. Entity Approvals
            var entityApprovals = await (
                from ea in dbContext.EntityApproval
                join u in dbContext.Users on ea.ManagerId equals u.Id
                join uk in dbContext.Users on ea.CreatedBy equals uk.Id
                join ras in dbContext.RefApprovalStatus on ea.ApprovalStatus equals ras.Id
                join eh in dbContext.EntityHistory on ea.EntityId equals eh.Id
                where ea.ManagerId == userId || ea.CreatedBy == userId
                select new EntityApprovalList
                {
                    OrganizationId = eh.OrganizationId,
                    EntityId = eh.Id,
                    BillingDetilsId = null,
                    Type = "Entity",
                    Name = eh.EntityName,
                    CustomerId = u.EmpId,
                    PointOfContact = uk.FullName,
                    StatusId = ea.ApprovalStatus,
                    Status = ras.Status,
                    OnboardingStage = "Add Entity",
                    CreatedBy = (int?)ea.CreatedBy,
                    Approvedby = ras.Status == "Approved" ? u.FullName : "N/A",
                    UID = ea.UID
                }
            ).ToListAsync();

            var allResults = orgApprovals
                .Concat(entityApprovals)
                .OrderByDescending(x => x.CreatedBy)
                .ThenByDescending(x => x.OrganizationId)
                .ThenBy(x => x.EntityId)
                .ToList();

            return allResults;
        }

        public async Task<List<Models.Entity>> GetAllEntitiesByOrgId(int orgId)
        {
            try
            {
                using (var connection = _unitOfWork.ConnectionFactory())
                {
                    var parameters = new { OrgId = orgId };
                    var mul = await connection.QueryMultipleAsync("[organizations].USP_GETENTITYAPPROVALLIST", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    var result = mul.Read<Models.Entity>().ToList();
                    mul.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new List<Models.Entity>();
            }
        }

        public async Task<Models.Entity> GetEntityView(int entityId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var parameters = new { EntityId = entityId };
                var mul = await connection.QueryMultipleAsync("[organizations].[USP_GETENTITYVIEWDETAILS]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<Models.Entity>().FirstOrDefault();
                mul.Dispose();
                return result;
            }
        }

        public async Task<Models.Entity> GetEntityDetails(int entityId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var parameters = new { EntityId = entityId };
                var mul = await connection.QueryMultipleAsync("[organizations].[USP_GETENTITYDETAILS]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<Models.Entity>().FirstOrDefault();
                mul.Dispose();
                return result;
            }
        }

        public async Task<PostEntity> PostEntity(PostEntity entity)
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
                .Where(mapping => mapping.UserId == entity.CreatedBy)
                .Select(mapping => mapping.RoleId)
                .FirstOrDefault();

            var existingEntity = dbContext.Entity.Where(ent => ent.Id == entity.Id).FirstOrDefault();
            var managerId = dbContext.Users.Where(manager => manager.Id == entity.ManagerId).FirstOrDefault();

            if (existingEntity != null)
            {
                var existingApproval = dbContext.EntityApproval.Where(ent => ent.EntityId == entity.Id).FirstOrDefault();
                var existingEntityHistory = dbContext.EntityHistory.Where(ent => ent.EntityName == entity.EntityName).FirstOrDefault();
                // Update existing entity
                existingEntity.EntityName = entity.EntityName;
                existingEntity.Id = entity.Id;
                existingEntity.OrganizationId = entity.OrganizationId;
                existingEntity.EntityTypeId = entity.EntityTypeId;
                existingEntity.CountryId = entity.CountryId;
                existingEntity.StateId = entity.StateId;
                existingEntity.City = entity.City;
                existingEntity.Address = entity.Address;
                existingEntity.Pin = entity.Pin;
                existingEntity.Status = roleId == 1 ? 2 : pendingStatusId;
                existingEntity.ModifiedBy = entity.ModifiedBy;
                existingEntity.ModifiedOn = DateTime.Now;
                existingEntity.MajorIndustryId = entity.MajorIndustry;
                existingEntity.MinorIndustryId = entity.MinorIndustry;
                existingEntity.UID = entity?.UID;
                existingEntity.FinancialYearStart = entity?.FinancialYearStart;
                existingEntity.FinancialYearEnd = entity?.FinancialYearEnd;
                existingEntity.FromMonth = entity?.FromMonth;
                existingEntity.ToMonth = entity?.ToMonth;

                existingEntityHistory.EntityName = entity.EntityName;
                existingEntityHistory.Id = entity.Id;
                existingEntityHistory.OrganizationId = entity.OrganizationId;
                existingEntityHistory.EntityTypeId = entity.EntityTypeId;
                existingEntityHistory.CountryId = entity.CountryId;
                existingEntityHistory.StateId = entity.StateId;
                existingEntityHistory.City = entity.City;
                existingEntityHistory.Address = entity.Address;
                existingEntityHistory.Pin = entity.Pin;
                existingEntityHistory.Status = managerId == null ? 2 : pendingStatusId;
                existingEntityHistory.ModifiedBy = entity.ModifiedBy;
                existingEntityHistory.ModifiedOn = DateTime.Now;
                existingEntityHistory.MajorIndustryId = entity.MajorIndustry;
                existingEntityHistory.MinorIndustryId = entity.MinorIndustry;
                existingEntityHistory.UID = entity?.UID;
                existingEntityHistory.FinancialYearStart = entity?.FinancialYearStart;
                existingEntityHistory.FinancialYearEnd = entity?.FinancialYearEnd;
                existingEntityHistory.FromMonth = entity.FromMonth;
                existingEntityHistory.ToMonth = entity.ToMonth;

                if (existingApproval != null)
                {
                    existingApproval.ApprovalStatus = managerId == null ? 2 : pendingStatusId;
                    existingApproval.ModifiedBy = entity.ModifiedBy;
                    existingApproval.ModifiedOn = DateTime.Now;
                    dbContext.EntityApproval.Update(existingApproval);
                }

                // Update other properties as needed
                dbContext.Entity.Update(existingEntity);
                dbContext.EntityHistory.Update(existingEntityHistory);
                dbContext.SaveChanges();

                var result = dbContext.Entity
                    .Where(u => u.Id == existingEntity.Id)
                    .Select(u => new PostEntity
                    {
                        Id = u.Id,
                        EntityName = existingEntityHistory.EntityName,
                        OrganizationId = existingEntityHistory.OrganizationId,
                        EntityTypeId = existingEntityHistory.EntityTypeId,
                        CountryId = existingEntityHistory.CountryId,
                        StateId = existingEntityHistory.StateId,
                        City = existingEntityHistory.City,
                        Address = existingEntityHistory.Address,
                        Pin = existingEntityHistory.Pin,
                        Status = existingEntityHistory.Status,
                        CreatedOn = existingEntityHistory.CreatedOn,
                        CreatedBy = existingEntityHistory.CreatedBy,
                        MajorIndustry = existingEntityHistory.MajorIndustryId,
                        MinorIndustry = existingEntityHistory.MinorIndustryId,
                        ManagerId = existingEntityHistory.ManagerId,
                        ModifiedBy = existingEntityHistory.ModifiedBy,
                        ModifiedOn = existingEntityHistory.ModifiedOn,
                        FinancialYearStart = existingEntityHistory.FinancialYearStart,
                        FinancialYearEnd = existingEntityHistory.FinancialYearEnd,
                        FromMonth = existingEntityHistory.FromMonth,
                        ToMonth = existingEntityHistory.ToMonth,
                    })
                    .FirstOrDefault();
                return result;
            }
            else
            {
                // Insert into Entity table
                ComplianceAPI.Models.DataModels.Entity entityobject = null;
                var generatedUID = Guid.NewGuid();

                entityobject = new ComplianceAPI.Models.DataModels.Entity
                {
                    EntityName = entity.EntityName,
                    OrganizationId = entity.OrganizationId,
                    EntityTypeId = entity.EntityTypeId,
                    CountryId = entity.CountryId,
                    StateId = entity.StateId,
                    City = entity.City,
                    Address = entity.Address,
                    Pin = entity.Pin,
                    Status = roleId == 1 ? 2 : pendingStatusId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = entity.CreatedBy,
                    MajorIndustryId = entity.MajorIndustry,
                    MinorIndustryId = entity.MinorIndustry,
                    ManagerId = entity.ManagerId,
                    UID = entity?.UID,
                    FinancialYearStart = entity?.FinancialYearStart,
                    FinancialYearEnd = entity?.FinancialYearEnd,
                    FromMonth = entity?.FromMonth,
                    ToMonth = entity?.ToMonth
                };

                dbContext.Entity.Add(entityobject);

                dbContext.SaveChanges();

                // Create an EntityHistory object
                var entityHistoryObject = new ComplianceAPI.Models.DataModels.EntityHistory
                {
                    EntityName = entityobject.EntityName,
                    Id = entityobject.Id,
                    EntityTypeId = entityobject.EntityTypeId,
                    OrganizationId = entityobject.OrganizationId,
                    CountryId = entityobject.CountryId,
                    StateId = entityobject.StateId,
                    City = entityobject.City,
                    Address = entityobject.Address,
                    Pin = entityobject.Pin,
                    Status = roleId == null ? 2 : 1,
                    CreatedOn = DateTime.Now,
                    CreatedBy = entityobject.CreatedBy,
                    MajorIndustryId = entityobject.MajorIndustryId,
                    MinorIndustryId = entityobject.MinorIndustryId,
                    ManagerId = entityobject.ManagerId,
                    UID = Guid.NewGuid(),
                    FinancialYearStart = entityobject?.FinancialYearStart,
                    FinancialYearEnd = entityobject?.FinancialYearEnd,
                    FromMonth = entityobject?.FromMonth,
                    ToMonth = entityobject?.ToMonth
                };
                // Add history to DbContext and save changes
                dbContext.EntityHistory.Add(entityHistoryObject);
                dbContext.SaveChanges();

                var entityHistory = dbContext.EntityHistory.Where(ent => ent.EntityName == entity.EntityName).FirstOrDefault();

                //Insert Into Approval
                var entityAppobject = new ComplianceAPI.Models.DataModels.EntityApproval
                {
                    ApprovalStatus = roleId == 1 ? 2 : pendingStatusId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = entity.CreatedBy,
                    ManagerId = Convert.ToInt64(entity.ManagerId),
                    EntityId = entityobject.Id,
                    Id = entity.Id,
                    UID = generatedUID
                };
                dbContext.EntityApproval.Add(entityAppobject);

                dbContext.SaveChanges();

                await AddEntityApprovalNotification(entity.CreatedBy, entity.EntityName, roleId);

                var result = dbContext.Entity
                    .Where(u => u.Id == entityobject.Id)
                    .Select(u => new PostEntity
                    {
                        Id = u.Id,
                        EntityName = entityobject.EntityName,
                        OrganizationId = entityobject.OrganizationId,
                        EntityTypeId = entityobject.EntityTypeId,
                        CountryId = entityobject.CountryId,
                        StateId = entityobject.StateId,
                        City = entityobject.City,
                        Address = entityobject.Address,
                        Pin = entityobject.Pin,
                        Status = entityobject.Status,
                        CreatedOn = entityobject.CreatedOn,
                        CreatedBy = entityobject.CreatedBy,
                        MajorIndustry = entityobject.MajorIndustryId,
                        MinorIndustry = entityobject.MinorIndustryId,
                        ManagerId = entityobject.ManagerId,
                        FinancialYearStart = entityobject.FinancialYearStart,
                        FinancialYearEnd = entityobject.FinancialYearEnd,
                        FromMonth = entityobject.FromMonth,
                        ToMonth = entityobject.ToMonth
                    })
                    .FirstOrDefault();
                return result;
            }
        }

        public async Task<bool> UpdateEntityApproval(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, ApprovalStatus = ApprovalStatus, EntityId = access.EntityId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[organizations].USP_UPDATEENTITYAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                    if (ApprovalStatus != "Pending" && access.UID != null)
                    {
                        await EntityApprovalResponseNotification(access.UID.Value, ApprovalStatus);
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

        public async Task<FinancialMonth> GetStartAndEndMonthsByCountryId(int countryId)
        {
            //
            var startDate = await dbContext.Country.Where(a => a.Id == countryId && a.Status == 1).Select(a => a.FinancialStartDate).FirstOrDefaultAsync();
            var endDate = await dbContext.Country.Where(a => a.Id == countryId && a.Status == 1).Select(a => a.FinancialEndDate).FirstOrDefaultAsync();
            string month = startDate?.ToString("MMMM", CultureInfo.InvariantCulture) ?? string.Empty;
            var result = new FinancialMonth
            {
                FromMonth = startDate?.ToString("MMMM", CultureInfo.InvariantCulture) ?? string.Empty,
                ToMonth = endDate?.ToString("MMMM", CultureInfo.InvariantCulture) ?? string.Empty

            };
            return result;
        }

        public async Task<List<Models.Entity>> GetEntitiesByOrganizationId(long organizationId)
        {
            try
            {
                // Get all entities for the organization
                var entities = await dbContext.Entity
                    .Where(e => e.OrganizationId == organizationId)
                    .ToListAsync();

                var entityIds = entities.Select(e => e.Id).ToList();

                // Get all billing details for these entities
                var billingDetails = await dbContext.BillingDetails
                    .Where(b => b.EntityId.HasValue && entityIds.Contains(b.EntityId.Value))
                    .Select(b => b.EntityId.Value)
                    .Distinct()
                    .ToListAsync();

                // Get all service requests for these entities
                var serviceRequests = await dbContext.ServiceRequests
                    .Where(sr => entityIds.Contains(sr.EntityId))
                    .Select(sr => sr.EntityId)
                    .Distinct()
                    .ToListAsync();

                var mappedResult = await (
                    from e in dbContext.Entity
                    join o in dbContext.Organizations on e.OrganizationId equals o.Id into orgJoin
                    from o in orgJoin.DefaultIfEmpty()
                    join mi in dbContext.MajorIndustries on e.MajorIndustryId equals mi.Id into majorJoin
                    from mi in majorJoin.DefaultIfEmpty()
                    join mni in dbContext.MinorIndustries on e.MinorIndustryId equals mni.Id into minorJoin
                    from mni in minorJoin.DefaultIfEmpty()
                    join c in dbContext.Country on e.CountryId equals c.Id into countryJoin
                    from c in countryJoin.DefaultIfEmpty()
                    join s in dbContext.States on e.StateId equals s.Id into stateJoin
                    from s in stateJoin.DefaultIfEmpty()
                    join et in dbContext.EntityType on e.EntityTypeId equals et.Id into entityTypeJoin
                    from et in entityTypeJoin.DefaultIfEmpty()
                    join u in dbContext.Users on e.ManagerId equals u.Id into userJoin
                    from u in userJoin.DefaultIfEmpty()
                    where e.OrganizationId == organizationId
                    select new Models.Entity
                    {
                        Id = e.Id,
                        EntityName = e.EntityName,
                        OrganizationId = e.OrganizationId,
                        OrganizationName = o != null ? o.OrganizationName : null,
                        MajorIndustryId = e.MajorIndustryId,
                        MinorIndustryId = e.MinorIndustryId,
                        MajorIndustry = mi != null ? mi.MajorIndustryName : null,
                        MinorIndustry = mni != null ? mni.MinorIndustryName : null,
                        MajorIndustryName = mi != null ? mi.MajorIndustryName : null,
                        MinorIndustryName = mni != null ? mni.MinorIndustryName : null,
                        CountryId = e.CountryId.HasValue ? e.CountryId.Value.ToString() : null,
                        CountryName = c != null ? c.CountryName : null,
                        StateId = e.StateId,
                        StateName = s != null ? s.StateName : null,
                        City = e.City,
                        Address = e.Address,
                        Pin = e.Pin,
                        Status = e.Status,
                        CreatedOn = e.CreatedOn,
                        CreatedBy = e.CreatedBy.HasValue ? (int?)e.CreatedBy.Value : null,
                        ModifiedOn = e.ModifiedOn,
                        ModifiedBy = e.ModifiedBy.HasValue ? (int?)e.ModifiedBy.Value : null,
                        UID = e.UID.ToString(),
                        ManagerId = e.ManagerId.HasValue ? (int?)e.ManagerId.Value : null,
                        EntityTypeId = e.EntityTypeId.HasValue ? (int?)e.EntityTypeId.Value : null,
                        EntityType = et != null ? et.EntityType : null,
                        CustomerId = u != null ? u.EmpId : null,
                        PointOfContact = u != null ? u.FullName : null,
                        Approvedby = u != null ? u.FullName : null,
                        FinancialYearStart = e.FinancialYearStart.HasValue ? e.FinancialYearStart.Value.ToString() : null,
                        FinancialYearEnd = e.FinancialYearEnd.HasValue ? e.FinancialYearEnd.Value.ToString() : null,
                        ToMonth = e.ToMonth,
                        FromMonth = e.FromMonth,
                        HasBillingDetails = billingDetails.Contains(e.Id),
                        HasServiceRequests = serviceRequests.Contains(e.Id)
                    }
                ).ToListAsync();

                return mappedResult;
            }
            catch (Exception)
            {
                return new List<Models.Entity>();
            }
        }

        public async Task<List<Models.Entity>> GetEntitiesByOrganizationAndCountryId(long organizationId,long countryId)
        {
            try
            {
                var mappedResult = await (
                    from e in dbContext.Entity
                    join o in dbContext.Organizations on e.OrganizationId equals o.Id into orgJoin
                    from o in orgJoin.DefaultIfEmpty()
                    join mi in dbContext.MajorIndustries on e.MajorIndustryId equals mi.Id into majorJoin
                    from mi in majorJoin.DefaultIfEmpty()
                    join mni in dbContext.MinorIndustries on e.MinorIndustryId equals mni.Id into minorJoin
                    from mni in minorJoin.DefaultIfEmpty()
                    join c in dbContext.Country on e.CountryId equals c.Id into countryJoin
                    from c in countryJoin.DefaultIfEmpty()
                    join s in dbContext.States on e.StateId equals s.Id into stateJoin
                    from s in stateJoin.DefaultIfEmpty()
                    join et in dbContext.EntityType on e.EntityTypeId equals et.Id into entityTypeJoin
                    from et in entityTypeJoin.DefaultIfEmpty()
                    join u in dbContext.Users on e.ManagerId equals u.Id into userJoin
                    from u in userJoin.DefaultIfEmpty()
                    where e.OrganizationId == organizationId && e.CountryId == countryId
                    select new Models.Entity
                    {
                        Id = e.Id,
                        EntityName = e.EntityName,
                        OrganizationId = e.OrganizationId,
                        OrganizationName = o != null ? o.OrganizationName : null,
                        MajorIndustryId = e.MajorIndustryId,
                        MinorIndustryId = e.MinorIndustryId,
                        MajorIndustry = mi != null ? mi.MajorIndustryName : null,
                        MinorIndustry = mni != null ? mni.MinorIndustryName : null,
                        MajorIndustryName = mi != null ? mi.MajorIndustryName : null,
                        MinorIndustryName = mni != null ? mni.MinorIndustryName : null,
                        CountryId = e.CountryId.HasValue ? e.CountryId.Value.ToString() : null,
                        CountryName = c != null ? c.CountryName : null,
                        StateId = e.StateId,
                        StateName = s != null ? s.StateName : null,
                        City = e.City,
                        Address = e.Address,
                        Pin = e.Pin,
                        Status = e.Status,
                        CreatedOn = e.CreatedOn,
                        CreatedBy = e.CreatedBy.HasValue ? (int?)e.CreatedBy.Value : null,
                        ModifiedOn = e.ModifiedOn,
                        ModifiedBy = e.ModifiedBy.HasValue ? (int?)e.ModifiedBy.Value : null,
                        UID = e.UID.ToString(),
                        ManagerId = e.ManagerId.HasValue ? (int?)e.ManagerId.Value : null,
                        EntityTypeId = e.EntityTypeId.HasValue ? (int?)e.EntityTypeId.Value : null,
                        EntityType = et != null ? et.EntityType : null,
                        CustomerId = u != null ? u.EmpId : null,
                        PointOfContact = u != null ? u.FullName : null,
                        Approvedby = u != null ? u.FullName : null,
                        FinancialYearStart = e.FinancialYearStart.HasValue ? e.FinancialYearStart.Value.ToString() : null,
                        FinancialYearEnd = e.FinancialYearEnd.HasValue ? e.FinancialYearEnd.Value.ToString() : null,
                        ToMonth = e.ToMonth,
                        FromMonth = e.FromMonth
                    }
                ).ToListAsync();

                return mappedResult;
            }
            catch (Exception)
            {
                return new List<Models.Entity>();
            }
        }

        public async Task<List<Models.Entity>> GetClientEntitiesLocations(int organizationId)
        {
            try
            {
                int countryId;
                EntitiesCityCoordinate entitiesCityCoordinate = new EntitiesCityCoordinate();
                var result = new List<Models.Entity>();

                var countries = await dbContext.Country.ToListAsync();
                var states = await dbContext.States.ToListAsync();

                var countryDict = countries
                    .Where(c => c.Id.HasValue)
                    .ToDictionary(c => c.Id.Value, c => c.CountryName ?? string.Empty);

                var stateDict = states.ToDictionary(s => s.Id, s => s.StateName ?? string.Empty);

                var value = await dbContext.Entity.Where(a => a.OrganizationId == organizationId).ToListAsync();

                if (value != null && value.Count > 0)
                {
                    foreach (var entity in value)
                    {
                        if (!string.IsNullOrEmpty(entity.City))
                        {
                            entity.City = entity.City.Trim();
                            entitiesCityCoordinate = await _getCoordinates.GetCoordinatesFromCityAsync(entity.City);
                        }

                        var mappedEntity = new Models.Entity
                        {
                            Id = entity.Id,
                            EntityName = entity.EntityName,
                            OrganizationId = entity.OrganizationId,
                            MajorIndustryId = entity.MajorIndustryId,
                            MinorIndustryId = entity.MinorIndustryId,
                            CountryId = entity.CountryId.HasValue ? entity.CountryId.Value.ToString() : null,
                            StateId = entity.StateId,
                            City = entity.City,
                            Address = entity.Address,
                            Pin = entity.Pin,
                            Status = entity.Status,
                            CreatedOn = entity.CreatedOn,
                            CreatedBy = entity.CreatedBy.HasValue ? (int?)entity.CreatedBy.Value : null,
                            ModifiedOn = entity.ModifiedOn,
                            ModifiedBy = entity.ModifiedBy.HasValue ? (int?)entity.ModifiedBy.Value : null,
                            UID = entity.UID != null ? entity.UID.ToString() : null,
                            ManagerId = entity.ManagerId.HasValue ? (int?)entity.ManagerId.Value : null,
                            EntityTypeId = entity.EntityTypeId.HasValue ? (int?)entity.EntityTypeId.Value : null,
                            FinancialYearStart = entity.FinancialYearStart.HasValue ? entity.FinancialYearStart.Value.ToString() : null,
                            FinancialYearEnd = entity.FinancialYearEnd.HasValue ? entity.FinancialYearEnd.Value.ToString() : null,
                            FromMonth = entity.FromMonth,
                            ToMonth = entity.ToMonth
                        };

                        if (!string.IsNullOrEmpty(mappedEntity.CountryId) && int.TryParse(mappedEntity.CountryId, out countryId))
                        {
                            mappedEntity.CountryName = countryDict.TryGetValue(countryId, out var countryname) ? countryname : string.Empty;
                        }
                        else
                        {
                            mappedEntity.CountryName = string.Empty;
                        }

                        if (mappedEntity.StateId.HasValue)
                        {
                            long stateId = mappedEntity.StateId.Value;
                            mappedEntity.StateName = stateDict.TryGetValue(stateId, out var statename) ? statename : string.Empty;
                        }
                        else
                        {
                            mappedEntity.StateName = string.Empty;
                        }

                        mappedEntity.Latitude = entitiesCityCoordinate?.Latitude ?? 0;
                        mappedEntity.Longitude = entitiesCityCoordinate?.Longitude ?? 0;
                        result.Add(mappedEntity);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<Models.Entity>();
            }
        }

        private async Task<bool> AddEntityApprovalNotification(long? createdBy, string entityName, int? createdByRoleId)
        {
            try
            {
                var createdByDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy.Value);
                if (createdByDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        NotificationTitle = createdByRoleId == 1 ?
                        string.Format(ApiConstants.NewEntityBySuperAdminNotificationTitleTemplate, entityName)
                        :
                        string.Format(ApiConstants.NewEntityNotificationTitleTemplate, createdByDetails.UserName, entityName),

                        NotificationMessage = createdByRoleId == 1 ?
                        string.Format(ApiConstants.NewEntityBySuperAdminNotificationMessageTemplate, entityName)
                        :
                        string.Format(ApiConstants.NewEntityNotificationMessageTemplate, createdByDetails.UserName, entityName),

                        SenderUserId = createdByDetails.UserId,
                        RecipientUserId = createdByDetails.ManagerId,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        Status = createdByRoleId == 1 ?
                        Models.Enums.RefApprovalStatus.Approved
                        :
                        Models.Enums.RefApprovalStatus.Pending,

                        ModuleType = Models.Enums.ModuleType.Entity,
                        SenderUserName = createdByDetails?.UserName,
                        RecipientUserName = createdByDetails?.ManagerName,
                        MarkAsRead = false
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

        private async Task<bool> EntityApprovalResponseNotification(Guid uid, string approvalStatus)
        {
            try
            {
                var entityApprovalDetails = await dbContext.EntityApproval.FirstOrDefaultAsync(x => x.UID == uid);
                if (entityApprovalDetails != null)
                {
                    var userDetails = await _helperRepository.GetUserAndManagerInfoAsync(entityApprovalDetails.CreatedBy.Value);
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
                            notification.NotificationTitle = $"<b>{userDetails.ManagerName}</b> approved a <b>New Entity</b>";
                            notification.NotificationMessage = $"<b>{userDetails.ManagerName}</b> approved a <b>New Entity</b>";
                        }
                        else if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                            notification.NotificationTitle = $"<b>{userDetails.ManagerName}</b> rejected the <b>New Entity</b>";
                            notification.NotificationMessage = $"<b>{userDetails.ManagerName}</b> rejected the <b>New Entity</b>";
                        }
                        else if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Forward;
                            notification.NotificationTitle = $"<b>{userDetails.ManagerName}</b> forward the <b>New Entity</b>";
                            notification.NotificationMessage = $"<b>{userDetails.ManagerName}</b> forward the <b>New Entity</b>";
                        }
                        await dbContext.AddAsync(notification);
                        await dbContext.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}