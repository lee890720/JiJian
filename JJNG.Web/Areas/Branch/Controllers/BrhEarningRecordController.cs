using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台")]
    public class BrhEarningRecordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhEarningRecordController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;

            return View(await _context.BrhEarningRecord.Where(x => x.Branch == _user.BelongTo).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;

            var list_paymentType = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType");
            var list_earningType = _context.FncEarningType.ToList();
            ViewData["EarningType"] = new SelectList(list_earningType, "EarningType", "EarningType");

            return PartialView("~/Areas/Branch/Views/BrhEarningRecord/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EarningRecordId,EnteringDate,EarningType,Source,Amount,PaymentType,EnteringStaff,Branch,Note")] BrhEarningRecord brhEarningRecord)
        {
            if (ModelState.IsValid)
            {
                brhEarningRecord.EnteringDate = TimeZoneInfo.ConvertTime(brhEarningRecord.EnteringDate, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                _context.Add(brhEarningRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhEarningRecord/CreateEdit.cshtml", brhEarningRecord);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brhEarningRecord = await _context.BrhEarningRecord.SingleOrDefaultAsync(m => m.EarningRecordId == id);
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;

            var list_paymentType = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType", brhEarningRecord.PaymentType);
            var list_earningType = _context.FncEarningType.ToList();
            ViewData["EarningType"] = new SelectList(list_earningType, "EarningType", "EarningType", brhEarningRecord.EarningType);

            if (brhEarningRecord == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Branch/Views/BrhEarningRecord/CreateEdit.cshtml", brhEarningRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EarningRecordId,EnteringDate,EarningType,Source,Amount,PaymentType,EnteringStaff,Branch,Note")] BrhEarningRecord brhEarningRecord)
        {
            if (id != brhEarningRecord.EarningRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //brhEarningRecord.EnteringDate = TimeZoneInfo.ConvertTime(brhEarningRecord.EnteringDate, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                    _context.Update(brhEarningRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhEarningRecordExists(brhEarningRecord.EarningRecordId))
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
            return PartialView("~/Areas/Branch/Views/BrhEarningRecord/CreateEdit.cshtml", brhEarningRecord);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhEarningRecord = await _context.BrhEarningRecord
                .SingleOrDefaultAsync(m => m.EarningRecordId == id);
            if (brhEarningRecord == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhEarningRecord/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhEarningRecord = await _context.BrhEarningRecord.SingleOrDefaultAsync(m => m.EarningRecordId == id);
            _context.BrhEarningRecord.Remove(brhEarningRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhEarningRecordExists(int id)
        {
            return _context.BrhEarningRecord.Any(e => e.EarningRecordId == id);
        }
    }
}
