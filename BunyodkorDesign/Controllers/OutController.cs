using Application.Common.Dtos.OutDtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers;

public class OutController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public OutController(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    [Authorize]
    public IActionResult AddOut()
    {
        ViewData["OutTypes"] = _appDbContext.OutTypes.ToList();
        return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddOut(AddOutDto OutDto)
    {
        if (!ModelState.IsValid)
        {
            ViewData["OutTypes"] = _appDbContext.OutTypes.ToList();
            return View(OutDto);
        }
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == HttpContext.Session.GetString("UserId"));
        if (user is null)
        {
            ViewData["OutTypes"] = _appDbContext.OutTypes.ToList();
            return View(OutDto);
        }
        Out @out = _mapper.Map<Out>(OutDto);
        @out.User = user;
        @out.OutType = (await _appDbContext.OutTypes.FirstOrDefaultAsync(x => x.Id == @out.OutType.Id))!;
        @out.Date = DateTime.Now;
        await _appDbContext.Outs.AddAsync(@out);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 1)
            return View("Choose");
        else
        {
            ViewData["OutTypes"] = _appDbContext.OutTypes.ToList();
            return View(OutDto);
        }

    }



    [Authorize]
    public IActionResult Choose() => View();



    [Authorize]
    public IActionResult GetAllConfirmed()
    {
        List<Out> outs = _appDbContext.Outs.ToList().Where(x => x.IsConfirmed == true).ToList();
        return View(outs);
    }


    [Authorize]
    public IActionResult GetAllNoConfirmed()
    {
        List<Out> outs = _appDbContext.Outs.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(outs);
    }


    ///<<=====        Admin Action        ========>>
    [Authorize]
    public async Task<IActionResult> ConfirmOut(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        var outs = _appDbContext.Outs.Where(x => x.User.Id == construction!.User.Id && x.IsConfirmed == false).ToList();
        return View(outs);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ConfirmOut(List<ConfirmationOut?> outsDto)
    {
        List<Out> outs = _appDbContext.Outs.ToList().Where(x => x.IsConfirmed == false && outsDto.Any(y => y.Id == x.Id && y.IsConfirm == true)).ToList();
        foreach (var item in outs) item.IsConfirmed = true;
        _appDbContext.Outs.UpdateRange(outs);
        var result = await _appDbContext.SaveChangesAsync();
        if (result > 0)
        {
            return RedirectToAction("Choose", "Construction", new { constructionId = outs[0].User.Construction!.Id });
        }
        outs = _appDbContext.Outs.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(outs);
    }
    [Authorize]
    public async Task<IActionResult> GetAllConfirmedForAdmin(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        var outs =await _appDbContext.Outs.Where(x => x.User.Id == construction!.UserId && x.IsConfirmed == true).ToListAsync();
        return View(outs);
    }

    [Authorize]
    public async Task<IActionResult> GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        var outs =await _appDbContext.Outs.Where(x => x.User.Id == construction!.UserId && x.IsConfirmed == true).ToListAsync();
        return View(outs);
    }
}
