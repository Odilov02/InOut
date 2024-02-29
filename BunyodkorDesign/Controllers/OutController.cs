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
    public IActionResult ConfirmOut(Guid constructionId)
    {
        return View();
    }


    [Authorize]
    public IActionResult GetAllConfirmedForAdmin(Guid constructionId)
    {
        return View();
    }


    public IActionResult GetAllNoConfirmedForAdmin(Guid constructionId)
    {
        return View();
    }
}
