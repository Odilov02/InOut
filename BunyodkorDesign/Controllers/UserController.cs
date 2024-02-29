﻿using Application.Common.DTOs.UserDTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers;

public class UserController : Controller
{
    private readonly IAppDbContext _appDbContext;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _Mapper;
    private readonly SignInManager<User> _signInManager;

    public UserController(IAppDbContext appDbContext, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _Mapper = mapper;
        _signInManager = signInManager;
    }

    public IActionResult Register() => View();
    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterDto userRegister)
    {
        if (!ModelState.IsValid)
            return View(userRegister);
        User user = _Mapper.Map<User>(userRegister);
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
            return View("Login");
        else
            return View(userRegister);
    }


    //public IActionResult Login() => View();
    //[HttpPost]
    //public async Task<IActionResult> Login(UserCridential userCridential)
    //{
    //    if (!ModelState.IsValid)
    //        return View(userCridential);
    //    var users = _userManager.Users.ToList();
    //    User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Password == userCridential.Password && x.UserName == userCridential.UserName);
    //    if (user == null) return View();
    //    await _signInManager.SignInAsync(user, true);
    //    if (HttpContext.Session.GetString("UserId") is not null)
    //        HttpContext.Session.Remove("UserId");
    //    HttpContext.Session.SetString("UserId", user.Id.ToString());
    //    ViewData["FullName"] = user.FullName;
    //    ViewData["PhoneNumber"] = user.PhoneNumber;
    //    return RedirectToAction("Choose", "Out");
    //}

    public async Task<IActionResult> LogOut()
    {
        HttpContext.Session.Remove("UserId");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    public IActionResult Login() => View();
    [HttpPost]
    public async Task<IActionResult> Login(UserCridential userCridential)
    {
        if (!ModelState.IsValid)
            return View(userCridential);
        var users = _userManager.Users.ToList();
        User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Password == userCridential.Password && x.UserName == userCridential.UserName);
        if (user == null) return View(userCridential);
        await _signInManager.SignInAsync(user, true);
        if (HttpContext.Session.GetString("UserId") is not null)
            HttpContext.Session.Remove("UserId");
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        ViewData["FullName"] = user.FullName;
        ViewData["PhoneNumber"] = user.PhoneNumber;

        return RedirectToAction("GetAllConstruction", "Construction");
    }
}