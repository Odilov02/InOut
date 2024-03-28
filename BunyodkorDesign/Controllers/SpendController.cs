using Microsoft.AspNetCore.RateLimiting;

namespace WebUI.Controllers;

[EnableRateLimiting("fixed")]
public class SpendController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public SpendController(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    [Authorize(Roles = "User")]
    public IActionResult AddSpend()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        var spendTypes = _appDbContext.SpendTypes.ToList();
        ViewData["SpendTypes"] = spendTypes;
        return View();
    }



    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> AddSpend(AddSpendDto SpendDto)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        if (!ModelState.IsValid)
        {
            ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
            return View(SpendDto);
        }
        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

        Spend spend = _mapper.Map<Spend>(SpendDto);
        spend.SpendType = (await _appDbContext.SpendTypes.FirstOrDefaultAsync(x => x.Id == spend.SpendType.Id))!;
        spend.Date = DateTime.Now;
        spend.UserId = user!.Id;
        await _appDbContext.Spends.AddAsync(spend);
        var result = await _appDbContext.SaveChangesAsync();
        ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
        ViewData["result"] = result;
        return View();
    }



    [Authorize(Roles = "User")]
    public async Task<IActionResult> Choose()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
        return View();
    }




    [Authorize(Roles = "User")]
    public IActionResult GetAllConfirmed()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        List<Spend> spends = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == true && x.UserId.ToString() == userId).ToList();
        return View(spends);
    }




    [Authorize(Roles = "User")]
    public IActionResult GetAllNoConfirmed()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        List<Spend> spends = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && x.UserId.ToString() == userId).ToList();
        return View(spends);
    }





    //<<=====        Admin Action        ========>>

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ConfirmSpend(Guid constructionId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<Spend> spends = _appDbContext.Spends.Where(x => x.User.Id == construction!.User.Id && x.IsConfirmed == false).ToList();
        var spendsConfirming = new SpendsConfirming()
        {
            ConstructionId = constructionId,
            Spends = spends
        };
        return View(spendsConfirming);
    }




    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> ConfirmSpend(SpendsConfirming spendsConfirming)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == spendsConfirming.ConstructionId);

        List<Spend> entities = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && spendsConfirming.Spends.Any(y => y.Id == x.Id && y.IsConfirmed == true)).ToList();
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                foreach (var item in entities)
                {
                    construction!.User!.Residual -= item.Price;
                    item!.IsConfirmed = true;
                }
                _appDbContext.Spends.UpdateRange(entities!);
                var result = await _appDbContext.SaveChangesAsync();
                if (result > 0)
                {
                    foreach (var item in entities)
                    {
                        if (item.IsConfirmed)
                        {
                            construction!.Spend += item.Price;
                            construction.SpendDate = DateTime.Now;
                        }
                    }
                    _appDbContext.Constructions.Update(construction!);
                    await _appDbContext.SaveChangesAsync();
                    entities = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && x.UserId == construction!.UserId).ToList()!;
                    var resultSpendsConfirming = new SpendsConfirming()
                    {
                        ConstructionId = spendsConfirming.ConstructionId,
                        Spends = entities
                    };
                    transaction.Commit();
                    ViewData["result"] = result-1;
                    return View(resultSpendsConfirming);
                }
                else
                {
                    throw new();
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        return View(spendsConfirming);
    }




    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllConfirmedForAdmin(Guid constructionId)
    {

        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        ViewData["constructionId"] = constructionId;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<AllSpend> allSpends = new List<AllSpend>();
        List<Spend> spends = await _appDbContext.Spends.Where(x => x.User.Id == construction!.UserId && x.IsConfirmed == true).ToListAsync();
        if (spends is not null)
        {
            foreach (var item in spends)
            {
                AllSpend allSpend = new AllSpend()
                {
                    AdminOrUser = "P",
                    Date = item.Date,
                    Price = item.Price,
                    Reason = item.Reason,
                    SpendType = item.SpendType.Name
                };
                allSpends.Add(allSpend);
            }
        }
        var spendAdmin = _appDbContext.AdminSpends.Where(x => x.ConstructionId == constructionId).ToList();
        if (spendAdmin is not null)
        {
            foreach (var item in spendAdmin)
            {
                AllSpend allSpend = new AllSpend()
                {
                    AdminOrUser = "A",
                    Date = item.CreatedDate,
                    Price = item.Price ?? 0,
                    Reason = item.Reason,
                    SpendType = item.SpendType.Name
                };
                allSpends.Add(allSpend);
            }
        }
        allSpends = allSpends.OrderByDescending(x => x.Date).ToList();
        return View(allSpends);
    }





    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        ViewData["constructionId"] = constructionId;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        var spends = await _appDbContext.Spends.Where(x => x.User.Id == construction!.UserId && x.IsConfirmed == false).ToListAsync();
        return View(spends);
    }




    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllPersonalSpend()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        string? userId = HttpContext.Session.GetString("AdminId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        ViewData["Residual"] = (await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId))!.Residual;
        List<Spend> spends = await _appDbContext.Spends.Where(x => x.UserId.ToString() == userId).ToListAsync();
        return View(spends);
    }




    [Authorize(Roles = "Admin")]
    public IActionResult AddSpendPersonal()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        var spendTypes = _appDbContext.SpendTypes.ToList();
        ViewData["SpendTypes"] = spendTypes;
        return View();
    }




    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddSpendPersonal(AddSpendDto SpendDto)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        if (!ModelState.IsValid)
        {
            ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
            return View(SpendDto);
        }
        string? userId = HttpContext.Session.GetString("AdminId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

        Spend spend = _mapper.Map<Spend>(SpendDto);
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                spend.SpendType = (await _appDbContext.SpendTypes.FirstOrDefaultAsync(x => x.Id == spend.SpendType.Id))!;
                spend.Date = DateTime.Now;
                spend.UserId = user!.Id;
                await _appDbContext.Spends.AddAsync(spend);
                var result = await _appDbContext.SaveChangesAsync();
                if (result > 0)
                {
                    user.Residual -= spend.Price;
                    _appDbContext.Users.Update(user);
                    await _appDbContext.SaveChangesAsync();
                    transaction.Commit();
                    ViewData["result"] = result;
                    ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
                    return View();
                }
                else
                {
                    throw new();
                }

            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
        return View();
    }
}
