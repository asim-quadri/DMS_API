using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Dapper.SqlMapper;

namespace ComplianceAPI.Repository
{
    public interface IAccessRepository
    {
        Task<List<AccessModel>> GetAccessList(Guid? UserUID);

        Task<List<PendingApproval>> GetPendingApproval(Guid? UserUID);

        Task<bool> PostUserManagement(List<AccessModel> access);

        Task<bool> PostApproveAccess(AccessModel access);

        Task<bool> PostRejectAccess(AccessModel access);

        /// <summary>
        /// Retrieves the menu options for a given role ID.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MenuOptions>> GetMenuOptions(int roleId, int userId);

        /// <summary>
        /// Retrieves the user access details for a given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserAccess>> GetUserAccess(int userId);

        /// <summary>
        /// Set User access for the user Id
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns></returns>
        Task<bool> SetUserAccess(List<SetAccessRequest> userAccess);

        /// <summary>
        /// Get Menu options for the parent id
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MenuOptions>> GetMenuOptionsForParent(int parentId, int userId);

        /// <summary>
        /// Post User Organization Mapping
        /// </summary>
        /// <param name="userOrganizations"></param>
        /// <returns></returns>
        Task<bool> PostUserOrganizationMapping(List<UsersOrganizations> userOrganizations);

        Task<UserRegulationMappingDetails> GetUserRegulationMappingDetailsByUserIdAsync(long userId);

        Task<bool> AddUserRegulationMappingAsync(PostUserRegulationMapping mapping);
    }

    public class AccessRepository : IAccessRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly ComplianceDbContext dbContext;

        public AccessRepository(IUnitOfWork unitOfWork, ComplianceDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }

        public async Task<List<AccessModel>> GetAccessList(Guid? UserUID)
        {
            try
            {
                var userId = dbContext.Users.FirstOrDefaultAsync(u => u.UID == UserUID).Id;

                if (userId == 0)
                {
                    // User not found
                    return null;
                }

                var accessList = await (from u in dbContext.Users
                                        join um in dbContext.Users on u.ManagerId equals um.Id into umj
                                        from um in umj.DefaultIfEmpty()
                                        join ur in dbContext.UserRoleMapping on u.Id equals ur.UserId
                                        join rr in dbContext.RefRoles on ur.RoleId equals rr.Id
                                        join upm in dbContext.UserProductMapping on u.Id equals upm.UserId
                                        join rp in dbContext.RefProducts on upm.ProductId equals rp.Id
                                        where u.Id == userId
                                        select new AccessModel
                                        {
                                            //FullName = u.FullName,
                                            //EmpId = u.EmpId,
                                            //RoleDisplayName = rr.RoleDisplayName,
                                            ManagerId = u.ManagerId ?? 0,
                                            Status = upm.Status,
                                            //PointOfContact = u.FullName, // Assuming PointOfContact is same as FullName
                                            UserUID = u.UID,
                                            ProductMappingId = upm.Id,
                                            ProductId = rp.Id,
                                            UserId = u.Id,
                                            UID = upm.UID,
                                            CreatedBy = upm.CreatedBy,
                                            ProductName = rp.ProductName
                                        }).ToListAsync();

                return accessList;
            }
            catch (Exception ex)
            {
                // Handle exception
                throw ex;
            }
            //using (var connection = _unitOfWork.ConnectionFactory())
            //{
            //    var result = await connection.QueryAsync<AccessModel>("USP_GETACCESSLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
            //    return result.ToList();
            //}
        }

