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
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true).ToList();
        return View(ins);
    }


    [Authorize(Roles = "User")]
    public IActionResult GetAllNoConfirmed()
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(ins);
    }







    [Authorize(Roles = "User")]
    public IActionResult ConfirmationIn()
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(ins);
    }

    [Authorize(Roles = "User")]
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
    public IActionResult GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(ins);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult GetAllConfirmedForAdmin(Guid constructionId)
    {
        List<In> ins = _appDbContext.Ins.ToList().Where(x => x.IsConfirmed == true).ToList();
        return View(ins);
    }
}
