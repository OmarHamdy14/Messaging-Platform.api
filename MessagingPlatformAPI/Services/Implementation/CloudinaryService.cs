using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MessagingPlatformAPI.CloudinaryConfigs;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.Extensions.Options;
using System.Security.Principal;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IOptions<CloudinarySettings> options;
        public CloudinaryService()
        {
            var acc = new Account()
            {
                Cloud = options.Value.CloudName,
                ApiKey = options.Value.APIKey,
                ApiSecret = options.Value.APISecret
            };
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<CloudinaryUploadResponse> UploadFile(IFormFile file)
        {
            if (file == null || file.Length < 0)
            {
                return new CloudinaryUploadResponse()
                {
                    IsSuccess = false,
                    Message = "Invalid File"
                };
            }
            var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream)
            };
            var UploadResult = await _cloudinary.UploadAsync(uploadParams);
            return new CloudinaryUploadResponse()
            {
                Url = UploadResult.SecureUri.AbsoluteUri,
                PublicId = UploadResult.PublicId,
                IsSuccess = true,
                Message = "Uploading image is succeeded"
            };
        }
        public async Task<CloudinaryDeleteResponse> DeleteFile(string FileId)
        {
            if (string.IsNullOrEmpty(FileId))
            {
                return new CloudinaryDeleteResponse()
                {
                    IsSuccess = false,
                    Message = "FileId is null"
                };
            }
            var deleteParams = new DeletionParams(FileId);
            var res = await _cloudinary.DestroyAsync(deleteParams);
            if (res.Result == "deleted")
            {
                return new CloudinaryDeleteResponse()
                {
                    IsSuccess = true,
                    Message = "Deletion is done"
                };
            }
            return new CloudinaryDeleteResponse()
            {
                IsSuccess = false,
                Message = $"Failed to delete this image!! {res.Error.Message}"
            };
        }
    }

}
