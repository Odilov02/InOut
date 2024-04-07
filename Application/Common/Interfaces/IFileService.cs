using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IFileService
{
    public Task<string?> SaveFile(IFormFile file);
    public Task<bool?> RemoveFile(string fileName);
}
