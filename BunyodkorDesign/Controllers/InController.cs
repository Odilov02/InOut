using Microsoft.AspNetCore.RateLimiting;

namespace WebUI.Controllers;
[EnableRateLimiting("fixed")]
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
    public IActionResult Choose()
    {

        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
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

        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true && x.User.Id.ToString() == userId).ToList();
        return View(ins);
    }




    [Authorize(Roles = "User")]
    public IActionResult GetAllNoConfirmed()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        string? userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false && x.User.Id.ToString() == userId).ToList();
        return View(ins);
    }




    [Authorize(Roles = "User")]
    public async Task<IActionResult> ConfirmationIn()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


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
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

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

        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                foreach (var item in ins)
                {
                    item.IsConfirmed = true;
                    _appDbContext.Ins.Update(item);
                    var result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                    {
                        throw new();
                    }
                    user!.Residual += item.Price;
                    user!.Construction!.In += item.Price;
                    user.Construction.InDate = DateTime.Now;
                    _appDbContext.Constructions.Update(user!.Construction!);
                    result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                    {
                        throw new();
                    }
                }
                transaction.Commit();
                ViewData["result"] = ins.Count;
                ins = (await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId))!.Ins!.Where(x => x.IsConfirmed == false)!.ToList()!;
                return View(ins);
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            List<In?> newInsDto = (await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId))!.Ins!.Where(x => x.IsConfirmed == false)!.ToList();
            return View(newInsDto);
        }
    }




    ///Admin Action
    [Authorize(Roles = "Admin")]
    public IActionResult AddIn(Guid constructionId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


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
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


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

        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                @in.User = user;
                @in.Date = DateTime.Now;
                await _appDbContext.Ins.AddAsync(@in);
                var result = await _appDbContext.SaveChangesAsync();
                if(result<=0)
                {
                    throw new();
                }
                var adminId= HttpContext.Session.GetString("AdminId");
                if (adminId is null)
                    return RedirectToAction(actionName: "LogOut", controllerName: "User");
                var admin =await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == adminId);


                admin!.Residual -= @in.Price;
                _appDbContext.Users.Update(admin);
                result =await _appDbContext.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new();
                }
                var inResult = new AddInDto()
                {
                    ConstructionId = inDto.ConstructionId
                };
                transaction.Commit();
                ViewData["result"] = result;
                return View(inResult);
            }
            catch(Exception)
            {
                transaction.Rollback();
            }
        }
        return View(inDto);
    }




    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        ViewData["constructionId"] = constructionId;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false && x.User.Id == construction!.UserId).ToList();
        return View(ins);
    }




    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllConfirmedForAdmin(Guid constructionId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        ViewData["constructionId"] = constructionId;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true && x.User.Id == construction!.UserId).ToList();
        return View(ins);
    }




    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllInPersonal()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


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
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


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
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        if (!ModelState.IsValid)
            return View(personalIn);
        var @in = _mapper.Map<In>(personalIn);
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == personalIn.UserId);
        if (user == null)
            return View(personalIn);
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
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
                    transaction.Commit();
                    return View(inDto);
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
        return View(personalIn);
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateIn(Guid id)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        var @in =await _appDbContext.Ins.FirstOrDefaultAsync(x => x.Id == id);
        UpdateInDto inDto = _mapper.Map<UpdateInDto>(@in);
        ViewData["constructionId"] = @in!.User.Construction!.Id;
        return View(inDto);
    }



    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateIn(UpdateInDto inDto)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");


        if (!ModelState.IsValid)    
        {
            return View(inDto);
        }
        var @in =await _appDbContext.Ins.FirstOrDefaultAsync(x => x.Id == inDto.Id);
        if(@in == null)
        {
            throw new();
        }

        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                long oldPrice = @in.Price;
                @in.Price = inDto.Price??0;
                @in.Reason = inDto.Reason;
                 _appDbContext.Ins.Update(@in);
                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new();
                }
                var adminId = HttpContext.Session.GetString("AdminId");
                if (adminId is null)
                    return RedirectToAction(actionName: "LogOut", controllerName: "User");
                var admin = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == adminId);


                admin!.Residual -= @in.Price-oldPrice;
                _appDbContext.Users.Update(admin);
                result = await _appDbContext.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new();
                }
                transaction.Commit();
                ViewData["result"] = result;
                ViewData["constructionId"] = @in!.User.Construction!.Id;
                UpdateInDto newInDto = _mapper.Map<UpdateInDto>(@in);
                return View(newInDto);
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        return View(inDto);
    }



    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteIn(Guid id)
    {
        var @in = await _appDbContext.Ins.FirstOrDefaultAsync(x => x.Id == id);
        if (@in == null)
        {
            throw new();
        }
        Guid? constructionId = @in.User.Construction!.Id;
        if (constructionId is null)
        {
            throw new();
        }
        var adminId = HttpContext.Session.GetString("AdminId");
        if (adminId is null)
            return RedirectToAction(actionName: "LogOut", controllerName: "User");

        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == adminId);

        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                user!.Residual += @in.Price;
                _appDbContext.Users.Update(user);
                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new();
                }
                _appDbContext.Ins.Remove(@in);
                result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new();
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        return RedirectToAction(actionName: "GetAllNoConfirmedForAdmin", new { constructionId = constructionId });
    }
}
