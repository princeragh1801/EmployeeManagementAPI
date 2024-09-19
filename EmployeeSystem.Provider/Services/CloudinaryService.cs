using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace EmployeeSystem.Provider.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService()
        {
            Account account = new Account(
                Environment.GetEnvironmentVariable("CLOUDINARY_CLOUDNAME"),
                Environment.GetEnvironmentVariable("CLOUDINARY_KEY"),
                Environment.GetEnvironmentVariable("CLOUDINARY_SECRET")
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                if(file.Length > 0)
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                        PublicId = Path.GetFileNameWithoutExtension(file.FileName)
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    if (uploadResult.Error != null)
                    {
                        throw new Exception(uploadResult.Error.Message);
                    }
                    Console.WriteLine("Upload result : " + uploadResult);
                    return uploadResult.SecureUrl.ToString();
                }
                throw new Exception("File is empty");
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
