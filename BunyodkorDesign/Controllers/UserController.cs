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
        user.Password = user.Password.stringHash();
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            ViewData["result"] = 1;
            return View("Login");
        }
        else
        {
            ViewData["result"] = 0;
            return View(userRegister);
        }
    }


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
        User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Password == userCridential.Password.stringHash() && x.UserName == userCridential.UserName);
        if (user == null) return View(userCridential);
        await _signInManager.SignInAsync(user, true);
        if (HttpContext.Session.GetString("UserId") is not null)
            HttpContext.Session.Remove("UserId");
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        List<string> roles = (await _userManager.GetRolesAsync(user)).ToList();
        if (roles.Contains("Admin"))
            return RedirectToAction("GetAllConstruction", "Construction");
        else if (roles.Contains("User"))
            return RedirectToAction("Choose", "Spend");
        else
        {
            HttpContext.Session.Remove("UserId");
            await _signInManager.SignOutAsync();
            ViewData["result"] = 0;
        return View();
        }
    }
    [Authorize]
    public IActionResult UpdateLogin()
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    public IActionResult UpdateLogin(UserCridential userCridential)
    {
        if (!ModelState.IsValid)
        {
            return View(userCridential);
        }
        var user = _appDbContext.Users.FirstOrDefault(x => x.UserName == userCridential.UserName && x.Password == userCridential.Password.stringHash());
        if (user == null)
            return View(userCridential);
        UserUpdate userUpdate = new()
        {
            Id = user.Id
        };
        return View("UpdateUser", userUpdate);
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
        user.UserName = userUpdate.UserName;
        user.Password = userUpdate.Password.stringHash();
        user.PhoneNumber = userUpdate.PhoneNumber;
        user.FullName = userUpdate.FullName;
        _appDbContext.Users.Update(user);
        var result = await _appDbContext.SaveChangesAsync();
        if (result > 0)
        {
            return RedirectToAction("LogOut");
        }
        else
        {

            UserUpdate newUserUpdate = new()
            {
                Id = user.Id
            };
            ViewData["result"] = result;
            return View(newUserUpdate);
        }
    }


}