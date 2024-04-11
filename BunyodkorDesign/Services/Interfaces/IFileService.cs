namespace WebUI.Services.Interfaces;

public interface IFileService
{
    public Task<string?> SaveFileAsync(IFormFile file);
    public bool RemoveFile(string fileName);

    public string GetFilePath(string fileName);
}

