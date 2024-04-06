using Microsoft.AspNetCore.RateLimiting;

namespace WebUI.Controllers;

[EnableRateLimiting("fixed")]
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
        user.Password = user.Password.stringHash();
        user.UserName = Guid.NewGuid().ToString();
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
            return View("Login");
        else
            return View(userRegister);
    }




    public async Task<IActionResult> LogOut()
    {
        if (HttpContext.Session.GetString("UserId") is not null)
            HttpContext.Session.Remove("UserId");

        if (HttpContext.Session.GetString("AdminId") is not null)
            HttpContext.Session.Remove("AdminId");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }



    public IActionResult Login() => View();



    public IActionResult ErrorHandling() => View();



    [HttpPost]
    public async Task<IActionResult> Login(UserCridential userCridential)
    {
        if (!ModelState.IsValid)
            return View(userCridential);
        User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Password == userCridential.Password.stringHash() && x.Login == userCridential.Login);
        if (user == null) return View(userCridential);
        await _signInManager.SignInAsync(user, true);

        HttpContext.Session.SetString("FullName", user.FullName);
        HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber!);


        List<string> roles = (await _userManager.GetRolesAsync(user)).ToList();
        if (roles.Contains("Admin"))
        {
            HttpContext.Session.SetString("AdminId", user.Id.ToString());
            return RedirectToAction("GetAllConstruction", "Construction");
        }
        else if (roles.Contains("User"))
        {
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            return RedirectToAction("Choose", "Spend");
        }
        else
        {
            HttpContext.Session.Remove("UserId");
            await _signInManager.SignOutAsync();
            return View();
        }
    }



    [Authorize]
    public IActionResult UpdateLogin() => View();



    [Authorize]
    [HttpPost]
    public IActionResult UpdateLogin(UserCridential userCridential)
    {
        if (!ModelState.IsValid)
            return View(userCridential);
        var user = _appDbContext.Users.FirstOrDefault(x => x.Login == userCridential.Login && x.Password == userCridential.Password.stringHash());
        if (user is null)
            return View(userCridential);
        UserUpdate userUpdate = new()
        {
            Id = user.Id
        };
        return View("UpdateUser", userUpdate);
    }


    [Authorize(Roles = "Admin")]
    
    public async Task<IActionResult> UpdateUserForAdmin(Guid constructionId)
    {
        User? user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Construction!.Id == constructionId);
        if (user is null)
            return RedirectToAction("GetAllConstruction","User");
        UserUpdateAdmin userUpdate = new()
        {
            Id = user!.Id,
        };
        return View(userUpdate);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateUserForAdmin(UserUpdateAdmin userUpdate)
    {
        if (!ModelState.IsValid)
            return View(userUpdate);

        User? user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == userUpdate.Id);

        if (user is null)
            return RedirectToAction("GetAllConstruction", "User");

        user!.Login=userUpdate.Login;
        user.Password=userUpdate.Password.stringHash();
        _appDbContext.Users.Update(user);
        var result =await _appDbContext.SaveChangesAsync();
        ViewData["result"] = result;

        UserUpdateAdmin newUserUpdate = new()
        {
            Id = user!.Id
        };
        return View(newUserUpdate);

    }



    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateUser(UserUpdate userUpdate)
    {
        if (!ModelState.IsValid)
        {
            return View(userUpdate);
        }
        var user = _appDbContext.Users.FirstOrDefault(x => x.Id == userUpdate.Id);
        if (user == null)
            return View(userUpdate);
        user.Login = userUpdate.Login;
        user.Password = userUpdate.Password.stringHash();
        user.PhoneNumber = userUpdate.PhoneNumber;
        user.FullName = userUpdate.FullName;
        _appDbContext.Users.Update(user);
        var result = await _appDbContext.SaveChangesAsync();
        if (result > 0)
            return RedirectToAction("LogOut");
        else
        {
            UserUpdate newUserUpdate = new()
            {
                Id = user.Id
            };
            return View(newUserUpdate);
        }
    }
}