namespace WebUI.Controllers;

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
        var spendTypes = _appDbContext.SpendTypes.ToList();
        ViewData["SpendTypes"] = spendTypes;
        return View();
    }
    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> AddSpend(AddSpendDto SpendDto)
    {
        if (!ModelState.IsValid)
        {
            ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
            return View(SpendDto);
        }
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == HttpContext.Session.GetString("UserId"));

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
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == HttpContext.Session.GetString("UserId"));
        ViewData["FullName"] = user!.FullName;
        ViewData["PhoneNumber"] = user.PhoneNumber!.Substring(1);
        return View();
    }
    [Authorize(Roles = "User")]
    public IActionResult GetAllConfirmed()
    {
        var userId = (HttpContext.Session.GetString("UserId"));
        List<Spend> spends = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == true && x.UserId.ToString() == userId).ToList();
        return View(spends);
    }

    [Authorize(Roles = "User")]
    public IActionResult GetAllNoConfirmed()
    {
        var userId = (HttpContext.Session.GetString("UserId"));
        List<Spend> spends = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && x.UserId.ToString() == userId).ToList();
        return View(spends);
    }

    //<<=====        Admin Action        ========>>

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ConfirmSpend(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<Spend> spends = _appDbContext.Spends.Where(x => x.User.Id == construction!.User.Id && x.IsConfirmed == false).ToList();
        if (HttpContext.Session.GetString("ConstructionId") is not null)
            HttpContext.Session.Remove("ConstructionId");
        HttpContext.Session.SetString("ConstructionId", constructionId.ToString());
        ViewData["constructionId"] = constructionId;
        return View(spends);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> ConfirmSpend(List<ConfirmationSpend> outDtos)
    {
        string constructionId = HttpContext.Session.GetString("ConstructionId")!;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id.ToString() == constructionId);
        if (!ModelState.IsValid)
        {
            ViewData["constructionId"] = constructionId;
            List<Spend> spends = _appDbContext.Spends.Where(x => x.User.Id == construction!.User.Id && x.IsConfirmed == false).ToList();
            return View(spends);
        }
        List<Spend> entities = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && outDtos.Any(y => y.Id == x.Id && y.IsConfirmed == true)).ToList();
        foreach (var item in entities)
        {
            item!.IsConfirmed = true;
            construction!.User!.Residual -= item.Price;
            construction.Spend += item.Price;
            construction.SpendDate = DateTime.Now;
        }
        _appDbContext.Spends.UpdateRange(entities!);
        var result = await _appDbContext.SaveChangesAsync();
        entities = _appDbContext.Spends.ToList().Where(x => x.IsConfirmed == false && x.User.Id == construction!.User.Id).ToList()!;
        ViewData["constructionId"] = constructionId;
        ViewData["result"] = result-2;
        return View(entities);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllConfirmedForAdmin(Guid constructionId)
    {
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
        ViewData["constructionId"] = constructionId;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        var spends = await _appDbContext.Spends.Where(x => x.User.Id == construction!.UserId && x.IsConfirmed == false).ToListAsync();
        return View(spends);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllPersonalSpend()
    {
        var userId = (HttpContext.Session.GetString("UserId"));
        ViewData["Residual"]=(await _appDbContext.Users.FirstOrDefaultAsync(x=>x.Id.ToString() == userId))!.Residual;
        List<Spend> spends =await _appDbContext.Spends.Where(x => x.UserId.ToString() != userId).ToListAsync();
        return View(spends);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult AddSpendPersonal()
    {
        var spendTypes = _appDbContext.SpendTypes.ToList();
        ViewData["SpendTypes"] = spendTypes;
        return View();
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddSpendPersonal(AddSpendDto SpendDto)
    {
        if (!ModelState.IsValid)
        {
            ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
            return View(SpendDto);
        }
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == HttpContext.Session.GetString("UserId"));

        Spend spend = _mapper.Map<Spend>(SpendDto);
        spend.SpendType = (await _appDbContext.SpendTypes.FirstOrDefaultAsync(x => x.Id == spend.SpendType.Id))!;
        spend.Date = DateTime.Now;
        spend.UserId = user!.Id;
        await _appDbContext.Spends.AddAsync(spend);
        var result = await _appDbContext.SaveChangesAsync();
        if(result>0)
        {
            user.Residual -= spend.Price;
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
        }
        ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
        ViewData["result"] = result;
        return View();
    }
}
