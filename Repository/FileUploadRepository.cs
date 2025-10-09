using Dapper;
using DmsApi.Helpers;
using DmsApi.Models;
using DmsApi.Services;

namespace DmsApi.Repository
{
    public interface IFileUploadRepository
    {
            Task<bool> SaveFileDetails(FileDetail fileDetail);
            Task<List<FileDetail>> GetFilesListbyFolder(int folderId);
            Task<bool> DeleteFile( int fileId);

    }

    public class FileUploadRepository : IFileUploadRepository
    {
        private readonly IWebHostEnvironment _environment;

        protected readonly IUnitOfWork _unitOfWork;
        public FileUploadRepository(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }
        //public async Task<string> SaveFileDetails(FileDetail fileDetail)
        //{
        //    if (fileDetail == null || fileDetail.FileName.Length == 0)
        //        throw new ArgumentException("No file uploaded.");

        //    using (var sqlContext = _unitOfWork.ContextFactory())
        //    {
        //        var mul = await sqlContext.Connection.QueryAsync<string>("USP_CREATEFILE", fileDetail, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);

        //        sqlContext.Commit();
        //        return mul;
        //    }

        //}

        public async Task<bool> SaveFileDetails(FileDetail fileDetail)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var obj = await sqlContext.Connection.QueryAsync<FileDetail>("USP_CREATEFILE", fileDetail, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }

            return true;


        }

        public async Task<List<FileDetail>> GetFilesListbyFolder(int folderId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                // Define the SQL query with a parameter placeholder
                var query = @"
        ;WITH RecursiveFolders AS (
            SELECT 
                Id
            FROM 
                Folders
            WHERE 
                ParentId = @ParentId
            
            UNION ALL
            
            SELECT 
                f.Id
            FROM 
                Folders f
            INNER JOIN 
                RecursiveFolders rf 
            ON 
                f.ParentId = rf.Id
        )
        SELECT * 
        FROM Files
        WHERE FolderId IN (
            SELECT Id FROM RecursiveFolders
            UNION
            SELECT @ParentId -- Include the root folder itself
        );
    ";

                // Use QueryMultipleAsync with the parameter
                using (var mul = await connection.QueryMultipleAsync(query, new { ParentId = folderId }))
                {
                    // Read the results into a list of FileDetail
                    var result = mul.Read<FileDetail>().ToList();
                    return result;
                }
            }
        }


        public Task<bool> DeleteFile(int fileId)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileId.ToString());
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }


    }

}
