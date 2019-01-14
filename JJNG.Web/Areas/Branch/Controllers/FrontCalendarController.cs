using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using JJNG.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台")]
    public class FrontCalendarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

       public FrontCalendarController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<ActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View(_user);
        }
        
        public async Task<JsonResult> GetCalendarData()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var branch = _user.BelongTo;
            var frontdata = _context.BrhFrontDeskAccounts.Where(x => x.Branch == branch).ToList();
            var list_branch = _identityContext.UserBelongToDetial.Include(x => x.UserBelongTo).GroupBy(x => new
            {
                x.BelongToId,
                x.UserBelongTo.BelongToName,
                x.HouseNumber,
            }).Select(x => new
            {
                BranchId = x.Key.BelongToId,
                Branch = x.Key.BelongToName,
                HouseNumber = x.Key.HouseNumber,
            }).ToList();

            var houseNumbers = list_branch.Where(x => x.Branch == branch).Select(x => x.HouseNumber).ToList();

            
            var branchdata = houseNumbers;
            return Json(new { frontdata,branchdata});
        }
        public async Task<JsonResult> Create([FromBody]BrhFrontDeskAccounts _params)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var belongToId = _identityContext.UserBelongTo.SingleOrDefault(x => x.BelongToName == _user.BelongTo).BelongToId;
            _params.FrontDeskAccountsId = Convert.ToInt64(belongToId.ToString() + ConvertJson.DateTimeToStamp(DateTime.Now).ToString());
            _params.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            if (_params.Receivable == _params.Received)
                _params.IsFinish = true;
            else
                _params.IsFinish = false;
            _context.Add(_params);
            await _context.SaveChangesAsync();
            var frontdata = _context.BrhFrontDeskAccounts.Where(x => x.Branch == _params.Branch).ToList();
            var branchdata = _identityContext.UserBelongToDetial.Where(x => x.BelongToId == belongToId).ToList();
            return Json(new { frontdata});
        }
    }
}
