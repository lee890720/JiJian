using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJNG.Data;
using JJNG.Data.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Identity;
using JJNG.Web;
using JJNG.Web.Areas.Branch.Models;

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
            ViewData["Department"] = _user.Department;
            ViewData["Branch"] = _user.Branch;
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

            var templist7 = _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.Branch &&  !x.IsFinish||x.UnitPrice==0).ToList();
            typename = "FrontIsFinish";
            amount = templist7.Sum(x => x.Receivable);
            count = templist7.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

            return View(Tuple.Create<BrhConnectRecord,List<BrhMemo>,BrhImprestAccounts,List<BrhFrontDeskAccounts>,List<BrhCollectModel>>(connectRecord,brhMemoList,brhImprestAccount,brhFrontDeskAccounts,brhCollectModel));
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

        private bool BrhMemoExists(int id)
        {
            return _context.BrhMemo.Any(e => e.MemoId == id);
        }
    }
}