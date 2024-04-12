using Microsoft.AspNetCore.RateLimiting;

namespace WebUI.Controllers;
public class ConstructionController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IDateTimeService _dateTime;

    public ConstructionController(IAppDbContext appDbContext, IMapper mapper, UserManager<User> userManager, IDateTimeService dateTime)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _userManager = userManager;
        _dateTime = dateTime;
    }

    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetResidual()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.UserId.ToString() == userId);
        return View(construction);
    }





    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllConstruction()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        var constructions = await _appDbContext.Constructions.OrderByDescending(x => x.CreatedDate).ToListAsync();
        return View(constructions);
    }




    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddConstruction()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        var usersAll = _userManager.Users.ToList();
        List<User> users = new List<User>();
        foreach (var user in usersAll)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
                users.Add(user);
        }
        ViewData["users"] = users;
        return View();
    }




    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddConstruction(AddConstructionDto constructionDto)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        if (!ModelState.IsValid)
        {
            var usersAll = _userManager.Users.ToList();
            List<User> users = new List<User>();
            foreach (var item in usersAll)
            {
                var roles = await _userManager.GetRolesAsync(item);
                if (roles.Count == 0)
                    users.Add(item);
            }
            ViewData["users"] = users;
            return View(constructionDto);
        }
        var construction = _mapper.Map<Construction>(constructionDto);
        construction.CreatedDate = _dateTime.NowTime();
        construction.SpendDate = _dateTime.NowTime();
        construction.InDate = _dateTime.NowTime();
        var user = _userManager.Users.FirstOrDefault(x => x.Id == construction.UserId);
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                await _appDbContext.Constructions.AddAsync(construction);
                var result = await _appDbContext.SaveChangesAsync();
                if(result<=0)
                {
                    throw new();
                }

                IdentityResult r = await _userManager.AddToRoleAsync(user!, "User");
                if(!r.Succeeded)
                {
                    throw new();
                }
                transaction.Commit();
                return RedirectToAction(actionName: "GetAllConstruction", controllerName: "Construction");
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        var newUsersAll = _userManager.Users.ToList();
        List<User> newUsers = new List<User>();
        foreach (var item in newUsersAll)
        {
            var newRoles = await _userManager.GetRolesAsync(item);
            if (newRoles.Count == 0)
                newUsers.Add(item);
        }
        ViewData["result"] = 0;
        ViewData["users"] = newUsers;
        return View();
    }



    [Authorize(Roles = "Admin")]
    public IActionResult Choose(Guid constructionId) => View(constructionId);


    [Authorize(Roles = "Admin")]
    public IActionResult GetAllDetails()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        List<Construction> constructions = _appDbContext.Constructions.OrderByDescending(x=>x.CreatedDate).ToList();
        return View(constructions);
    }

                //  Super Admin   

    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAllDetailsForSuperAdmin()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        var constructions = await _appDbContext.Constructions.OrderByDescending(x => x.CreatedDate).ToListAsync();
        foreach (var construction in constructions)
        {
            var outs = construction.Outs;
            foreach (var @out in outs)
            {
                construction.Spend += @out!.Price;
            }
        }
        return View(constructions);
    }

}
