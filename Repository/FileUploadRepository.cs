using Dapper;
using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Services;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Repository
{
    public interface IFileUploadRepository
    {
            Task<bool> SaveFileDetails(FileDetail fileDetail);
            Task<List<FileDetail>> GetFilesListbyFolder(int folderId,string type);
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
                var data = new { fileDetail.FileName, fileDetail.FileType, fileDetail.FilePath, fileDetail.FolderId, fileDetail.UserId };
                try
                {
                    var obj = await sqlContext.Connection.QueryAsync<FileDetail>("USP_CREATEFILE", data, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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

        public async Task<List<FileDetail>> GetFilesListbyFolder(int folderId, string type)
        {
            using var connection = _unitOfWork.ConnectionFactory();

            var query = "";
            if (type == "Dms")
            {
                query = @"
                   ;WITH RootFolders AS (
                -- Resolve @ParentId to actual folder Id(s) via Id OR EntityId
                SELECT Id
                FROM Folders
                WHERE (Id = @ParentId
                   OR EntityId = @ParentId) and module_type='Dms'
            ),
            RecursiveFolders AS (
                -- Start from resolved root folder(s)
                SELECT Id
                FROM RootFolders

                UNION ALL

                -- Recurse down the folder tree
                SELECT f.Id
                FROM Folders f
                INNER JOIN RecursiveFolders rf ON f.ParentId = rf.Id
            )
            SELECT 
                fi.*, 
                u.FullName, 
                f.FolderName 
            FROM Files fi
            INNER JOIN Users u ON fi.UserId = u.Id
            INNER JOIN Folders f ON fi.FolderId = f.Id
            WHERE fi.FolderId IN (
                SELECT Id FROM RecursiveFolders
            );";
         }
            else if (type == "compseqr360")
            {
                query = @"
        ;WITH RootFolders AS (
            SELECT Id
            FROM Folders
            WHERE  module_type != 'Dms'
              
        ),
        RecursiveFolders AS (
            SELECT Id FROM RootFolders
            UNION ALL
            SELECT f.Id
            FROM Folders f
            INNER JOIN RecursiveFolders rf ON f.ParentId = rf.Id
        )
        SELECT 
            fi.*, 
            u.FullName, 
            f.FolderName 
        FROM Files fi
        INNER JOIN Users u ON fi.UserId = u.Id
        INNER JOIN Folders f ON fi.FolderId = f.Id
        WHERE fi.FolderId IN (SELECT Id FROM RecursiveFolders);";

            }
            else
            {
                query = @"
        ;WITH RootFolders AS (
            SELECT Id
            FROM Folders
            WHERE (Id = @ParentId OR EntityId = @ParentId)
              AND module_type = @type
        ),
        RecursiveFolders AS (
            SELECT Id FROM RootFolders
            UNION ALL
            SELECT f.Id
            FROM Folders f
            INNER JOIN RecursiveFolders rf ON f.ParentId = rf.Id
        )
        SELECT 
            fi.*, 
            u.FullName, 
            f.FolderName 
        FROM Files fi
        INNER JOIN Users u ON fi.UserId = u.Id
        INNER JOIN Folders f ON fi.FolderId = f.Id
        WHERE fi.FolderId IN (SELECT Id FROM RecursiveFolders);";
            }
            

            var result = await connection.QueryAsync<FileDetail>(
                query,
                new { ParentId = folderId, type }
            );

            return result.ToList();
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
