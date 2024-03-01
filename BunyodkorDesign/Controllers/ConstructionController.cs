using Application.Common.Dtos.ConstructionDtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WebUI.Controllers;

public class ConstructionController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    public ConstructionController(IAppDbContext appDbContext, UserManager<User> userManager, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _mapper = mapper;
    }

    [Authorize]
    public async Task<IActionResult> GetResidual()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.UserId.ToString() == userId);
        return View(construction);
    }

    [Authorize]
    public async Task<IActionResult> GetAllConstruction()
    {
        var constructions = await _appDbContext.Constructions.ToListAsync();
        return View(constructions);
    }
    [Authorize]
    public async Task<IActionResult> AddConstruction()
    {

        ViewData["users"] = await _userManager.Users.Where(x => x.Construction == null).ToListAsync();
        return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddConstruction(AddConstructionDto constructionDto)
    {
        if (!ModelState.IsValid)
        {
            ViewData["users"] = await _userManager.Users.Where(x => x.Construction == null).ToListAsync();
            return View(constructionDto);
        }
        var construction=_mapper.Map<Construction>(constructionDto);  
       // construction.User= await _userManager.Users.FirstOrDefaultAsync(x => x.Id == constructionDto.userId);
      await  _appDbContext.Constructions.AddAsync(construction);
        var result =await _appDbContext.SaveChangesAsync();
        if(result>0)
        {
            return RedirectToAction("GetAllConstruction");
        }
        return View();
    }
    public  IActionResult Choose(Guid constructionId)=> View(constructionId);
}
