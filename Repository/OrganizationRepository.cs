using DmsApi.Helpers;
using DmsApi.Models;
using Dapper;
using Dms_Api.Models;
using DmsApi.Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dms_Api.Repository
{
    public interface IOrganizationRepository
    {
        Task<Organization> AddOrganisation(Organization organization);
        Task<List<Organization>> GetAllOrganizations();

        Task<bool> DeleteOrganization(int id);
        Task<Organization> AddOrganization(Organization organization);
    }

    // Repositories/OrganisationRepository.cs
    // Repository/OrganisationRepository.cs
    public class OrganisationRepository : IOrganizationRepository
    {
        protected readonly IUnitOfWork _unitOfWork;

        public OrganisationRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<Organization>> GetAllOrganizations()
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var result = await sqlContext.Connection.QueryAsync<Organization>(
                    "USP_GETALLORGANISATIONS",
                    commandType: System.Data.CommandType.StoredProcedure,
                    transaction: sqlContext.Transaction);

                sqlContext.Commit();
                return result.ToList();
            }
        }


        public async Task<bool> DeleteOrganization(int id)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.ExecuteAsync(
                    "USP_DELETEORGANISATION",
                    new { Id = id },
                    commandType: System.Data.CommandType.StoredProcedure,
                    transaction: sqlContext.Transaction);

                sqlContext.Commit();
                return true;
            }
        }

        public Task<Organization> AddOrganisation(Organization organization)
        {
            throw new NotImplementedException();

        }

        public async Task<Organization> AddOrganization(Organization organization)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var queryOrg = @"
    INSERT INTO Organizations (OrgName, Country, State, City, Address)
    OUTPUT INSERTED.Id  -- Return the auto-generated Id
    VALUES (@OrgName, @Country, @State, @City, @Address)";

                    // Execute the query using Dapper and return the new Id
                    var newOrgId = await sqlContext.Connection.QuerySingleAsync<int>(
                        queryOrg,
                        new
                        {
                            OrgName = organization.OrganizationName,  // Ensure property names match the SQL query variables
                            Country = organization.Country,
                            State = organization.State,
                            City = organization.City,
                            Address = organization.Address
                        },
                        transaction: sqlContext.Transaction
                    ).ConfigureAwait(false);

                    var queryUser = @"
    INSERT INTO [dbo].[Users]([EmpId],[FirstName],[LastName],[FullName],[Email],[Mobile],[Password],[Status],[CreatedOn])
    OUTPUT INSERTED.Id  -- Return the auto-generated Id
    VALUES (@EmpId, @FirstName, @LastName, @FullName, @Email, @Mobile, @Password, @Status, @CreatedOn)";

                    // Execute the query using Dapper and return the new Id
                    var newUserId = await sqlContext.Connection.QuerySingleAsync<int>(
                        queryUser,
                        new
                        {
                            EmpId = 1001,  // Ensure property names match the SQL query variables
                            FirstName = organization.OrganizationName,
                            LastName = organization.OrganizationName,
                            FullName = organization.OrganizationName,
                            Email = organization.emailAddress,
                            Password = organization.password,
                            Mobile = 12346789,
                            Status = 1,
                            CreatedOn = DateTime.Now,
                        },
                        transaction: sqlContext.Transaction
                    ).ConfigureAwait(false);

                    var queryUserRoleMapping = @"
    INSERT INTO [dbo].[UserRoleMapping]
    ([UserId], [RoleId], [Status], [CreatedOn])
    OUTPUT INSERTED.Id  -- Return the auto-generated Id
    VALUES (@UserId, @RoleId, @Status, @CreatedOn)";

                    // Execute the query using Dapper and return the new Id
                    var newUserRoleMappingId = await sqlContext.Connection.QuerySingleAsync<int>(
                        queryUserRoleMapping,
                        new
                        {
                            UserId = newUserId,  // Ensure property names match the SQL query variables
                            RoleId = 2,
                            Status = 1,
                            CreatedOn = DateTime.Now,
                        },
                        transaction: sqlContext.Transaction
                    ).ConfigureAwait(false);

                    var queryCreateFolder = @"
    INSERT INTO [dbo].[Folders] ([FolderName],[ParentId],[UserId],[IsParent],[EntityId],[CreatedOn])
    OUTPUT INSERTED.Id  -- Return the auto-generated Id
    VALUES (@FolderName, @ParentId, @UserId, @IsParent, @EntityId, @CreatedOn)"; // Fixed comma error

                    // Execute the query using Dapper and return the new Id
                    var newFolderId = await sqlContext.Connection.QuerySingleAsync<int>(
                        queryCreateFolder,  // Use the correct query variable
                        new
                        {
                            FolderName = "COMPSEQR360",  // Ensure property names match the SQL query variables
                            ParentId = 2,
                            UserId = newUserId,
                            IsParent = true,
                            EntityId = newOrgId,
                            CreatedOn = DateTime.Now,
                        },
                        transaction: sqlContext.Transaction
                    ).ConfigureAwait(false);

                    // Commit the transaction after a successful operation
                    sqlContext.Commit();

                    // Assign the new Id to the organization object
                    organization.Id = newOrgId;

                    // Return the newly created organization
                    return organization;
                }
                catch (Exception)
                {
                    // Rollback the transaction in case of an error
                    sqlContext.Rollback();
                    throw;  // Re-throw the exception to ensure it's handled elsewhere
                }
            }
        }

    }

}
