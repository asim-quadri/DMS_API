using Dapper;
using DmsApi.Models;
using DmsApi.Services;

namespace DmsApi.Repository
{
    public interface IFolderService
    {
            Task<bool> CreateFolder( Folder folder);
            Task<List<Folder>> GetFoldersListByEntity(int entityId);
            Task<bool> DeleteFolder( int folderId);

            Task<List<FolderTreeNode>> GetFolderTreeAsync(int intityId, int userId);
    }

    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;

        public FolderService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }
        public async Task<bool> CreateFolder(Folder folder)
        {
            return await _folderRepository.CreateFolder(folder);

        }
        public async Task<List<Folder>> GetFoldersListByEntity(int entityId)
        {
            return await  _folderRepository.GetFoldersListByEntity(entityId);
        }

        public async Task<List<FolderTreeNode>> GetFolderTreeAsync(int entityId, int userId)
        {
            return await _folderRepository.GetFolderTreeAsync(entityId,userId);
        }

        public Task<bool> DeleteFolder(int folderId)
        {
            return Task.FromResult(false);
        }

    }

}
