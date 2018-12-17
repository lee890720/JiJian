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
using JJNG.Web.Areas.Branch.Model;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台")]
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
            ViewData["BelongTo"] = _user.BelongTo;
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            var connectRecord = _context.BrhConnectRecord.Where(x => x.Branch == _user.BelongTo).OrderByDescending(x => x.EnteringDate).LastOrDefault();
            var brhMemoList = _context.BrhMemo.Where(x=>x.Branch==_user.BelongTo&&x.IsFinish==false).ToList();
            var brhImprestAccount = _context.BrhImprestAccounts.SingleOrDefault(x => x.Department == _user.Department && x.BelongTo == _user.BelongTo&&string.IsNullOrEmpty(x.Manager));
            var brhFrontDeskAccounts=_context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.BelongTo&&DateTime.Compare(x.StartDate,now)<=0&&DateTime.Compare(x.EndDate,now)>0).ToList();

            List<BrhCollectModel> brhCollectModel = new List<BrhCollectModel>();
            var typename = "";
            var amount = 0.00;
            var count = 0;
            var year = now.Year;
            var month = now.Month;
            var newdate = new DateTime(year, month, 1);
            var templist1 = _context.BrhEarningRecord.Where(x => x.Branch == _user.BelongTo && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
            typename = "Earning";
            amount = templist1.Sum(x => x.Amount);
            count = templist1.Count;
            brhCollectModel.Add(new BrhCollectModel { Type=typename, Amount=amount, Count=count });
            var templist2=_context.BrhExpendRecord.Where(x => x.Branch == _user.BelongTo && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
            typename = "Expend";
            amount = templist2.Sum(x => x.Amount);
            count = templist2.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
            var templist3 = _context.BrhImprestRecord.Where(x => x.Branch == _user.BelongTo && !x.IsFinance).ToList();
            typename = "Imprest";
            amount = templist3.Sum(x => x.Amount);
            count = templist3.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
            var templist4 = _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.BelongTo && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
            typename = "Front";
            amount = templist4.Sum(x => x.Receivable);
            count = templist4.Count;
            brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
            var templist5 = templist4.Where(x => !x.IsFinish).ToList();
            typename = "FrontIsFinish";
            amount = templist5.Sum(x => x.Receivable);
            count = templist5.Count;
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
            ViewData["BelongTo"] = _user.BelongTo;
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
            ViewData["BelongTo"] = _user.BelongTo;
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
            ViewData["BelongTo"] = _user.BelongTo;
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

        private bool BrhMemoExists(int id)
        {
            return _context.BrhMemo.Any(e => e.MemoId == id);
        }
    }
}