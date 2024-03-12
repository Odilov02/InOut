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

    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetResidual()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var construction = await _appDbContext.Constructions.FirstOrDefaultAsync(x => x.UserId.ToString() == userId);
        return View(construction);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllConstruction()
    {
        var constructions = await _appDbContext.Constructions.ToListAsync();
        return View(constructions);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddConstruction()
    {
        var usersAll = _userManager.Users.ToList();
        List<User> users = new List<User>();
        foreach (var user in usersAll)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
                users.Add(user);
        }
        ViewData["users"] = users;
        return View();
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddConstruction(AddConstructionDto constructionDto)
    {
        if (!ModelState.IsValid)
        {
            var usersAll = _userManager.Users.ToList();
            List<User> users = new List<User>();
            foreach (var item in usersAll)
            {
                var roles = await _userManager.GetRolesAsync(item);
                if (roles.Count == 0)
                    users.Add(item);
            }
            ViewData["users"] = users;
            return View(constructionDto);
        }
        var construction = _mapper.Map<Construction>(constructionDto);
        var user = _userManager.Users.FirstOrDefault(x => x.Id == construction.UserId);
        await _appDbContext.Constructions.AddAsync(construction);
        var result = await _appDbContext.SaveChangesAsync();
        if (result > 0)
        {
            await _userManager.AddToRoleAsync(user!, "User");
            return RedirectToAction("GetAllConstruction");
        }
        return View();
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Choose(Guid constructionId) => View(constructionId);
    public IActionResult GetAllDetails()
    {
        List<Construction> constructions = _appDbContext.Constructions.ToList();
        return View(constructions);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult AddAdminSpend(Guid constructionId)
    {
        var adminSpend = new AdminSpend()
        {
            ConstructionId = constructionId
        };
        return View(adminSpend);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddAdminSpend(AdminSpend adminSpend)
    {
        if (!ModelState.IsValid)
            return View(adminSpend);
        Construction? construction = _appDbContext.Constructions.FirstOrDefault(x => x.Id == adminSpend.ConstructionId);
        if (construction == null)
            return View(adminSpend);
        _appDbContext.AdminSpends.Add(adminSpend);
        var result = await _appDbContext.SaveChangesAsync();
        if (result > 0)
        {
            construction.Spend += adminSpend.Price;
            construction.SpendDate = DateTime.Now;
            _appDbContext.Constructions.Update(construction);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("GetAllConstruction", new { constructionId = adminSpend.ConstructionId } );
        }
        return View(adminSpend);
    }
}
