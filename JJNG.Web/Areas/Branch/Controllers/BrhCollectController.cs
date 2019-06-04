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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台,管家,前台审核,管家审核")]
    public class BrhCollectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identitycontext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhCollectController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identitycontext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["BranchId"] = _user.BranchId;
            var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchName == _user.Branch);
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            var connectRecord = _context.BrhConnectRecord.Where(x => x.Branch == _user.Branch).OrderByDescending(x => x.EnteringDate).LastOrDefault();
            var brhMemoList = _context.BrhMemo.Where(x=>x.Branch==_user.Branch&&x.IsFinish==false).ToList();
            var brhImprestAccount = _context.BrhImprestAccounts.SingleOrDefault(x => x.Department == _user.Department && x.Branch == _user.Branch&&string.IsNullOrEmpty(x.Manager));
            var brhFrontDeskAccounts=_context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.Branch&&DateTime.Compare(x.StartDate,now)<=0&&DateTime.Compare(x.EndDate,now)>0).ToList();

            List<BrhCollectModel> brhCollectModel = new List<BrhCollectModel>();
            var typename = "";
            decimal amount = 0.00M;
            var count = 0;
            var year = now.Year;
            var month = now.Month;
            var newdate = new DateTime(year, month, 1);
            var templist1 = _context.BrhEarningRecord.Where(x => x.Branch == _user.Branch && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
            typename = "Earning_M";
            amount = (decimal)templist1.Sum(x => x.Amount);
            count = templist1.Count;
            brhCollectModel.Add(new BrhCollectModel { Type=typename, Amount=amount, Count=count });
            var templist1d = _context.BrhEarningRecord.Where(x => x.Branch == _user.Branch && x.EnteringDate.Date == DateTime.Now.Date).ToList();
            typename = "Earning_D";
            amount = (decimal)templist1d.Sum(x => x.Amount);
            count = templist1d.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

            var templist2 =_context.BrhExpendRecord.Where(x => x.Branch == _user.Branch && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
            typename = "Expend_M";
            amount = (decimal)templist2.Sum(x => x.Amount);
            count = templist2.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
            var templist2d = _context.BrhExpendRecord.Where(x => x.Branch == _user.Branch && x.EnteringDate.Date == DateTime.Now.Date).ToList();
            typename = "Expend_D";
            amount = (decimal)templist2d.Sum(x => x.Amount);
            count = templist2d.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

            var templist3 = _context.BrhImprestRecord.Include(x=>x.BrhImprestAccounts).Where(x => x.Branch == _user.Branch && !x.IsFinance&&string.IsNullOrEmpty(x.BrhImprestAccounts.Manager)).ToList();
            typename = "Imprest";
            amount = (decimal)templist3.Sum(x => x.Amount);
            count = templist3.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

            var templist4 = _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.Branch && DateTime.Compare(x.StartDate, newdate) >= 0).ToList();
            var templist4d = _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.Branch && DateTime.Compare(x.StartDate, DateTime.Now.Date) <= 0 && DateTime.Compare(x.EndDate, DateTime.Now.Date) > 0).ToList();
            var houselist = _identitycontext.UserBranchDetial.Where(x => x.UserBranch.BranchName == _user.Branch).ToList();
            var housenum = houselist.Count;
            var housetotal = housenum * DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            decimal day_rate = 0;
            decimal month_rate = 0;
            if(housenum!=0)
             day_rate = (decimal)templist4d.Count / (decimal)housenum*100;
            if(housetotal!=0)
             month_rate = (decimal)templist4.Count / (decimal)housetotal*100;
            typename = "MonthRate";
            amount = Math.Round(month_rate, 2);
            count = 0;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
            typename = "DayRate";
            amount = Math.Round(day_rate,2);
            count = 0;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

            var templist5 = _context.BrhStewardAccounts.Where(x => x.Branch == _user.Branch && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
            typename = "Steward_M";
            amount = templist5.Sum(x => x.Receivable);
            count = templist5.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
            var templist5d = _context.BrhStewardAccounts.Where(x => x.Branch == _user.Branch && DateTime.Compare(x.EnteringDate.Date, DateTime.Now.Date) == 0).ToList();
            typename = "Steward_D";
            amount = templist5d.Sum(x => x.Receivable);
            count = templist5d.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

            var templist6 = _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.Branch && DateTime.Compare(x.StartDate, newdate) >= 0).ToList();
            typename = "Front_M";
            amount = templist6.Sum(x => x.TotalPrice);
            count = templist6.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
            var templist6d = _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.Branch && DateTime.Compare(x.StartDate, DateTime.Now.Date) <= 0 && DateTime.Compare(x.EndDate, DateTime.Now.Date) > 0).ToList();
            typename = "Front_D";
            amount = templist6d.Sum(x => x.UnitPrice);
            count = templist6d.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

            var templist7 = _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.Branch &&  !x.IsFinish).ToList();
            typename = "FrontIsFinish";
            amount = templist7.Sum(x => x.Receivable);
            count = templist7.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

            return View(Tuple.Create<BrhConnectRecord,List<BrhMemo>,BrhImprestAccounts,List<BrhFrontDeskAccounts>,List<BrhCollectModel>,FncBranch>(connectRecord,brhMemoList,brhImprestAccount,brhFrontDeskAccounts,brhCollectModel,fncBranch));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<int> ids)
        {
            if (ids.Count > 0)
            {
                _context.BrhMemo.Where(x => ids.Contains(x.MemoId) && !x.IsFinish).ToList().ForEach(x =>
                {
                    x.IsFinish = true;
                    _context.Update(x);
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            return PartialView("~/Areas/Branch/Views/BrhCollect/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemoId,EnteringDate,Memo,IsFinish,EnteringStaff,Branch,Note")] BrhMemo brhMemo)
        {
            if (ModelState.IsValid)
            {
                brhMemo.EnteringDate= TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                _context.Add(brhMemo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhCollect/CreateEdit.cshtml",brhMemo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhMemo = await _context.BrhMemo.SingleOrDefaultAsync(m => m.MemoId == id);
            if (brhMemo == null)
            {
                return NotFound();
            }
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            return PartialView("~/Areas/Branch/Views/BrhCollect/CreateEdit.cshtml",brhMemo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemoId,EnteringDate,Memo,EnteringStaff,Branch,Note")] BrhMemo brhMemo)
        {
            if (id != brhMemo.MemoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    brhMemo.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                    _context.Update(brhMemo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhMemoExists(brhMemo.MemoId))
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
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            return PartialView("~/Areas/Branch/Views/BrhCollect/CreateEdit.cshtml", brhMemo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhMemo = await _context.BrhMemo
                .SingleOrDefaultAsync(m => m.MemoId == id);
            if (brhMemo == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhCollect/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhMemo = await _context.BrhMemo.SingleOrDefaultAsync(m => m.MemoId == id);
            _context.BrhMemo.Remove(brhMemo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> GetGroup()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time")).Date;
            var frontDetials = _context.BrhFrontPaymentDetials.Include(x => x.BrhFrontDeskAccounts).Where(x =>x.BrhFrontDeskAccounts.Branch==_user.Branch&& x.PayDate.Date == now).ToList();
            List<BrhGroupModel> brhGroup = new List<BrhGroupModel>();
            var frontGroup = frontDetials.GroupBy(x => new { x.PayWay }).Select(s => new
            {
                PayWay = s.Key.PayWay,
                FrontAmount = s.Sum(x => x.PayAmount),
            }).ToList();
            var stewardDetials = _context.BrhStewardPaymentDetial.Include(x => x.BrhStewardAccounts).Where(x => x.BrhStewardAccounts.Branch == _user.Branch && x.PayDate.Date == now).ToList();
            var stewardGroup = stewardDetials.GroupBy(x => new { x.PayWay }).Select(s => new
            {
                PayWay = s.Key.PayWay,
                StewardAmount = s.Sum(x => x.PayAmount),
            }).ToList();
            var earningDetials = _context.BrhEarningRecord.Where(x => x.Branch == _user.Branch && x.EnteringDate.Date == now).ToList();
            var earningGroup = earningDetials.GroupBy(x => new { x.PaymentType }).Select(s => new
            {
                PayWay = s.Key.PaymentType,
                EarningAmount = s.Sum(x => x.Amount),
            }).ToList();
            foreach(var p in _context.FncPaymentType.ToList())
            {
                var isAdd = false;
                var brhG = new BrhGroupModel();
                if (frontGroup.Where(x=>x.PayWay==p.PaymentType).Count()>0)
                {
                    var frontG = frontGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                    brhG.PayWay = p.PaymentType;
                    brhG.FrontAmount = frontG.FrontAmount;
                    isAdd = true;
                }
                if (stewardGroup.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                {
                    var stewardG = stewardGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                    brhG.PayWay = p.PaymentType;
                    brhG.StewardAmount = stewardG.StewardAmount;
                    isAdd = true;
                }
                if (earningGroup.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                {
                    var earningG = earningGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                    brhG.PayWay = p.PaymentType;
                    brhG.EarningAmount = earningG.EarningAmount;
                    isAdd = true;
                }
                if (isAdd)
                {
                    brhG.Total = brhG.FrontAmount + brhG.StewardAmount + brhG.EarningAmount;
                    brhGroup.Add(brhG);
                }
            }
            return Json(new { brhGroup, brhGroup.Count});
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

        private bool BrhMemoExists(int id)
        {
            return _context.BrhMemo.Any(e => e.MemoId == id);
        }
    }
}
