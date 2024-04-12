using Application.Common.Dtos.OutDto;

namespace WebUI.Controllers
{
    public class OutsController : Controller
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTime;

        public OutsController(IAppDbContext appDbContext, IMapper mapper, IDateTimeService dateTime)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllOut()
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

            var outs = _appDbContext.Outs.OrderByDescending(x => x.Date).ToList();
            return View(outs);
        }


        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddOut()
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");

            var construction = _appDbContext.Constructions.OrderByDescending(x => x.CreatedDate).ToList();
            ViewData["construction"] = construction;
            return View();
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddOut(AddOutDto outDto)
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["PhoneNumber"] = HttpContext.Session.GetString("PhoneNumber");
            var construction = _appDbContext.Constructions.OrderByDescending(x => x.CreatedDate).ToList();
            if (!ModelState.IsValid)
            {
                ViewData["construction"] = construction;
                return View(outDto);
            }
            var @out = _mapper.Map<Out>(outDto);
            @out.Date = _dateTime.NowTime();
            await _appDbContext.Outs.AddAsync(@out);
            var result = await _appDbContext.SaveChangesAsync();
            ViewData["result"] = result;
            ViewData["construction"] = construction;
            return View();
        }
    }
}
