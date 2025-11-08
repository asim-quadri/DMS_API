using Dapper;
using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Parameter = ComplianceAPI.Models.Parameter;
using ComplianceAPI.Models.Enums;

namespace ComplianceAPI.Repository
{
    public interface IParameterRepository
    {
        Task<List<Parameters>> GetAllParameters();
        Task<Parameter> GetHistoryParameters(long HistoryId);
        Task<Parameter> AdminAddParameter(AddParameter Parameter);
        Task<Parameter> AddParameter(AddParameter Parameter);
        Task<List<Parameters>> GetParameterById(long id);
        Task<Parameter> DeleteParameter(Guid uid, int status);
        Task<bool> PostUpdateParameterApproval(AccessModel access);
        Task<List<PendingApproval>> GetPendingParameterApproval(Guid? UserUID);
        Task<List<PendingApproval>> GetAllParameterApproval();
        Task<bool> PostParameterApproveAccess(AccessModel access);
        Task<bool> PostParameterReviewAccess(AccessModel access);
        Task<bool> PostParameterRejectAccess(AccessModel access);
        bool IsSuperAdmin(int userId);
        Task<string> GetNextParameterCode();

    }
    public class ParameterRepository : IParameterRepository
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ComplianceDbContext dbContext;
        public ParameterRepository(ComplianceDbContext dbContext, IUnitOfWork unitOfWork)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Parameter> GetHistoryParameters(long HistoryId)
        {

            var parameter = (from ph in dbContext.ParameterHistory
                             join ud in dbContext.Users on ph.ManagerId equals ud.Id into managerJoin
                             where ph.HistoryId == HistoryId
                             select new Parameter
                             {
                                 HistoryId = ph.HistoryId,
                                 Id = ph.Id,
                                 EmpId = ph.EmpId,
                                 ParameterName = ph.ParameterName,
                                 ParameterType = ph.ParameterType,
                                 Status = ph.Status,
                                 ManagerId = ph.ManagerId,
                                 CreatedOn = ph.CreatedOn,
                                 CreatedBy = ph.CreatedBy,
                                 ModifiedBy = ph.ModifiedBy,
                                 ModifiedOn = ph.ModifiedOn,
                                 UID = ph.UID,
                                 //ManagerName = manager.FullName
                             }).FirstOrDefault();

            return parameter;
        }
        public async Task<Parameter> AdminAddParameter(AddParameter Parameter)
        {
            int superAdminCount = 0;
            string roleName = "";
            string message = "Successfully Saved";
            ComplianceAPI.Models.DataModels.Parameters parameterobject = null;
            parameterobject = new ComplianceAPI.Models.DataModels.Parameters
            {
                EmpId = Parameter.CreatedBy.ToString(),
                ParameterName = Parameter.ParameterName,
                ParameterType = Parameter.ParameterType,
                Status = 0,
                ManagerId = Parameter.ManagerId,
                CreatedOn = DateTime.Now,
                CreatedBy = Parameter.CreatedBy,
                ParameterReferenceCode = Parameter.ParameterReferenceCode
            };
            dbContext.Parameter.Add(parameterobject);
            dbContext.SaveChanges();

            return dbContext.Parameter
                .Where(p => p.Id == parameterobject.Id)
                .Select(p => new Parameter
                {
                    Id = p.Id,
                    EmpId = p.EmpId,
                    ParameterName = p.ParameterName,
                    ParameterType = p.ParameterType,
                    Status = p.Status,
                    ManagerId = p.ManagerId,
                    CreatedOn = p.CreatedOn,
                    CreatedBy = p.CreatedBy,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn,
                    UID = p.UID,
                    ResponseCode = 1,
                    ResponseMessage = message,
                    ParameterReferenceCode = p.ParameterReferenceCode
                })
                .FirstOrDefault();

        }
        public async Task<Parameter> AddParameter(AddParameter Parameter)
        {
            int superAdminCount = 0;
            string roleName = "";
            string message = "Parameter Added and sent for Approval";
            ComplianceAPI.Models.DataModels.ParameterHistory parameterobject = null;
            parameterobject = new ComplianceAPI.Models.DataModels.ParameterHistory
            {
                EmpId = Parameter.CreatedBy.ToString(),
                ParameterName = Parameter.ParameterName,
                ParameterType = Parameter.ParameterType,
                Status = 0,
                ManagerId = Parameter.ManagerId,
                CreatedOn = DateTime.Now,
                CreatedBy = Parameter.CreatedBy,

            };
            dbContext.ParameterHistory.Add(parameterobject);
            dbContext.SaveChanges();

            return dbContext.ParameterHistory
                .Where(p => p.HistoryId == parameterobject.HistoryId)
                .Select(p => new Parameter
                {
                    HistoryId = p.HistoryId,
                    EmpId = p.EmpId,
                    ParameterName = p.ParameterName,
                    ParameterType = p.ParameterType,
                    Status = p.Status,
                    ManagerId = p.ManagerId,
                    CreatedOn = p.CreatedOn,
                    CreatedBy = p.CreatedBy,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn,
                    UID = p.UID,
                    ResponseCode = 1,
                    ResponseMessage = message
                })
                .FirstOrDefault();

        }
        public async Task<List<Parameters>> GetAllParameters()
        {
            return await dbContext.Parameter.ToListAsync();
        }
        public async Task<List<Parameters>> GetParameterById(long id)
        {
            return await dbContext.Parameter.Where(i => i.Id == id).ToListAsync();
        }
        public async Task<Parameter> DeleteParameter(Guid uid, int status)
        {
            var user = await dbContext.Parameter.FirstOrDefaultAsync(u => u.UID == uid);

            if (user != null)
            {
                var id = user.Id;

                var isSuperAdmin = await (
                                     from u in dbContext.Parameter
                                     join urm in dbContext.UserRoleMapping on u.Id equals urm.UserId
                                     join rr in dbContext.RefRoles on urm.RoleId equals rr.Id
                                     where u.UID == uid && rr.RoleName == "SuperAdmin"
                                     select u
                                     ).AnyAsync();

                var hasActiveUsers = await dbContext.Users
                                     .Where(u => u.Status == 1 && u.ManagerId == id)
                                     .AnyAsync();
                if (!isSuperAdmin && !hasActiveUsers)
                {
                    user.Status = (byte)status;
                    user.ModifiedOn = DateTime.Now;
                    user.ModifiedBy = 1;
                    await dbContext.SaveChangesAsync();

                    return new Parameter() { ResponseCode = 1, ResponseMessage = "Deleted Successfully" };
                }
                return new Parameter() { ResponseCode = 0, ResponseMessage = "In order to delete, Please change the user's role under {user.FullName}" };
            }
            return new Parameter() { ResponseCode = -1, ResponseMessage = "Parameter not found." };


            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    var mul = await sqlContext.Connection.QueryAsync<User>("USP_DELETEUSERBYUID", new { UID = uid, ModifiedBy = 1, Status = status }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction);
            //    sqlContext.Commit();
            //    return mul.FirstOrDefault();
            //}
        }
        public async Task<bool> PostUpdateParameterApproval(AccessModel access)
        {

            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
            var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
            var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");
            var forwardId = approvalStatuses.GetValueOrDefault("Forward");


            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.ParameterApproval.Any(ua => ua.HistoryId == access.HistoryId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.ParameterApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.ParameterApproval.Add(new ParameterApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now,
                    HistoryId = access.HistoryId,
                    UID = Guid.NewGuid(),

                });
            }
            else
            {
                var userApproval = dbContext.ParameterApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.Parameter.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();
            return true;
        }
        public async Task<List<PendingApproval>> GetPendingParameterApproval(Guid? UserUID)
        {
            try
            {
                var approvalTypeIds = dbContext.RefApprovalType
                                    .Where(at => at.ApprovalType == "User" || at.ApprovalType == "Role" || at.ApprovalType == "Access")
                                    .ToDictionary(at => at.ApprovalType, at => at.Id);

                var userAType = approvalTypeIds.GetValueOrDefault("User");
                var roleAType = approvalTypeIds.GetValueOrDefault("Role");
                var accessAType = approvalTypeIds.GetValueOrDefault("Access");

                var userId = dbContext.Users
                    .Where(u => u.UID == UserUID)
                    .Select(u => u.Id)
                    .FirstOrDefault();

                var userApprovals = (from upa in dbContext.ParameterApproval
                                     join p in dbContext.ParameterHistory
                                        on upa.HistoryId equals p.HistoryId
                                     join um in dbContext.Parameter on upa.ManagerId equals um.Id into managerJoin
                                     from um in managerJoin.DefaultIfEmpty()
                                     join rat in dbContext.RefApprovalType on upa.ApprovalType equals rat.Id
                                     join ras in dbContext.RefApprovalStatus on upa.ApprovalStatus equals ras.Id
                                     join u in dbContext.Users on upa.ManagerId equals u.Id
                                     join uk in dbContext.Users on upa.CreatedBy equals uk.Id
                                     where (upa.ManagerId == userId || upa.CreatedBy == userId) && new[] { userAType, roleAType, accessAType }.Contains(rat.Id)
                                     select new PendingApproval
                                     {
                                         // Id = upa.Id,
                                         HistoryId = p.HistoryId,
                                         ParameterName = p.ParameterName,
                                         ParameterType = p.ParameterType,
                                         EmpId = p.EmpId,
                                         ApproverManager = um != null ? um.ManagerId ?? 0 : 0,
                                         ApprovalType = rat.ApprovalType,
                                         ApprovalTypeId = rat.Id,
                                         Status = ras.Status,
                                         PointofContact = uk.FullName,
                                         approvedBy = u.FullName,
                                         ApproverUID = upa.UID,
                                         UserUID = p.UID,
                                         CreatedBy = upa.CreatedBy
                                     }).ToList();
                return userApprovals;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<PendingApproval>> GetAllParameterApproval()
        {
            try
            {
                var userApprovals = (from upa in dbContext.ParameterApproval
                                     join p in dbContext.ParameterHistory
                                        on upa.HistoryId equals p.HistoryId
                                     join um in dbContext.Parameter on upa.ManagerId equals um.Id into managerJoin
                                     from um in managerJoin.DefaultIfEmpty()
                                     join rat in dbContext.RefApprovalType on upa.ApprovalType equals rat.Id
                                     join ras in dbContext.RefApprovalStatus on upa.ApprovalStatus equals ras.Id
                                     join u in dbContext.Users on upa.ManagerId equals u.Id
                                     join uk in dbContext.Users on upa.CreatedBy equals uk.Id
                                     select new PendingApproval
                                     {
                                         // Id = upa.Id,
                                         HistoryId = p.HistoryId,
                                         ParameterName = p.ParameterName,
                                         ParameterType = p.ParameterType,
                                         EmpId = p.EmpId,
                                         ApproverManager = um != null ? um.ManagerId ?? 0 : 0,
                                         ApprovalType = rat.ApprovalType,
                                         ApprovalTypeId = rat.Id,
                                         Status = ras.Status,
                                         PointofContact = uk.FullName,
                                         approvedBy = u.FullName,
                                         ApproverUID = upa.UID,
                                         UserUID = p.UID,
                                         CreatedBy = upa.CreatedBy
                                     }).ToList();
                return userApprovals;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> PostParameterApproveAccess(AccessModel access)
        {

            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var statusIds = dbContext.RefApprovalStatus
                            .Where(s => s.Status == "Approved" || s.Status == "Reviewed" || s.Status == "Pending")
                            .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = statusIds.GetValueOrDefault("Approved");
            var reviewerStatusId = statusIds.GetValueOrDefault("Reviewed");
            var pendingApprovalId = statusIds.GetValueOrDefault("Pending");

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.ParameterApproval.Any(ua => ua.HistoryId == access.HistoryId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.ParameterApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.ParameterApproval.Add(new ParameterApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.ParameterApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;

                    //if ((approvalStatusId == reviewerStatusId) && !isAdmin)
                    //{
                    //    access.CreatedBy = dbContext.ParameterApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                    //    dbContext.UserApproval.Add(new UserApproval
                    //    {
                    //        //UserId = access.UserId,
                    //        ManagerId = newManagerId,
                    //        ApprovalType = access.ApprovalTypeId,
                    //        ApprovalStatus = approvedStatusId,
                    //        CreatedBy = access.CreatedBy,
                    //        CreatedOn = DateTime.Now
                    //    });
                    //}
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == approvedStatusId) && isAdmin))
            {
                var parameter = dbContext.ParameterHistory.FirstOrDefault(u => u.HistoryId == access.HistoryId);
                if (parameter != null)
                {
                    ComplianceAPI.Models.DataModels.Parameters parameterobject = null;
                    if (parameter.Id == 0 || parameter.Id == null)
                    {

                        parameterobject = new ComplianceAPI.Models.DataModels.Parameters
                        {
                            EmpId = parameter.EmpId,
                            ParameterName = parameter.ParameterName,
                            ParameterType = parameter.ParameterType,
                            Status = 1,
                            ManagerId = parameter.ManagerId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = parameter.CreatedBy,


                        };
                        dbContext.Parameter.Add(parameterobject);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        parameterobject = dbContext.Parameter.FirstOrDefault(u => u.Id == parameter.Id);
                        if (parameter != null)
                        {
                            parameterobject.ParameterName = parameter.ParameterName;
                            parameterobject.ParameterType = parameter.ParameterType;
                            parameterobject.EmpId = parameter.EmpId;
                            parameterobject.ManagerId = parameter.ManagerId;
                            parameterobject.ModifiedBy = parameter.CreatedBy;
                            parameterobject.ModifiedOn = DateTime.Now;
                            parameterobject.Status = 1;
                            dbContext.Update(parameterobject);
                            // dbContext.SaveChanges();
                        }
                    }
                }
            }

            dbContext.SaveChanges();
            return true;
        }
        public async Task<bool> PostParameterRejectAccess(AccessModel access)
        {
            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var approvedStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Approved").Select(s => s.Id)
                .FirstOrDefault();

            var reviewerStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Reviewed").Select(s => s.Id)
                .FirstOrDefault();

            var pendingApprovalId = dbContext.RefApprovalStatus.Where(s => s.Status == "Pending").Select(s => s.Id)
                .FirstOrDefault();

            var rejectedId = dbContext.RefApprovalStatus.Where(s => s.Status == "Rejected").Select(s => s.Id)
                .FirstOrDefault();

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Rejected.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.ParameterApproval.Any(ua => ua.CreatedBy == access.UserId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.ParameterApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.ParameterApproval.Add(new ParameterApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.ParameterApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;

                    if ((approvalStatusId == reviewerStatusId) && !isAdmin)
                    {
                        access.CreatedBy = dbContext.ParameterApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                        dbContext.ParameterApproval.Add(new ParameterApproval
                        {
                            //UserId = access.UserId,
                            ManagerId = newManagerId,
                            ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = approvedStatusId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.Parameter.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();


            return true;

        }
        public async Task<bool> PostParameterReviewAccess(AccessModel access)
        {
            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var approvedStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Approved").Select(s => s.Id)
                .FirstOrDefault();

            var reviewerStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Reviewed").Select(s => s.Id)
                .FirstOrDefault();

            var pendingApprovalId = dbContext.RefApprovalStatus.Where(s => s.Status == "Pending").Select(s => s.Id)
                .FirstOrDefault();

            var rejectedId = dbContext.RefApprovalStatus.Where(s => s.Status == "Rejected").Select(s => s.Id)
                .FirstOrDefault();

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Reviewed.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.ParameterApproval.Any(ua => ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.ParameterApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.ParameterApproval.Add(new ParameterApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var parameterApproval = dbContext.ParameterApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (parameterApproval != null)
                {
                    parameterApproval.ApprovalStatus = approvalStatusId;
                    parameterApproval.ModifiedBy = access.CreatedBy;
                    parameterApproval.ModifiedOn = DateTime.Now;

                    if (approvalStatusId == reviewerStatusId && !isAdmin)
                    {
                        access.CreatedBy = dbContext.ParameterApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                        dbContext.ParameterApproval.Add(new ParameterApproval
                        {
                            //UserId = access.UserId,
                            ManagerId = newManagerId,
                            ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = approvedStatusId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }

            if (approvalStatusId == approvedStatusId || approvalStatusId == approvedStatusId && isAdmin)
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();

            return true;
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

        public async Task<string> GetNextParameterCode()
        {
            using (var connection = unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[Parameter]");
                int nextNumber = count + 1;
                return $"{ReferencePrefixes.Parameter}{nextNumber:D3}";
            }
        }
    }
}

