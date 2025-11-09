using Dapper;
using ComplianceAPI.Models;
using ComplianceAPI.Services;

namespace ComplianceAPI.Repository
{
    public interface IFolderRepository
    {
            Task<bool> CreateFolder( Folder folder);
            Task<List<Folder>> GetFoldersListByEntity(int entityId);
            Task<bool> DeleteFolder( int folderId);
            Task<List<FolderTreeNode>> GetFolderTreeAsync(int entityId,int userId, string type);
    }

    public class FolderRepository : IFolderRepository
    {
        private readonly IWebHostEnvironment _environment;

        protected readonly IUnitOfWork _unitOfWork;


        public FolderRepository(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }
        public async Task<bool> CreateFolder(Folder folder)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var result = await sqlContext.Connection.QueryAsync<bool>("USP_CREATEFOLDER", folder, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }

                 return true;

            }

        }
      

        public async Task<List<Folder>> GetFoldersListByEntity(int entityId)
        {

            using (var connection = _unitOfWork.ConnectionFactory())
            {
               
                using (var mul = await connection.QueryMultipleAsync("select * from Folders", commandType: System.Data.CommandType.Text))
                {
                    var Result = mul.Read<Folder>().ToList();
                    mul.Dispose();
                    return Result;
                }
               
            }
        }


        public Task<bool> DeleteFolder(int folderId)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", folderId.ToString());
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
               
                return Task.FromResult(true);
            }


            return Task.FromResult(false);
        }
        public async Task<List<FolderTreeNode>> GetFolderTreeAsync(int entityId, int userId, string type)
        {
            var query = "";
            if (type == "compseqr360")
            {
                query = "SELECT * FROM Folders where module_type <> 'Dms'";
            }
            else {
             query="SELECT * FROM Folders where module_type=@mtype";
            }
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var folders = await connection.QueryAsync<Folder>(query, new {mtype=type});

                // Convert folders to hierarchical structure
                var rootFolders = folders.Where(f => f.ParentId == 0).ToList();
                var folderTree = BuildTree(rootFolders, folders);

                return folderTree;
            }
        }

        private List<FolderTreeNode> BuildTree(IEnumerable<Folder> rootFolders, IEnumerable<Folder> allFolders)
        {
            var tree = new List<FolderTreeNode>();

            foreach (var folder in rootFolders)
            {
                var node = new FolderTreeNode
                {   
                    Id = folder.Id,
                    Label = folder.FolderName,
                    Expanded = false,
                    ParentId = folder.ParentId,
                    Children = BuildTree(allFolders.Where(f => f.ParentId == folder.Id), allFolders)
                };

                tree.Add(node);
            }

            return tree;
        }


    }

}
