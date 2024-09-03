using Microsoft.AspNetCore.Http;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ICloudinaryService
    {
        public Task<string> UploadFile(IFormFile file);
    }
}
