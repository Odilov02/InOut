namespace WebUI.Controllers;

public class InController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public InController(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    [Authorize(Roles = "User")]
    public IActionResult Choose() => View();

    [Authorize(Roles = "User")]
    public IActionResult GetAllConfirmed()
    {
        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true && x.User.Id.ToString() == userId).ToList();
        return View(ins);
    }

    [Authorize(Roles = "User")]
    public IActionResult GetAllNoConfirmed()
    {
        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false && x.User.Id.ToString() == userId).ToList();
        return View(ins);
    }

    [Authorize(Roles = "User")]
    public async Task<IActionResult> ConfirmationIn()
    {
        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        List<In?> ins = (await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId))!.Ins!.Where(x => x.IsConfirmed == false)!.ToList();
        return View(ins);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> ConfirmationIn(List<ConfirmationIn?> insDto)
    {
        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        if (!ModelState.IsValid)
        {
            List<In?> insdto = (await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId))!.Ins!.Where(x => x.IsConfirmed == false)!.ToList();
            return View(insdto);
        }
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false && insDto.Any(y => y.Id == x.Id && y.IsConfirmed == true)).ToList();
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
        foreach (var item in ins)
        {
            item.IsConfirmed = true;
            user!.Residual += item.Price;
            user.Construction!.In += item.Price;
            user.Construction.InDate = DateTime.Now;
        }

        _appDbContext.Ins.UpdateRange(ins);
        var result = await _appDbContext.SaveChangesAsync();
        ins = (await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId))!.Ins!.Where(x => x.IsConfirmed == false)!.ToList()!;
        ViewData["result"] = result-2;
        return View(ins);
    }

    ///Admin Action
    [Authorize(Roles = "Admin")]
    public IActionResult AddIn(Guid constructionId)
    {
        var inDto = new AddInDto()
        {
            ConstructionId = constructionId
        };
        return View(inDto);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddIn(AddInDto inDto)
    {
        if (!ModelState.IsValid)
        {
            return View(inDto);
        }
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Construction!.Id == inDto.ConstructionId);
        if (user is null)
        {
            return View(inDto);
        }
        In @in = _mapper.Map<In>(inDto);
        @in.User = user;
        @in.Date = DateTime.Now;
        await _appDbContext.Ins.AddAsync(@in);
        var result = await _appDbContext.SaveChangesAsync();
        var inResult = new AddInDto()
        {
            ConstructionId = inDto.ConstructionId
        };
        ViewData["result"] = result;
        return View(inResult);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        ViewData["constructionId"] = constructionId;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false && x.User.Id == construction!.UserId).ToList();
        return View(ins);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllConfirmedForAdmin(Guid constructionId)
    {
        ViewData["constructionId"] = constructionId;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true && x.User.Id == construction!.UserId).ToList();
        return View(ins);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllInPersonal()
    {
        string? userId = HttpContext.Session.GetString("AdminId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        ViewData["UserId"] = userId;
        List<In> ins = await _appDbContext.Ins.Where(x => x.User.Id.ToString() == userId).ToListAsync();
        return View(ins);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AddPersonalIn()
    {
        string? userId = HttpContext.Session.GetString("AdminId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        var inDto = new PersonalIn()
        {
            UserId = Guid.Parse(userId)
        };
        return View(inDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddPersonalIn(PersonalIn personalIn)
    {
        if (!ModelState.IsValid)
            return View(personalIn);
        var @in = _mapper.Map<In>(personalIn);
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == personalIn.UserId);
        if (user == null)
            return View(personalIn);
        await _appDbContext.Ins.AddAsync(@in);
        var result = await _appDbContext.SaveChangesAsync();
        ViewData["result"] = result;
        if (result > 0)
        {
            user.Residual += @in.Price;
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            var inDto = new PersonalIn()
            {
                UserId = user.Id
            };
            return View(inDto);
        }
        return View(personalIn);
    }
}
