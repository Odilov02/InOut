using Application.Common.Dtos.InDtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var userId = (HttpContext.Session.GetString("UserId"));
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true && x.User.Id.ToString() == userId).ToList();
        return View(ins);
    }

    [Authorize(Roles = "User")]
    public IActionResult GetAllNoConfirmed()
    {
        var userId = (HttpContext.Session.GetString("UserId"));
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false && x.User.Id.ToString() == userId).ToList();
        return View(ins);
    }

    [Authorize(Roles = "User")]
    public async Task<IActionResult> ConfirmationIn()
    {
        var userId = (HttpContext.Session.GetString("UserId"));
        List<In?> ins = (await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId))!.Ins!.Where(x => x.IsConfirmed == false)!.ToList();
        return View(ins);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> ConfirmationIn(List<ConfirmationIn?> insDto)
    {
        var userId = (HttpContext.Session.GetString("UserId"));
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
        if (result > 0)
            return RedirectToAction("Choose", "In");
        ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
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
        if (result > 0)
            return RedirectToAction("Choose", "Construction", new { constructionId = inDto.ConstructionId });
        else
        {
            return View(inDto);
        }
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false && x.User.Id == construction!.UserId).ToList();
        return View(ins);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllConfirmedForAdmin(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true && x.User.Id == construction!.UserId).ToList();
        return View(ins);
    }
}
