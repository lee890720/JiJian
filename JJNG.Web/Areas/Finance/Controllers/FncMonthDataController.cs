using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using JJNG.Web.Areas.Branch.Models;
using JJNG.Web.Areas.Finance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncMonthDataController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncMonthDataController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
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

        public async Task<JsonResult> GetMonthData([FromBody]FncBranch fncBranch)
        {
            var frontList = _context.BrhFrontDeskAccounts.Where(x => x.Branch == fncBranch.BranchName && x.State != StateType.已删除 && DateTime.Compare(x.StartDate.Date, DateTime.Now.Date) <= 0).ToList();
            var earningList = _context.BrhEarningRecord.Where(x => x.Branch == fncBranch.BranchName).ToList();
            var expendList = _context.BrhExpendRecord.Where(x => x.Branch == fncBranch.BranchName).ToList();
            var stewardList = _context.BrhStewardAccounts.Where(x => x.Branch == fncBranch.BranchName).ToList();
            var start = frontList.Select(x => x.StartDate).Min().Date;
            start = start.AddDays(1 - start.Day);
            var end = frontList.Select(x => x.EndDate).Max().Date;
            if (DateTime.Compare(end, DateTime.Now) < 0)
                end = DateTime.Now.AddDays(1).Date;
            end = end.AddDays(1 - end.Day);
            var ms = (end.Year * 12 + end.Month) - (start.Year * 12 + start.Month);
            if (ms == 0)
                return Json(new { });
            var monthDataList = new List<MonthData>();
            var fncMonthDataList = _context.FncMonthData.Where(x => x.BranchId == fncBranch.BranchId).ToList();
            var monthData_Month = fncMonthDataList.Select(x => x.Month.ToString("yyyy-MM")).ToList();
            for (var i = 0; i <= ms; i++)
            {
                var fncMonthData = new FncMonthData();
                var cFrontList = frontList.Where(x => DateTime.Compare(start.AddMonths(i), x.StartDate) <= 0 && DateTime.Compare(x.StartDate, start.AddMonths(i + 1)) < 0).ToList();
                var cEarningList = earningList.Where(x => DateTime.Compare(start.AddMonths(i), x.EnteringDate) <= 0 && DateTime.Compare(x.EnteringDate, start.AddMonths(i + 1)) < 0).ToList();
                var cExpendList = expendList.Where(x => DateTime.Compare(start.AddMonths(i), x.EnteringDate) <= 0 && DateTime.Compare(x.EnteringDate, start.AddMonths(i + 1)) < 0).ToList();
                var cStewardList = stewardList.Where(x => DateTime.Compare(start.AddMonths(i), x.EnteringDate) <= 0 && DateTime.Compare(x.EnteringDate, start.AddMonths(i + 1)) < 0).ToList();
                if (!monthData_Month.Contains(start.AddMonths(i).ToString("yyyy-MM")))
                {
                    fncMonthData.BranchId = fncBranch.BranchId;
                    fncMonthData.Earning = cEarningList.Select(x => x.Amount).Sum();
                    fncMonthData.Expend = cExpendList.Select(x => x.Amount).Sum();
                    fncMonthData.HouseAmount = cFrontList.Select(x => x.TotalPrice).Sum();
                    fncMonthData.HouseCount = cFrontList.Select(x => x.Count).Sum();
                    if (i != ms)
                    {
                        fncMonthData.HouseTotal = DateTime.DaysInMonth(start.AddMonths(i).Year, start.AddMonths(i).Month) * fncBranch.Count;
                    }
                    else
                    {
                        fncMonthData.HouseTotal = DateTime.Now.Day * fncBranch.Count;
                    }
                    if (fncMonthData.HouseTotal != 0)
                    {
                        fncMonthData.Rate = (double)fncMonthData.HouseCount / (double)fncMonthData.HouseTotal;
                        fncMonthData.ValidAverage = fncMonthData.HouseAmount / fncMonthData.HouseTotal;
                    }
                    else
                    {
                        fncMonthData.Rate = 0;
                        fncMonthData.ValidAverage = 0;
                    }
                    if (fncMonthData.HouseCount != 0)
                        fncMonthData.Average = fncMonthData.HouseAmount / fncMonthData.HouseCount;
                    else
                        fncMonthData.Average = 0;
                    fncMonthData.Month = start.AddMonths(i);
                    fncMonthData.SaleAmount = cStewardList.Select(x => x.Amount).Sum();
                    fncMonthData.SaleProfit = cStewardList.Select(x => x.Profit).Sum();
                    _context.Add(fncMonthData);
                    fncMonthDataList.Add(fncMonthData);
                }
                else if (i == ms - 1)
                {
                    var tempMonthData = fncMonthDataList.SingleOrDefault(x => DateTime.Compare(x.Month, start.AddMonths(i)) == 0);
                    tempMonthData.BranchId = fncBranch.BranchId;
                    tempMonthData.Earning = cEarningList.Select(x => x.Amount).Sum();
                    tempMonthData.Expend = cExpendList.Select(x => x.Amount).Sum();
                    tempMonthData.HouseAmount = cFrontList.Select(x => x.TotalPrice).Sum();
                    tempMonthData.HouseCount = cFrontList.Select(x => x.Count).Sum();
                    tempMonthData.HouseTotal = DateTime.DaysInMonth(start.AddMonths(i).Year, start.AddMonths(i).Month) * fncBranch.Count;
                    if (tempMonthData.HouseTotal != 0)
                    {
                        tempMonthData.Rate = (double)tempMonthData.HouseCount / (double)tempMonthData.HouseTotal;
                        tempMonthData.ValidAverage = tempMonthData.HouseAmount / tempMonthData.HouseTotal;
                    }
                    else
                    {
                        tempMonthData.Rate = 0;
                        tempMonthData.ValidAverage = 0;
                    }
                    if (tempMonthData.HouseCount != 0)
                        tempMonthData.Average = tempMonthData.HouseAmount / tempMonthData.HouseCount;
                    else
                        tempMonthData.Average = 0;
                    tempMonthData.Month = start.AddMonths(i);
                    tempMonthData.SaleAmount = cStewardList.Select(x => x.Amount).Sum();
                    tempMonthData.SaleProfit = cStewardList.Select(x => x.Profit).Sum();
                    _context.Update(tempMonthData);
                }
                else if (i == ms)
                {
                    var tempMonthData = fncMonthDataList.SingleOrDefault(x => DateTime.Compare(x.Month, start.AddMonths(i)) == 0);
                    tempMonthData.BranchId = fncBranch.BranchId;
                    tempMonthData.Earning = cEarningList.Select(x => x.Amount).Sum();
                    tempMonthData.Expend = cExpendList.Select(x => x.Amount).Sum();
                    tempMonthData.HouseAmount = cFrontList.Select(x => x.TotalPrice).Sum();
                    tempMonthData.HouseCount = cFrontList.Select(x => x.Count).Sum();
                    tempMonthData.HouseTotal = DateTime.Now.Day * fncBranch.Count;
                    if (tempMonthData.HouseTotal != 0)
                    {
                        tempMonthData.Rate = (double)tempMonthData.HouseCount / (double)tempMonthData.HouseTotal;
                        tempMonthData.ValidAverage = tempMonthData.HouseAmount / tempMonthData.HouseTotal;
                    }
                    else
                    {
                        tempMonthData.Rate = 0;
                        tempMonthData.ValidAverage = 0;
                    }
                    if (tempMonthData.HouseCount != 0)
                        tempMonthData.Average = tempMonthData.HouseAmount / tempMonthData.HouseCount;
                    else
                        tempMonthData.Average = 0;
                    tempMonthData.Month = start.AddMonths(i);
                    tempMonthData.SaleAmount = cStewardList.Select(x => x.Amount).Sum();
                    tempMonthData.SaleProfit = cStewardList.Select(x => x.Profit).Sum();
                    _context.Update(tempMonthData);
                }
                await _context.SaveChangesAsync();
            }

            foreach (var fncMonthData1 in fncMonthDataList)
            {
                var monthData1 = new MonthData();
                var ParentType = typeof(FncMonthData);
                var Properties = ParentType.GetProperties();
                foreach (var Propertie in Properties)
                {
                    if (Propertie.CanRead && Propertie.CanWrite)
                    {
                        Propertie.SetValue(monthData1, Propertie.GetValue(fncMonthData1, null), null);
                    }
                }
                var fncMonthData2 = fncMonthDataList.SingleOrDefault(x => x.Month.ToString("yyyy-MM") == fncMonthData1.Month.AddMonths(-1).ToString("yyyy-MM"));
                if (fncMonthData2 != null)
                {
                    monthData1.环比增长额 = fncMonthData1.HouseAmount - fncMonthData2.HouseAmount;
                    monthData1.环比增长率 = (double)monthData1.环比增长额 / (double)fncMonthData2.HouseAmount;
                    monthData1.出环比增长额 = (decimal)(fncMonthData1.Rate - fncMonthData2.Rate);
                    monthData1.出环比增长率 = (double)monthData1.出环比增长额 / fncMonthData2.Rate;
                    monthData1.均环比增长额 = fncMonthData1.Average - fncMonthData2.Average;
                    monthData1.均环比增长率 = (double)monthData1.均环比增长额 / (double)fncMonthData2.Average;
                    monthData1.有环比增长额 = fncMonthData1.ValidAverage - fncMonthData2.ValidAverage;
                    monthData1.有环比增长率 = (double)monthData1.有环比增长额 / (double)fncMonthData2.ValidAverage;
                    monthData1.支环比增长额 = fncMonthData1.Expend - fncMonthData2.Expend;
                    monthData1.支环比增长率= (double)monthData1.支环比增长额 / (double)fncMonthData2.Expend;
                    monthData1.外环比增长额 = fncMonthData1.SaleAmount - fncMonthData2.SaleAmount;
                    monthData1.外环比增长率 = (double)monthData1.外环比增长额 / (double)fncMonthData2.SaleAmount;
                    monthData1.外利环比增长额 = fncMonthData1.SaleProfit - fncMonthData2.SaleProfit;
                    monthData1.外利环比增长率 = (double)monthData1.外利环比增长额 / (double)fncMonthData2.SaleProfit;
                }
                else
                {
                    monthData1.环比增长额 = 0;
                    monthData1.环比增长率 = 0;
                    monthData1.出环比增长额 = 0;
                    monthData1.出环比增长率 = 0;
                    monthData1.均环比增长额 = 0;
                    monthData1.均环比增长率 = 0;
                    monthData1.有环比增长额 = 0;
                    monthData1.有环比增长率 = 0;
                    monthData1.支环比增长额 = 0;
                    monthData1.支环比增长率 = 0;
                    monthData1.外环比增长额 = 0;
                    monthData1.外环比增长率 = 0;
                    monthData1.外利环比增长额 = 0;
                    monthData1.外利环比增长率 =0;
                }
                var fncMonthData3 = fncMonthDataList.SingleOrDefault(x => x.Month.ToString("yyyy-MM") == fncMonthData1.Month.AddYears(-1).ToString("yyyy-MM"));
                if (fncMonthData3 != null)
                {
                    monthData1.同比增长额 = fncMonthData1.HouseAmount - fncMonthData3.HouseAmount;
                    monthData1.同比增长率 = (double)monthData1.同比增长额 / (double)fncMonthData3.HouseAmount;
                    monthData1.出同比增长额 = (decimal)(fncMonthData1.Rate - fncMonthData3.Rate);
                    monthData1.出同比增长率 = (double)monthData1.出同比增长额 / fncMonthData3.Rate;
                    monthData1.均同比增长额 = fncMonthData1.Average - fncMonthData3.Average;
                    monthData1.均同比增长率 = (double)monthData1.均同比增长额 / (double)fncMonthData3.Average;
                    monthData1.有同比增长额 = fncMonthData1.ValidAverage - fncMonthData3.ValidAverage;
                    monthData1.有同比增长率 = (double)monthData1.有同比增长额 / (double)fncMonthData3.ValidAverage;
                    monthData1.支同比增长额 = fncMonthData1.Expend - fncMonthData3.Expend;
                    monthData1.支同比增长率 = (double)monthData1.支同比增长额 / (double)fncMonthData3.Expend;
                    monthData1.外同比增长额 = fncMonthData1.SaleAmount - fncMonthData3.SaleAmount;
                    monthData1.外同比增长率 = (double)monthData1.外同比增长额 / (double)fncMonthData3.SaleAmount;
                    monthData1.外利同比增长额 = fncMonthData1.SaleProfit - fncMonthData3.SaleProfit;
                    monthData1.外利同比增长率 = (double)monthData1.外利同比增长额 / (double)fncMonthData3.SaleProfit;
                }
                else
                {
                    monthData1.同比增长额 = 0;
                    monthData1.同比增长率 = 0;
                    monthData1.出同比增长额 = 0;
                    monthData1.出同比增长率 = 0;
                    monthData1.均同比增长额 = 0;
                    monthData1.均同比增长率 = 0;
                    monthData1.有同比增长额 = 0;
                    monthData1.有同比增长率 = 0;
                    monthData1.支同比增长额 = 0;
                    monthData1.支同比增长率 = 0;
                    monthData1.外同比增长额 = 0;
                    monthData1.外同比增长率 = 0;
                    monthData1.外利同比增长额 = 0;
                    monthData1.外利同比增长率 = 0;
                }
                monthDataList.Add(monthData1);
            }
            monthDataList = monthDataList.OrderBy(x => x.Month).ToList();

            var front2List = _context.BrhFrontDeskAccounts.Where(x => x.Branch == fncBranch.BranchName && x.State != StateType.已删除 && x.StartDate.Month != x.EndDate.AddDays(-1).Month).ToList();
            return Json(new { monthDataList, monthData_Month, front2List });
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

        public IActionResult Create()
        {
            return PartialView("~/Areas/Finance/Views/FncMonthData/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MonthDataId,BranchId,Month,HouseAmount,HouseTotal,HouseCount,Earning,Expend,SaleAmount,SaleProfit")] FncMonthData fncMonthData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncMonthData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Finance/Views/FncMonthData/CreateEdit.cshtml", fncMonthData);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncMonthData = await _context.FncMonthData.SingleOrDefaultAsync(m => m.MonthDataId == id);
            if (fncMonthData == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Finance/Views/FncMonthData/CreateEdit.cshtml", fncMonthData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MonthDataId,BranchId,Month,HouseAmount,HouseTotal,HouseCount,Earning,Expend,SaleAmount,SaleProfit")] FncMonthData fncMonthData)
        {
            if (id != fncMonthData.MonthDataId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncMonthData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncMonthDataExists(fncMonthData.MonthDataId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Finance/Views/FncMonthData/CreateEdit.cshtml", fncMonthData);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncMonthData = await _context.FncMonthData
                .Include(f => f.FncBranch)
                .SingleOrDefaultAsync(m => m.MonthDataId == id);
            if (fncMonthData == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Finance/Views/FncMonthData/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncMonthData = await _context.FncMonthData.SingleOrDefaultAsync(m => m.MonthDataId == id);
            _context.FncMonthData.Remove(fncMonthData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncMonthDataExists(int id)
        {
            return _context.FncMonthData.Any(e => e.MonthDataId == id);
        }
    }
}
