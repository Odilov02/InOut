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


    [Authorize]
    public IActionResult Choose() => View();


    [Authorize]
    public IActionResult GetAllConfirmed()
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true).ToList();
        return View(ins);
    }


    [Authorize]
    public IActionResult GetAllNoConfirmed()
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(ins);
    }






    [Authorize]
    public IActionResult ConfirmationIn()
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(ins);
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ConfirmationIn(List<ConfirmationIn?> insDto)
    {

        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false && insDto.Any(y => y.Id == x.Id && y.IsConfirm == true)).ToList();
        foreach (var item in ins)
        {
            item.IsConfirmed = true;
        }
        _appDbContext.Ins.UpdateRange(ins);
        var result = await _appDbContext.SaveChangesAsync();
        if (result>0)
            return RedirectToAction("Choose", "In");
        ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(ins);
    }

    ///Admin Action
    [Authorize]
    public IActionResult AddIn(Guid constructionId)
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddIn(AddInDto inDto)
    {
        if (!ModelState.IsValid)
        {
            return View(inDto);
        }
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == HttpContext.Session.GetString("UserId"));
        if (user is null)
        {
            return View(inDto);
        }
        In @in = _mapper.Map<In>(inDto);
        @in.User = user;
        @in.Date = DateTime.Now;
        await _appDbContext.Ins.AddAsync(@in);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 1)
            return View("Choose");
        else
        {
            return View(inDto);
        }
    }

    [Authorize]
    public IActionResult GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View();
    }

    [Authorize]
    public IActionResult GetAllConfirmedForAdmin(Guid constructionId)
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View();
    }
}
