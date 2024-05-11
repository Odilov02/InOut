using WebUI.Services.Interfaces;

namespace WebUI.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string GetFilePath(string fileName)
    {
        return Path.Combine(_webHostEnvironment.WebRootPath, "Documents", fileName);
    }
    public bool RemoveFile(string fileName)
    {
        try
        {

            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return false;
            }

            System.IO.File.Delete(filePath);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string?> SaveFileAsync(IFormFile file)
    {
        try
        {
            if (file.Length <= 0 || file is null) return null;

            var fileName = Guid.NewGuid() + file.FileName;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Documents", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
        catch (Exception e)
        {

            return null;
        }
      
    }

}
