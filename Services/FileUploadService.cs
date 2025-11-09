using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface IFileUploadService
    {
        Task<List<FileDetail>> GetFilesbyFolder(int folderId,string type);
        Task<bool> DeleteFile( int fileId);
        Task<bool> SaveFileDetails(FileDetail file);
    }
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileUploadRepository _fileUploadRepository;

        public FileUploadService(IFileUploadRepository fileuploadRepository)
        {
            _fileUploadRepository = fileuploadRepository;
        }
        public async Task<bool> SaveFileDetails(FileDetail file)
        {
            return await _fileUploadRepository.SaveFileDetails(file);
        }
        public async Task<List<FileDetail>> GetFilesbyFolder(int folderId, string type)
        {
            return await _fileUploadRepository.GetFilesListbyFolder(folderId,type);

        }
        public async Task<bool> DeleteFile( int fileId)
        {
            return await _fileUploadRepository.DeleteFile(fileId);
        }


    }
}
