using Microsoft.AspNetCore.Hosting;
using WebUI.Models;
using WebUI.Services.Interfaces;

namespace WebUI.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IDateTimeService _dateTime;

        public DocumentController(IAppDbContext appDbContext, IFileService fileService, IDateTimeService dateTime)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _dateTime = dateTime;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetAllFile()
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
            var documents = _appDbContext.Documents.ToList();
            documents = documents.OrderByDescending(x => x.Time).ToList();
            return View(documents);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AddFile()
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddFile(AddFile file)
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
            if (!ModelState.IsValid)
                return View(file);

            var imgUrl = await _fileService.SaveFileAsync(file.FormFile);
            if (imgUrl == null)
                return View(file);

            var document = new Document()
            {
                ImgUrl = imgUrl,
                Name = file.Name,
                Time = _dateTime.NowTime(),
                Description = file.Description,
            };
            await _appDbContext.Documents.AddAsync(document);
            var result = await _appDbContext.SaveChangesAsync();
            if (result <= 0)
                return View(file);
            return RedirectToAction("GetAllFile");

        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
            var document = await _appDbContext.Documents.FirstOrDefaultAsync(x => x.Id == id);
            if (document is null)
                return RedirectToAction("GetAllFile");
            using (var transaction = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    string imgUrl = document.ImgUrl;
                    _appDbContext.Documents.Remove(document);
                    var result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                        throw new();
                    var resultBool = _fileService.RemoveFile(imgUrl);
                    if (!resultBool)
                        throw new();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            var documents = _appDbContext.Documents.ToList();
            documents = documents.OrderByDescending(x => x.Time).ToList();
            return View("GetAllFile", documents);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
            Document? document =await _appDbContext.Documents.FirstOrDefaultAsync(x=>x.Id==id);
            if (document is null)
                return RedirectToAction(actionName:"GetAllFile",controllerName:"Document");

            var fileName = document.ImgUrl;
            var path = _fileService.GetFilePath(fileName);
            var fileStream = new FileStream(path, FileMode.Open);
            return File(fileStream, "application/octet-stream", fileName);
        }
    }
}
