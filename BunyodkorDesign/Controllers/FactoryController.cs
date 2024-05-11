using Application.Common.Dtos.FactoryDtos;
using Domain.Entities;

namespace WebUI.Controllers;

public class FactoryController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
   private readonly IDateTimeService _dateTime;

    public FactoryController(IAppDbContext appDbContext, IMapper mapper, IDateTimeService dateTime)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    [Authorize(Roles = "Admin")]
    public IActionResult GetAllFactory()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        var factory = _appDbContext.Factories.OrderByDescending(x => x.CreatedDate).ToList();
        return View(factory);
    }




    [Authorize(Roles = "Admin")]
    public IActionResult GetAllDetails()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        var factory = _appDbContext.Factories.OrderByDescending(x => x.CreatedDate).ToList();
        return View(factory);
    }




    [Authorize(Roles = "Admin")]
    public IActionResult AddFactory()
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        return View();
    }



    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddFactory(AddFactoryDto factoryDto)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        if (!ModelState.IsValid)
            return View(factoryDto);
        Factory factory = _mapper.Map<Factory>(factoryDto);
        factory.SpendDate = _dateTime.NowTime();
        factory.InDate = _dateTime.NowTime();
        factory.CreatedDate = _dateTime.NowTime();
        await _appDbContext.Factories.AddAsync(factory);
        var result = await _appDbContext.SaveChangesAsync();
        if (result <= 0)
        {
            ViewData["result"] = 0;
            return View();
        }
        return RedirectToAction(actionName: "GetAllFactory");
    }





    [Authorize(Roles = "Admin")]
    public IActionResult GetAllFactorySpend(Guid factoryId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        var spends = _appDbContext.Spends.Where(x => x.FactoryId == factoryId).OrderByDescending(x=>x.Date).ToList();
        ViewData["factoryId"] = factoryId;
        return View(spends);
    }




    [Authorize(Roles = "Admin")]
    public IActionResult GetAllFactoryIn(Guid factoryId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        var ins = _appDbContext.Ins.Where(x => x.FactoryId == factoryId).OrderByDescending(x=>x.Date).ToList();
        ViewData["factoryId"] = factoryId;
        return View(ins);
    }




    [Authorize(Roles = "Admin")]
    public IActionResult AddFactoryIn(Guid factoryId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

        var inDto = new AddFactoryInDto()
        {
            FactoryId = factoryId
        };
        return View(inDto);
    }




    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddFactoryIn(AddFactoryInDto inDto)
    {
        try
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
            if (!ModelState.IsValid)
                return View(inDto);
            In @in = _mapper.Map<In>(inDto);
            @in.Date = _dateTime.NowTime();
            var factory = await _appDbContext.Factories.FirstOrDefaultAsync(x => x.Id == inDto.FactoryId);
            string? userId = HttpContext.Session.GetString("AdminId");

            if (userId is null)
                return RedirectToAction(actionName: "LogOut", controllerName: "User");

            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

            using (var transaction = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    await _appDbContext.Ins.AddAsync(@in);
                    var result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                        throw new();
                    if(inDto.IsCash??false)
                    {
                        user!.Residual -= @in.Price;
                        _appDbContext.Users.Update(user);
                        result = await _appDbContext.SaveChangesAsync();
                        if (result <= 0)
                        {
                            throw new();
                        }
                    }
                    factory!.In += @in.Price;
                    _appDbContext.Factories.Update(factory);
                    result = await _appDbContext.SaveChangesAsync();
                    if (result <= 0)
                        throw new();
                    transaction.Commit();
                    ViewData["result"] = result;
                    var newInDto = new AddFactoryInDto()
                    {
                        FactoryId = factory.Id
                    };
                    return View(newInDto);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            ViewData["result"] = 0;
        }
        catch (Exception e)
        {
            throw;
        }
        return View(inDto);
    }




    [Authorize(Roles = "Admin")]
    public IActionResult AddFactorySpend(Guid factoryId)
    {
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        ViewData["Constructions"] = _appDbContext.Constructions.ToList();
        ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
        var spendDto = new AddFactorySpendDto()
        {
            FactoryId = factoryId
        };
        return View(spendDto);
    }



    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddFactorySpend(AddFactorySpendDto spendDto)
    {
        ViewData["SpendTypes"] = _appDbContext.SpendTypes.ToList();
        ViewData["FullName"] = HttpContext.Session.GetString("FullName");
        ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
        ViewData["Constructions"] = _appDbContext.Constructions.ToList();
        if (!ModelState.IsValid)
            return View(spendDto);
        var spend = _mapper.Map<Spend>(spendDto);
        spend.IsConfirmed = true;
        spend.Date = _dateTime.NowTime();
        spend.SpendType  = (await _appDbContext.SpendTypes.FirstOrDefaultAsync(x => x.Id == spend.SpendTypeId))!;
        var factory = await _appDbContext.Factories.FirstOrDefaultAsync(x => x.Id == spendDto.FactoryId);

        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                await _appDbContext.Spends.AddAsync(spend);
                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                    throw new();
                factory!.Spend += spend.Price;
                _appDbContext.Factories.Update(factory);
                result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                    throw new();
                spend.Construction!.Spend += spend.Price;
                _appDbContext.Constructions.Update(spend.Construction);
                result= await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                    throw new();
                transaction.Commit();
                ViewData["result"] = result;
                var newInDto = new AddFactorySpendDto()
                {
                    FactoryId = factory.Id
                };
                return View(newInDto);
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
        ViewData["result"] = 0;
        return View(spendDto);
    }

}