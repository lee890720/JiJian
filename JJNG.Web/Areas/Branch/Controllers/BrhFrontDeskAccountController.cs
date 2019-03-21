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
namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台,前台审核")]
    public class BrhFrontDeskAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhFrontDeskAccountController(AppDbContext context, AppIdentityDbContext identityContext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["BranchId"] = _user.BranchId;
            var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchName == _user.Branch);
            return View(fncBranch);
        }

        public async Task<JsonResult> GetFrontList([FromBody]FDAParams fdaParams)
        {
            var payList = new List<BrhFrontPaymentDetial>();
            var frontList = await _context.BrhFrontDeskAccounts.Include(x => x.BrhFrontPaymentDetial).Where(x => x.Branch == fdaParams.BranchName && x.State != StateType.已删除 && DateTime.Compare(fdaParams.StartDate, x.StartDate) <= 0 && DateTime.Compare(x.StartDate, fdaParams.EndDate) < 0).ToListAsync();
            foreach (var fr in frontList)
            {
                if (fr.BrhFrontPaymentDetial.Count > 0)
                    payList.AddRange(fr.BrhFrontPaymentDetial);
            }
            var pie1List = frontList.GroupBy(x => new { x.Channel }).Select(x => new
            {
                Channel = x.Key.Channel,
                CTotal = x.Sum(s => s.Count),
                ATotal = x.Sum(s => s.BrhFrontPaymentDetial.Select(b => b.PayAmount).Sum())
            }).ToList();
            var pie2List = payList.GroupBy(x => new { x.PayWay }).Select(x => new
            {
                PayWay = x.Key.PayWay,
                Total = x.Sum(s => s.PayAmount),
            }).ToList();

            var days = (fdaParams.EndDate - fdaParams.StartDate).Days;
            List<FncMonthData> dailyList = new List<FncMonthData>();
            for (var i = 0; i < days; i++)
            {
                var daily = new FncMonthData();
                daily.Month = fdaParams.StartDate.AddDays(i).Date;
                daily.HouseTotal = fdaParams.Count;
                daily.HouseAmount = frontList.Where(x => DateTime.Compare(x.StartDate.Date, daily.Month) <= 0 && DateTime.Compare(daily.Month, x.EndDate.Date) < 0).Select(x => x.UnitPrice).Sum();
                daily.HouseCount = frontList.Where(x => DateTime.Compare(x.StartDate.Date, daily.Month) <= 0 && DateTime.Compare(daily.Month, x.EndDate.Date) < 0).Select(x => x.Count).Count();
                if (daily.HouseTotal != 0)
                {
                    daily.Rate = (double)daily.HouseCount / (double)daily.HouseTotal;
                    daily.ValidAverage = daily.HouseAmount / daily.HouseTotal;
                }
                else
                {
                    daily.Rate = 0;
                    daily.ValidAverage = 0;
                }
                if (daily.HouseCount != 0)
                    daily.Average = daily.HouseAmount / daily.HouseCount;
                else
                    daily.Average = 0;
                dailyList.Add(daily);
            }
            return Json(new { frontList, pie1List, pie2List, dailyList });
        }

        public async Task<JsonResult> UpdateList([FromBody]FDAParams fdaParams)
        {
            if (fdaParams.Ids.Count > 0)
            {
                _context.BrhFrontDeskAccounts.Where(x => fdaParams.Ids.Contains(x.FrontDeskAccountsId) && !x.IsFinance).ToList().ForEach(x =>
                {
                    x.IsFront = true;
                    _context.Update(x);
                });
                await _context.SaveChangesAsync();
            }

            var payList = new List<BrhFrontPaymentDetial>();
            var frontList = await _context.BrhFrontDeskAccounts.Include(x => x.BrhFrontPaymentDetial).Where(x => x.Branch == fdaParams.BranchName && x.State != StateType.已删除 && DateTime.Compare(fdaParams.StartDate, x.StartDate) <= 0 && DateTime.Compare(x.StartDate, fdaParams.EndDate) < 0).ToListAsync();
            foreach (var fr in frontList)
            {
                if (fr.BrhFrontPaymentDetial.Count > 0)
                    payList.AddRange(fr.BrhFrontPaymentDetial);
            }
            var pie1List = frontList.GroupBy(x => new { x.Channel }).Select(x => new
            {
                Channel = x.Key.Channel,
                CTotal = x.Sum(s => s.Count),
                ATotal = x.Sum(s => s.BrhFrontPaymentDetial.Select(b => b.PayAmount).Sum())
            }).ToList();
            var pie2List = payList.GroupBy(x => new { x.PayWay }).Select(x => new
            {
                PayWay = x.Key.PayWay,
                Total = x.Sum(s => s.PayAmount),
            }).ToList();

            var days = (fdaParams.EndDate - fdaParams.StartDate).Days;
            List<FncMonthData> dailyList = new List<FncMonthData>();
            for (var i = 0; i < days; i++)
            {
                var daily = new FncMonthData();
                daily.Month = fdaParams.StartDate.AddDays(i).Date;
                daily.HouseTotal = fdaParams.Count;
                daily.HouseAmount = frontList.Where(x => DateTime.Compare(x.StartDate.Date, daily.Month) <= 0 && DateTime.Compare(daily.Month, x.EndDate.Date) < 0).Select(x => x.UnitPrice).Sum();
                daily.HouseCount = frontList.Where(x => DateTime.Compare(x.StartDate.Date, daily.Month) <= 0 && DateTime.Compare(daily.Month, x.EndDate.Date) < 0).Select(x => x.Count).Count();
                if (daily.HouseTotal != 0)
                {
                    daily.Rate = (double)daily.HouseCount / (double)daily.HouseTotal;
                    daily.ValidAverage = daily.HouseAmount / daily.HouseTotal;
                }
                else
                {
                    daily.Rate = 0;
                    daily.ValidAverage = 0;
                }
                if (daily.HouseCount != 0)
                    daily.Average = daily.HouseAmount / daily.HouseCount;
                else
                    daily.Average = 0;
                dailyList.Add(daily);
            }
            return Json(new { frontList, pie1List, pie2List, dailyList });
        }
    }

    public class FDAParams : FncBranch
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<long> Ids { get; set; }
    }
}
