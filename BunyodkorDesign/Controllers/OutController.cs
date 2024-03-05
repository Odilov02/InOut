﻿using Application.Common.Dtos.OutDtos;
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

    [Authorize(Roles = "User")]
    public IActionResult AddOut()
    {
        var outTypes= _appDbContext.OutTypes.ToList();
        ViewData["OutTypes"] = outTypes;
        return View();
    }
    [Authorize(Roles = "User")]
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
        if (result > 0)
            return View("Choose");
        else
        {
            ViewData["OutTypes"] = _appDbContext.OutTypes.ToList();
            return View(OutDto);
        }
    }
    [Authorize(Roles = "User")]
    public IActionResult Choose() => View();

    [Authorize(Roles = "User")]
    public IActionResult GetAllConfirmed()
    {
        List<Out> outs = _appDbContext.Outs.ToList().Where(x => x.IsConfirmed == true).ToList();
        return View(outs);
    }


    [Authorize(Roles = "User")]
    public IActionResult GetAllNoConfirmed()
    {
        List<Out> outs = _appDbContext.Outs.ToList().Where(x => x.IsConfirmed == false).ToList();
        return View(outs);
    }


    ///<<=====        Admin Action        ========>>

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ConfirmOut(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        List<Out> outs = _appDbContext.Outs.Where(x => x.User.Id == construction!.User.Id && x.IsConfirmed == false).ToList();
        if (HttpContext.Session.GetString("ConstructionId") is not null)
            HttpContext.Session.Remove("ConstructionId");
        HttpContext.Session.SetString("ConstructionId", constructionId.ToString());
        return View(outs);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> ConfirmOut(List<ConfirmationOut> outDtos)
    {
        string constructionId = HttpContext.Session.GetString("ConstructionId")!;
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id.ToString() == constructionId);
        if (!ModelState.IsValid)
        {
            List<Out> outs = _appDbContext.Outs.Where(x => x.User.Id == construction!.User.Id && x.IsConfirmed == false).ToList();
            return View(outs);
        }
        List<Out> entities = _appDbContext.Outs.ToList().Where(x => x.IsConfirmed == false && outDtos.Any(y => y.Id == x.Id && y.IsConfirmed == true)).ToList();
        foreach (var item in entities)
        {
            item!.IsConfirmed = true;
            construction!.User!.Residual -= item.Price;
            construction.Out -= item.Price;
            construction.OutDate = DateTime.Now;
        }
        _appDbContext.Outs.UpdateRange(entities!);
        var result = await _appDbContext.SaveChangesAsync();
        if (result > 0)
        {
            return RedirectToAction("Choose", "Construction", new { constructionId = constructionId });
        }
        entities = _appDbContext.Outs.ToList().Where(x => x.IsConfirmed == false && x.User.Id == construction!.User.Id).ToList()!;
        HttpContext.Session.Remove("ConstructionId");
        return View(entities);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllConfirmedForAdmin(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        var outs = await _appDbContext.Outs.Where(x => x.User.Id == construction!.UserId && x.IsConfirmed == true).ToListAsync();
        return View(outs);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.Id == constructionId);
        var outs = await _appDbContext.Outs.Where(x => x.User.Id == construction!.UserId && x.IsConfirmed == false).ToListAsync();
        return View(outs);
    }
}
