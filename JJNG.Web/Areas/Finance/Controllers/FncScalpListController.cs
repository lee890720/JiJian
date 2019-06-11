using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncScalpListController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;
        public FncScalpListController(AppDbContext context, AppIdentityDbContext identityContext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = usrMgr;
        }

        public async Task<ActionResult> Index(string branchName = "既见·南国", int branchId = 2, int count = 15)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["BranchId"] = branchId;
            var fncBranch = new FncBranch();
            fncBranch.BranchName = branchName;
            fncBranch.BranchId = branchId;
            fncBranch.Count = count;
            var list_branch = await _context.FncBranch.Where(x => x.BranchName != "运营中心" && x.BranchName != "町隐学院").ToListAsync();
            return View(Tuple.Create<FncBranch, List<FncBranch>>(fncBranch, list_branch));
        }

        public async Task<JsonResult> GetFrontList([FromBody]FDAParams fdaParams)
        {
            var frontList = await _context.BrhScalp.Where(x => x.Branch == fdaParams.BranchName  && DateTime.Compare(fdaParams.StartDate, x.StartDate) <= 0 && DateTime.Compare(x.StartDate, fdaParams.EndDate) < 0).ToListAsync();
            var pie1List = frontList.GroupBy(x => new { x.Channel }).Select(x => new
            {
                Channel = x.Key.Channel,
                CTotal = x.Count(),
                ATotal = x.Sum(s=>s.TotalPrice)
            }).ToList();

            return Json(new { frontList, pie1List});
        }

        public async Task<JsonResult> UpdateList([FromBody]FDAParams fdaParams)
        {
            if (fdaParams.Ids.Count > 0)
            {
                _context.BrhFrontDeskAccounts.Where(x => fdaParams.Ids.Contains(x.FrontDeskAccountsId) && !x.IsFinance).ToList().ForEach(x =>
                {
                    x.IsFinance = true;
                    _context.Update(x);
                });
                await _context.SaveChangesAsync();
            }

            var frontList = await _context.BrhScalp.Where(x => x.Branch == fdaParams.BranchName && DateTime.Compare(fdaParams.StartDate, x.StartDate) <= 0 && DateTime.Compare(x.StartDate, fdaParams.EndDate) < 0).ToListAsync();
            var pie1List = frontList.GroupBy(x => new { x.Channel }).Select(x => new
            {
                Channel = x.Key.Channel,
                CTotal = x.Count(),
                ATotal = x.Sum(s => s.TotalPrice)
            }).ToList();

            return Json(new { frontList, pie1List });
        }
    }
}