using MessagingPlatformAPI.CloudinaryConfigs;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface ICloudinaryService
    {
        Task<CloudinaryUploadResponse> UploadFile(IFormFile file);
        Task<CloudinaryUploadMultipleResponse> UploadFiles(List<IFormFile> files);
        Task<CloudinaryDeleteResponse> DeleteFile(string FileId);
    }
}
