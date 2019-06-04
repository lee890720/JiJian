using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncExpendRecordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncExpendRecordController(AppDbContext context, AppIdentityDbContext identityContext, UserManager<AppIdentityUser> usrMgr)
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

        public async Task<JsonResult> GetExpendList([FromBody]FDAParams fdaParams)
        {
            var expendList = await _context.BrhExpendRecord.Where(x => x.Branch == fdaParams.BranchName  && DateTime.Compare(fdaParams.StartDate, x.EnteringDate) <= 0 && DateTime.Compare(x.EnteringDate, fdaParams.EndDate) < 0).ToListAsync();
            var pie1List = expendList.GroupBy(x => new { x.ExpendType }).Select(x => new
            {
                ExpendType = x.Key.ExpendType,
                Total = x.Sum(s => s.Amount),
            }).ToList();

            var days = (fdaParams.EndDate - fdaParams.StartDate).Days;
            List<FncMonthData> dailyList = new List<FncMonthData>();
            for (var i = 0; i < days; i++)
            {
                var daily = new FncMonthData();
                daily.Month = fdaParams.StartDate.AddDays(i).Date;
                daily.HouseAmount = expendList.Where(x => DateTime.Compare(x.EnteringDate.Date, daily.Month) == 0).Select(x => x.Amount).Sum();
                dailyList.Add(daily);
            }
            return Json(new { expendList, pie1List, dailyList });
        }

        public async Task<JsonResult> UpdateList([FromBody]FDAParams fdaParams)
        {
            if (fdaParams.Ids.Count > 0)
            {
                _context.BrhExpendRecord.Where(x => fdaParams.Ids.Contains(x.ExpendRecordId) && !x.IsFinance).ToList().ForEach(x =>
                {
                    x.IsFinance = true;
                    _context.Update(x);
                });
                await _context.SaveChangesAsync();
            }

            var expendList = await _context.BrhExpendRecord.Where(x => x.Branch == fdaParams.BranchName && DateTime.Compare(fdaParams.StartDate, x.EnteringDate) <= 0 && DateTime.Compare(x.EnteringDate, fdaParams.EndDate) < 0).ToListAsync();
            var pie1List = expendList.GroupBy(x => new { x.ExpendType }).Select(x => new
            {
                ExpendType = x.Key.ExpendType,
                Total = x.Sum(s => s.Amount),
            }).ToList();

            var days = (fdaParams.EndDate - fdaParams.StartDate).Days;
            List<FncMonthData> dailyList = new List<FncMonthData>();
            for (var i = 0; i < days; i++)
            {
                var daily = new FncMonthData();
                daily.Month = fdaParams.StartDate.AddDays(i).Date;
                daily.HouseAmount = expendList.Where(x => DateTime.Compare(x.EnteringDate.Date, daily.Month) == 0).Select(x => x.Amount).Sum();
                dailyList.Add(daily);
            }
            return Json(new { expendList, pie1List, dailyList });
        }
    }
}
