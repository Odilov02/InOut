using Microsoft.AspNetCore.RateLimiting;

namespace WebUI.Controllers;
[EnableRateLimiting("fixedWindow")]
public class ConstructionController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    public ConstructionController(IAppDbContext appDbContext, UserManager<User> userManager, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _mapper = mapper;
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
        string? userId = HttpContext.Session.GetString("AdminId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
        ViewData["FullName"] = user!.FullName;
        ViewData["PhoneNumber"] = user.PhoneNumber;
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
        var user = _userManager.Users.FirstOrDefault(x => x.Id == construction.UserId);
        await _appDbContext.Constructions.AddAsync(construction);
        var result = await _appDbContext.SaveChangesAsync();
        await _userManager.AddToRoleAsync(user!, "User");
        ViewData["result"] = result;
        var newUsersAll = _userManager.Users.ToList();
        List<User> newUsers = new List<User>();
        foreach (var item in newUsersAll)
        {
            var newRoles = await _userManager.GetRolesAsync(item);
            if (newRoles.Count == 0)
                newUsers.Add(item);
        }
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


        List<Construction> constructions = _appDbContext.Constructions.ToList();
        return View(constructions);
    }



    [Authorize(Roles = "Admin")]
    public IActionResult AddAdminSpend(Guid constructionId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        var adminSpend = new AdminSpend()
        {
            ConstructionId = constructionId
        };
        var spendTypes = _appDbContext.SpendTypes.ToList();
        ViewData["SpendTypes"] = spendTypes;
        return View(adminSpend);
    }
     


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddAdminSpend(AdminSpend adminSpend)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        var spendTypes = _appDbContext.SpendTypes.ToList();
        if (!ModelState.IsValid)
        {
            ViewData["SpendTypes"] = spendTypes;
            return View(adminSpend);
        }
        Construction? construction = _appDbContext.Constructions.FirstOrDefault(x => x.Id == adminSpend.ConstructionId);
        if (construction == null)
        {
            ViewData["SpendTypes"] = spendTypes;
            return View(adminSpend);
        }
        string? userId = HttpContext.Session.GetString("AdminId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                _appDbContext.AdminSpends.Add(adminSpend);
                var result = await _appDbContext.SaveChangesAsync();
                if(result<=0)
                {
                    throw new();
                }
                construction.Spend += adminSpend.Price ?? 0;
                construction.SpendDate = DateTime.Now;
                _appDbContext.Constructions.Update(construction);
              result=  await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new();
                }
               if(adminSpend.IsCash??false)
                {
                    var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
                    user!.Residual -= adminSpend.Price ?? 0;
                    _appDbContext.Users.Update(user);

                    result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                    {
                        throw new();
                    }
                }
                var newAdminSpend = new AdminSpend()
                {
                    ConstructionId = construction.Id
                };
                ViewData["SpendTypes"] = spendTypes;
                ViewData["result"] = result;
                transaction.Commit();
                return View(newAdminSpend);
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
         }
        return View(adminSpend);
    }
}
