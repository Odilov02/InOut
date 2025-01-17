namespace WebUI.Controllers;

public class SpendController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly IDateTimeService _dateTime;

    public SpendController(IAppDbContext appDbContext, IMapper mapper, IDateTimeService dateTime)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    [Authorize(Roles = "User")]
    public IActionResult AddSpend()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        ViewData["Construction"] = HttpContext.Session.GetString("Construction");

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
        ViewData["Construction"] = HttpContext.Session.GetString("Construction");


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
        spend.SpendType = (await _appDbContext.SpendTypes.FirstOrDefaultAsync(x => x.Id == spend.SpendTypeId))!;
        spend.Date = _dateTime.NowTime();
        spend.UserId = user!.Id;
        spend.ConstructionId = user.Construction!.Id;
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
        ViewData["Construction"] = HttpContext.Session.GetString("Construction");

        return View();
    }




    [Authorize(Roles = "User")]
    public IActionResult GetAllConfirmed()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        ViewData["Construction"] = HttpContext.Session.GetString("Construction");


        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        List<Spend> spends = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == true && x.UserId.ToString() == userId).OrderByDescending(x => x.Date).ToList();
        return View(spends);
    }




    [Authorize(Roles = "User")]
    public IActionResult GetAllNoConfirmed()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        ViewData["Construction"] = HttpContext.Session.GetString("Construction");


        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        List<Spend> spends = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && x.UserId.ToString() == userId).OrderByDescending(x => x.Date).ToList();
        return View(spends);
    }



    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteSpend(Guid id)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        ViewData["Construction"] = HttpContext.Session.GetString("Construction");

        var spend = await _appDbContext.Spends.FirstOrDefaultAsync(x => x.Id == id);
        if (spend == null) throw new();
        Guid? constructionId = spend.ConstructionId;


        _appDbContext.Spends.Remove(spend);
        await _appDbContext.SaveChangesAsync();
        if (constructionId == null) throw new();
        return RedirectToAction(actionName: "GetAllNoConfirmed", controllerName: "Spend", new { constructionId = constructionId });
    }



    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateSpend(Guid id)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        ViewData["Construction"] = HttpContext.Session.GetString("Construction");

        var spendTypes = _appDbContext.SpendTypes.ToList();
        ViewData["SpendTypes"] = spendTypes;

        var spend = await _appDbContext.Spends.FirstOrDefaultAsync(x => x.Id == id);
        if (spend == null) throw new();
        var spendDto = _mapper.Map<UpdateSpendDto>(spend);
        return View(spendDto);
    }




    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> UpdateSpend(UpdateSpendDto spendDto)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        ViewData["Construction"] = HttpContext.Session.GetString("Construction");


        if (!ModelState.IsValid)
        {
            ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
            return View(spendDto);
        }

        Spend? spend = await _appDbContext.Spends.FirstOrDefaultAsync(x => x.Id == spendDto.Id);
        if (spend is null) throw new();

        spend.SpendType = (await _appDbContext.SpendTypes.FirstOrDefaultAsync(x => x.Id == spendDto!.SpendTypeId!))!;
        spend.Price = spendDto.Price ?? 0;
        spend.Reason = spendDto.Reason;

        _appDbContext.Spends.Update(spend);
        var result = await _appDbContext.SaveChangesAsync();
        if (result >= 0)
        {
            Guid? constructionId = spend.ConstructionId;
            return RedirectToAction(actionName: "GetAllNoConfirmed", controllerName: "Spend", new { constructionId = constructionId });
        }
        ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
        ViewData["result"] = result;
        return View(spendDto);
    }




    //<<=====        Admin Action        ========>>

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ConfirmSpend(Guid constructionId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<Spend> spends = _appDbContext.Spends.Where(x => x.ConstructionId == construction!.Id && x.IsConfirmed == false).OrderByDescending(x => x.Date).ToList();
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

        List<Spend> entities = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && spendsConfirming.Spends.Any(y => y.Id == x.Id && y.IsConfirmed == true)).OrderByDescending(x => x.Date).ToList();
        List<Spend> entitiesFalse = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && spendsConfirming.Spends.Any(y => y.Id == x.Id && y.IsConfirmed == false)).OrderByDescending(x => x.Date).ToList();
        foreach (var item in entitiesFalse)
        {
            item.Comment = spendsConfirming.Spends.FirstOrDefault(x => x.Id == item.Id)!.Comment;
            _appDbContext.Spends.Update(item);
            await _appDbContext.SaveChangesAsync();
        }
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                foreach (var item in entities)
                {
                    item!.IsConfirmed = true;
                    _appDbContext.Spends.UpdateRange(entities!);
                    var result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                    {
                        throw new();
                    }
                    construction!.Spend += item.Price;
                    construction.SpendDate = _dateTime.NowTime();
                    _appDbContext.Constructions.Update(construction!);

                    result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                    {
                        throw new();
                    }
                    construction.User.Residual -= item.Price;
                    _appDbContext.Users.Update(construction.User!);
                    result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                    {
                        throw new();
                    }
                }
                ViewData["result"] = entities.Count;
                entities = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && x.UserId == construction!.UserId).ToList()!;
                var resultSpendsConfirming = new SpendsConfirming()
                {
                    ConstructionId = spendsConfirming.ConstructionId,
                    Spends = entities
                };
                transaction.Commit();
                return View(resultSpendsConfirming);

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
        ViewData["result"] = HttpContext.Session.GetInt32("result");
        HttpContext.Session.Remove("result");
        ViewData["constructionId"] = constructionId;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<AllSpend> allSpends = new List<AllSpend>();
        List<Spend> spends = await _appDbContext.Spends.Where(x => x.ConstructionId == construction!.Id && x.IsConfirmed == true).ToListAsync();
        if (spends is null)
        {
            throw new();
        }
        foreach (var item in spends)
        {
            AllSpend allSpend = new AllSpend()
            {
                Id = item.Id,
                Date = item.Date,
                Price = item.Price,
                Reason = item.Reason,
                SpendType = item.SpendType!.Name
            };
            if (item.UserId == construction!.UserId)
                allSpend.AdminOrUser = "P";
            else if (item.FactoryId is not null)
                allSpend.AdminOrUser = "T";
            else
                allSpend.AdminOrUser = "A";

            allSpends.Add(allSpend);
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
        var spends = await _appDbContext.Spends.Where(x => x.ConstructionId == constructionId && x.IsConfirmed == false).OrderByDescending(x => x.Date).ToListAsync();
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
        var resultSpend = new List<GettingPersonalSpend>();

        List<Spend> adminSpends = _appDbContext.Spends.Where(x => x.IsCash).ToList();

        foreach (var item in adminSpends)
        {
            var adminSpend = new GettingPersonalSpend();
            adminSpend.Construction = item.Construction ?? new() { FullName = "Умумий чиқим" };
            adminSpend.Date = item.Date;
            adminSpend.Price = item.Price;
            adminSpend.Reason = item.Reason;
            adminSpend.SpendType = item.SpendType ?? new();
            resultSpend.Add(adminSpend);
        }

        List<In> ins = _appDbContext.Ins.Where(x => x.ConstructionId != null).ToList();

        foreach (var item in ins)
        {
            var inSpend = new GettingPersonalSpend();
            inSpend.Construction = item.Construction!;
            inSpend.Date = item.Date;
            inSpend.Price = item.Price;
            inSpend.Reason = item.Reason;
            inSpend.SpendType = new SpendType() { Name = "Прорапга кирим" };
            resultSpend.Add(inSpend);
        }

        List<In> insFactory = _appDbContext.Ins.Where(x => x.ConstructionId == null && x.IsCash && x.Factory != null).ToList();

        foreach (var item in insFactory)
        {
            var factoryIn = new GettingPersonalSpend();
            factoryIn.Construction = new Construction() { FullName = "Умумий чиқим" };
            factoryIn.Date = item.Date;
            factoryIn.Price = item.Price;
            factoryIn.Reason = item.Reason;
            factoryIn.SpendType = new SpendType() { Name = "Таминотчига кирим" };
            resultSpend.Add(factoryIn);
        }

        resultSpend = resultSpend.OrderByDescending(x => x.Date).ToList();
        return View(resultSpend);
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
        spend.IsCash = true;
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                spend.SpendType = (await _appDbContext.SpendTypes.FirstOrDefaultAsync(x => x.Id == spend.SpendTypeId))!;
                spend.Date = _dateTime.NowTime();
                spend.UserId = user!.Id;
                await _appDbContext.Spends.AddAsync(spend);
                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new();
                }
                user.Residual -= spend.Price;
                _appDbContext.Users.Update(user);
                result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new();
                }
                transaction.Commit();
                ViewData["result"] = result;
                ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
                return View();

            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
        return View();
    }


    [Authorize(Roles = "Admin")]
    public IActionResult AddAdminSpend(Guid constructionId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        var adminSpend = new AdminSpendDto()
        {
            ConstructionId = constructionId
        };
        var spendTypes = _appDbContext.SpendTypes.ToList();
        ViewData["SpendTypes"] = spendTypes;
        return View(adminSpend);
    }



    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddAdminSpend(AdminSpendDto adminSpend)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        if (!ModelState.IsValid)
        {
            ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
            return View(adminSpend);
        }
        Construction? construction = _appDbContext.Constructions.FirstOrDefault(x => x.Id == adminSpend.ConstructionId);
        if (construction == null)
        {
            ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
            return View(adminSpend);
        }
        string? userId = HttpContext.Session.GetString("AdminId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");
        Spend spend = _mapper.Map<Spend>(adminSpend);
        spend.IsConfirmed = true;
        spend.Date = _dateTime.NowTime();
        spend.User = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                _appDbContext.Spends.Add(spend);
                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new();
                }
                construction.Spend += adminSpend.Price ?? 0;
                construction.SpendDate = _dateTime.NowTime();
                _appDbContext.Constructions.Update(construction);
                result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new();
                }
                if (adminSpend.IsCash ?? false)
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
                var newAdminSpend = new AdminSpendDto()
                {
                    ConstructionId = construction.Id
                };
                ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
                ViewData["result"] = result;
                transaction.Commit();
                return View(newAdminSpend);
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        ViewData["result"] = 0;
        ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
        return View(adminSpend);
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSpendForAdmin(Guid id)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        var spend = await _appDbContext.Spends.FirstOrDefaultAsync(x => x.Id == id);
        if (spend!.UserId == spend.Construction!.UserId)
        {
            HttpContext.Session.SetInt32("result", -2);
            return RedirectToAction(actionName: "GetAllConfirmedForAdmin", new { constructionId = spend.ConstructionId });
        }
        else if (spend.FactoryId is not null)
        {
            HttpContext.Session.SetInt32("result", -2);
            return RedirectToAction(actionName: "GetAllConfirmedForAdmin", new { constructionId = spend.ConstructionId });

        }
            HttpContext.Session.SetInt32("result", 2);
            return RedirectToAction(actionName: "GetAllConfirmedForAdmin", new { constructionId = spend.ConstructionId });
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSpendForAdmin(Guid id)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        var spend = await _appDbContext.Spends.FirstOrDefaultAsync(x => x.Id == id);
        var construction = spend!.Construction;
        var user = spend.User;
        if (spend!.UserId == construction!.UserId)
        {

            HttpContext.Session.SetInt32("result", -1);
            return RedirectToAction(actionName: "GetAllConfirmedForAdmin", new { constructionId = spend.ConstructionId });
        }
        else if (spend.FactoryId is not null)
        {
                HttpContext.Session.SetInt32("result", -1);
            return RedirectToAction(actionName: "GetAllConfirmedForAdmin", new { constructionId = spend.ConstructionId });
        }
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                construction.Spend -= spend.Price;
                _appDbContext.Constructions.Update(construction);
                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                    throw new();
                if (spend.IsCash)
                {
                    user!.Residual += spend.Price;
                    _appDbContext.Users.Update(user);
                    result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                        throw new();
                }

                _appDbContext.Spends.Remove(spend);
                result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                    throw new();

                transaction.Commit();
                HttpContext.Session.SetInt32("result", 1);
                return RedirectToAction(actionName: "GetAllConfirmedForAdmin", new { constructionId = spend.ConstructionId });
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        return RedirectToAction(actionName: "GetAllConfirmedForAdmin", new { constructionId = spend.ConstructionId });
    }
}