        public async Task<List<PendingApproval>> GetPendingApproval(Guid? UserUID)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<PendingApproval>("USP_GETAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<bool> PostUserManagement(List<AccessModel> accessModel)
        {
            List<Models.UserProductMapping> mainresult = new List<Models.UserProductMapping>();
            try
            {
                var result = await (from upm in dbContext.UserProductMapping
                                    join rp in dbContext.RefProducts on upm.ProductId equals rp.Id
                                    join u in dbContext.Users on upm.UserId equals u.Id
                                    where u.Id == accessModel[0].UserId
                                    select new Models.UserProductMapping
                                    {
                                        Id = upm.Id,
                                        UserId = upm.UserId,
                                        ProductId = upm.ProductId,
                                        Status = upm.Status,
                                        UID = upm.UID
                                    }).ToListAsync();

                mainresult = result.ToList();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw ex;
            }

            //using (var connection = _unitOfWork.ConnectionFactory())
            //{
            //    var result = await connection.QueryAsync<Models.UserProductMapping>("USP_GETPRODUCTMAPPINGBYUSERID", new { UserId = accessModel[0].UserId }, commandType: System.Data.CommandType.StoredProcedure);
            //    mainresult = result.ToList();
            //}

            foreach (var access in accessModel)
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    try
                    {
                        var isAdmin = IsSuperAdmin(access.CreatedBy);

                        var statusId = await dbContext.RefApprovalStatus.Where(s => s.Status == (isAdmin ? "Approved" : access.Status.ToString())).Select(s => s.Id).FirstOrDefaultAsync();

                        var existingMapping = await dbContext.UserProductMapping.FirstOrDefaultAsync(upm => upm.UserId == access.UserId && upm.ProductId == access.ProductId);
                        Models.DataModels.UserProductMapping result = null;
                        if (existingMapping == null)
                        {
                            var newMapping = new Models.DataModels.UserProductMapping
                            {
                                UserId = access.UserId,
                                ProductId = access.ProductId,
                                Status = statusId,
                                CreatedBy = access.CreatedBy,
                                CreatedOn = DateTime.Now,
                                Enable = access.Enable
                            };

                            await dbContext.UserProductMapping.AddAsync(newMapping);
                            await dbContext.SaveChangesAsync();
                            result = newMapping;
                        }
                        else
                        {
                            existingMapping.Enable = access.Enable;
                            existingMapping.ModifiedBy = access.CreatedBy;
                            existingMapping.ModifiedOn = DateTime.Now;
                            await dbContext.UserProductMapping.AddAsync(existingMapping);
                            await dbContext.SaveChangesAsync();
                            result = existingMapping;
                        }
                        //var inputdata = new { UserId = access.UserId, ProductId = access.ProductId, CreatedBy = access.CreatedBy, UID = access.UID, Status = RefApprovalStatusU.Pending, Enable = access.Enable };
                        //var result = await sqlContext.Connection.QueryFirstAsync<Models.UserProductMapping>("USP_ADDPRODUCTACCESS", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);

                        if (mainresult.Count(f => f.UserId == access.UserId && f.ProductId == access.ProductId) <= 0 || ((mainresult.Count(f => f.UserId == access.UserId && f.ProductId == access.ProductId) > 0 && mainresult.Find(f => f.UserId == access.UserId && f.ProductId == access.ProductId).Enable == 0) && access.Enable == 1))
                        {
                            var approvaldata = new { UserId = access.UserId, ManagerId = access.ManagerId, ProductMappingId = result.Id, ApprovalType = Helpers.RefApprovalType.Access, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = RefApprovalStatusU.Pending };
                            await sqlContext.Connection.QueryAsync<Models.UserProductMapping>("USP_ADDUPDATEPRODUCTAPPROVAL", approvaldata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                        }

                        sqlContext.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlContext.Rollback();
                        throw ex;
                    }
                }
            }

            return true;
        }

        public async Task<bool> PostApproveAccess(AccessModel access)
        {
            List<PendingApproval> Productresult = new List<PendingApproval>();

            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<PendingApproval>("USP_GETPRODUCTAPPROVALLIST", new { UserUID = access.UserUID, Unique = 0 }, commandType: System.Data.CommandType.StoredProcedure);
                Productresult = result.ToList();
            }

            foreach (var item in Productresult)
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    try
                    {
                        await sqlContext.Connection.QueryAsync("USP_ADDUPDATEPRODUCTAPPROVAL", new { UserId = item.UserId, ManagerId = access.ManagerId, ProductMappingId = item.ProductMappingId, ApprovalType = item.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Approved, CreatedBy = access.CreatedBy, UID = item.ApproverUID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                        sqlContext.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlContext.Rollback();
                        throw ex;
                    }
                }
            }
            return true;
        }

        public async Task<bool> PostRejectAccess(AccessModel access)
        {
            List<PendingApproval> Productresult = new List<PendingApproval>();

            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<PendingApproval>("USP_GETPRODUCTAPPROVALLIST", new { UserUID = access.UserUID, Unique = 0 }, commandType: System.Data.CommandType.StoredProcedure);
                Productresult = result.ToList();
            }

            foreach (var item in Productresult)
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    try
                    {
                        await sqlContext.Connection.QueryAsync("USP_ADDUPDATEPRODUCTAPPROVAL", new { UserId = item.UserId, ManagerId = access.ManagerId, ProductMappingId = item.ProductMappingId, ApprovalType = item.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Rejected, CreatedBy = access.CreatedBy, UID = item.ApproverUID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                        sqlContext.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlContext.Rollback();
                        throw ex;
                    }
                }
            }
            return true;
        }

        public bool IsSuperAdmin(long? userId)
        {
            var isSuperAdmin = (from user in dbContext.Users
                                join userRoleMapping in dbContext.UserRoleMapping on user.Id equals userRoleMapping.UserId
                                join refRole in dbContext.RefRoles on userRoleMapping.RoleId equals Convert.ToInt32(refRole.Id)
                                where user.Id == userId && (refRole.RoleName == "SuperAdmin" || refRole.RoleName == "ITSupportAdmin")
                                select user).Any();

            return isSuperAdmin;
        }

        ///<see cref="IAccessRepository.GetMenuOptions(int, int)"/>
        public async Task<List<MenuOptions>> GetMenuOptions(int roleId, int userId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<MenuOptions>("USP_GETMENUOPTIONS", new { RoleId = roleId }, commandType: System.Data.CommandType.StoredProcedure);
                if (result.Any())
                {
                    if (IsSuperAdmin(userId) || IsITAdmin(userId))
                    {
                        return result.ToList();
                    }
                    var usersAccesses = await GetUserAccess(userId);
                    foreach (var item in result)
                    {
                        var userAccess = usersAccesses.FirstOrDefault(x => x.ParentId == item.MenuId && x.HasAccess);

                        if (userAccess != null)
                        {
                            item.Route = userAccess.Route;
                        }
                        else
                        {
                            if (item.Title == "Home" || item.Title == "Announcements" || item.Title == "Service Requests" || item.Title == "Logout")
                            {
                                continue;
                            }
                            else
                            {
                                item.Route = $"/no-access/{item.Title}";
                            }
                        }
                    }
                }
                return result.ToList();
            }
        }

        ///<see cref="IAccessRepository.GetUserAccess(int)"/>
        public async Task<List<UserAccess>> GetUserAccess(int userId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<UserAccess>("USP_GETUSERACCESS", new { UId = userId }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        ///<see cref="IAccessRepository.SetUserAccess(List{SetAccessRequest})"/>
        public async Task<bool> SetUserAccess(List<SetAccessRequest> userAccess)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    foreach (var access in userAccess)
                    {
                        var inputdata = new
                        {
                            UserId = access.UserId,
                            MenuId = access.MenuId,
                            HasAccess = access.HasAccess
                        };
                        await sqlContext.Connection.ExecuteAsync("USP_ADDUSERACCESS", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    }
                    sqlContext.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }
        }

        ///<see cref="IAccessRepository.GetMenuOptionsForParent(int, int)"/>
        public async Task<List<MenuOptions>> GetMenuOptionsForParent(int parentId, int userId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                if (IsSuperAdmin(userId) || IsITAdmin(userId))
                {
                    var query = @"SELECT
                                M.Id,
                                M.Title,
                                M.Icon,
                                M.Route,
                                M.ParentId,
                                M.SortOrder,
                                M.Id AS MenuId,
                                CAST(1 AS BIT) AS CanView
                            FROM
                                Menus M
                            WHERE
                                M.ParentId = @ParentId
                            ORDER BY
                                M.SortOrder;
                            ";
                    return (await connection.QueryAsync<MenuOptions>(query, new { ParentId = parentId })).ToList();
                }

                var query1 = @"
                                SELECT
                                    M.Id,
                                    M.Title,
                                    M.Icon,
                                    M.Route,
                                    M.ParentId,
                                    M.SortOrder,
                                    M.Id AS MenuId,
                                    CAST(ISNULL(UA.HasAccess, 0) AS BIT) AS CanView
                                FROM
                                    Menus M
                                LEFT JOIN
                                    UserAccess UA ON UA.MenuId = M.Id AND UA.UserId = @UserId
                                WHERE
                                    M.ParentId = @ParentId
                                ORDER BY
                                    M.SortOrder;
                                ";

                var result = await connection.QueryAsync<MenuOptions>(query1, new { UserId = userId, ParentId = parentId });

                return result.ToList();
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

        public bool IsITAdmin(int userId)
        {
            var isITAdmin = (from user in dbContext.Users
                             join userRoleMapping in dbContext.UserRoleMapping on user.Id equals userRoleMapping.UserId
                             join refRole in dbContext.RefRoles on userRoleMapping.RoleId equals Convert.ToInt32(refRole.Id)
                             where user.Id == userId && (refRole.RoleName == "ITSupportAdmin" || refRole.RoleName == "ITAdmin" || refRole.RoleName == "ITUser")
                             select user).Any();
            return isITAdmin;
        }

        ///<see cref="IAccessRepository.PostUserOrganizationMapping(List<UsersOrganizations>)"/>
        public async Task<bool> PostUserOrganizationMapping(List<UsersOrganizations> userOrganizations)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    foreach (var userOrg in userOrganizations)
                    {
                        var inputdata = new
                        {
                            UserId = userOrg.UserId,
                            OrgId = userOrg.OrganizationId,
                            HasAccess = userOrg.HasAccess
                        };
                        await sqlContext.Connection.QueryAsync<object>(
                            "[dbo].[USP_USERORGANISATION]",
                            inputdata,
                            commandType: System.Data.CommandType.StoredProcedure,
                            transaction: sqlContext.Transaction
                        ).ConfigureAwait(false);
                    }
                    sqlContext.Commit();
                    return true;
                }
                catch
                {
                    sqlContext.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> AddUserRegulationMappingAsync(PostUserRegulationMapping mapping)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var input = new
                    {
                        UserId = mapping.UserId,
                        CountryIds = mapping.CountryIds,
                        StateIds = mapping.StateIds,
                        RegulationIds = mapping.RegulationIds,
                        RegulationGroupId = mapping.RegulationGroupId,
                        ComplianceType = mapping.ComplianceType,
                        ComplianceId = mapping.ComplianceId
                    };

                    await sqlContext.Connection.ExecuteAsync(
                        "[product_owner].[USP_POSTUSERREGULATIONMAPPING]",
                        input,
                        commandType: System.Data.CommandType.StoredProcedure,
                        transaction: sqlContext.Transaction
                    );
                    sqlContext.Commit();
                    return true;
                }
                catch
                {
                    sqlContext.Rollback();
                    return false;
                }
            }
        }

        public async Task<UserRegulationMappingDetails> GetUserRegulationMappingDetailsByUserIdAsync(long userId)
        {
            try
            {
                using (var connection = _unitOfWork.ConnectionFactory())
                {
                    var mapping = await connection.QueryFirstOrDefaultAsync<PostUserRegulationMapping>(
                        "[product_owner].[USP_GETUSERREGULATIONMAPPINGBYUSERID]",
                        new { UserId = userId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );

                    if (mapping == null)
                        return null;

                    var countryIds = (mapping.CountryIds ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => int.TryParse(id, out var i) ? i : (int?)null)
                        .Where(i => i.HasValue)
                        .Select(i => i.Value)
                        .ToList();

                    var stateIds = (mapping.StateIds ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => int.TryParse(id, out var i) ? i : (int?)null)
                        .Where(i => i.HasValue)
                        .Select(i => i.Value)
                        .ToList();

                    var regulationIds = (mapping.RegulationIds ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => long.TryParse(id, out var i) ? (long?)i : null)
                        .Where(i => i.HasValue)
                        .Select(i => i.Value)
                        .ToList();

                    var complianceIds = (mapping.ComplianceId ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => long.TryParse(id, out var i) ? (long?)i : null)
                        .Where(i => i.HasValue)
                        .Select(i => i.Value)
                        .ToList();

                    var countries = await dbContext.Country
                        .Where(c => countryIds.Contains(c.Id.Value))
                        .Select(c => new IdNamePair { Id = c.Id.ToString(), Name = c.CountryName })
                        .ToListAsync();

                    var states = await dbContext.States
                        .Where(s => stateIds.Contains((int)s.Id))
                        .Select(s => new IdNamePair { Id = s.Id.ToString(), Name = s.StateName })
                        .ToListAsync();

                    var regulations = await dbContext.RegulationSetupDetails
                        .Where(r => regulationIds.Contains(r.Id.Value))
                        .Select(r => new IdNamePair { Id = r.Id.ToString(), Name = r.RegulationName })
                        .ToListAsync();

                    var complianceIdList = await dbContext.RegulationSetupCompliance
                        .Where(c => complianceIds.Contains(c.Id))
                        .Select(c => new IdNamePair { Id = c.Id.ToString(), Name = c.ComplianceName })
                        .ToListAsync();

                    var complianceTypeIds = (mapping.ComplianceType ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => int.TryParse(id, out var i) ? i : (int?)null)
                        .Where(i => i.HasValue)
                        .Select(i => i.Value)
                        .ToList();

                    //var complianceTypeList = await dbContext.TOCImprisonment
                    //    .Where(t => t.ComplianceId.HasValue && complianceTypeIds.Contains((int)t.ComplianceId.Value))
                    //    .Select(t => new IdNamePair { Id = t.ComplianceId.Value.ToString(), Name = t.TOCRuleType })
                    //    .Distinct()
                    //    .ToListAsync();
                    var complianceTypeListRes = await GetTypeOfCompliances();

                    var complianceTypeList = complianceTypeListRes
                            .Where(x => complianceTypeIds.Contains(x.Id))
                            .Select(c => new IdNamePair { Id = c.Id.ToString(), Name = c.TypeOfComplianceName })
                            .ToList();

                    List<IdNamePair> regulationGroupNameList = new();
                    var groupIds = (mapping.RegulationGroupId ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => int.TryParse(id, out var i) ? i : (int?)null)
                        .Where(i => i.HasValue)
                        .Select(i => i.Value)
                        .ToList();

                    if (groupIds.Any())
                    {
                        var groupNames = await dbContext.RegulationGroups
                            .Where(g => groupIds.Contains(g.Id.HasValue ? (int)g.Id.Value : 0))
                            .Select(g => new IdNamePair { Id = g.Id.ToString(), Name = g.RegulationGroupName })
                            .ToListAsync();

                        regulationGroupNameList.AddRange(groupNames);
                    }

                    return new UserRegulationMappingDetails
                    {
                        UserId = mapping.UserId,
                        Countries = countries,
                        States = states,
                        Regulations = regulations,
                        RegulationGroupName = regulationGroupNameList,
                        ComplianceType = complianceTypeList,
                        ComplianceId = complianceIdList
                    };
                }
            }
            catch (Exception)
            {
                return new UserRegulationMappingDetails();
            }
        }

        private async Task<List<TypeOfCompliance>> GetTypeOfCompliances()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var res = await connection.QueryAsync<TypeOfCompliance>(
                    @"SELECT
                        t.Id,
                        t.RegulationSetupId,
                        t.ComplianceId,
                        CASE
                            WHEN t.RegulationSetupId IS NOT NULL
                                THEN r.RegulationName + '-' + t.TOCRuleType + '-' + t.Frequency + '-' + CAST(t.FromTrunOver AS VARCHAR) + '-' + CAST(t.ToTrunOver AS VARCHAR)
                            WHEN t.ComplianceId IS NOT NULL
                                THEN c.ComplianceName + '-' + t.TOCRuleType + '-' + t.Frequency + '-' + CAST(t.FromTrunOver AS VARCHAR) + '-' + CAST(t.ToTrunOver AS VARCHAR)
                            ELSE NULL
                        END AS TypeOfComplianceName
                    FROM [ComplianceNew].[product_owner].[TOCDues] t
                    LEFT JOIN [ComplianceNew].[product_owner].[RegulationSetupDetails] r
                        ON t.RegulationSetupId = r.Id
                    LEFT JOIN [ComplianceNew].[product_owner].[RegulationSetupCompliance] c
                        ON t.ComplianceId = c.Id
                    ");

                return res.ToList();
            }
        }
    }
}