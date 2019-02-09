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

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台")]
    public class BrhExpendRecordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identitycontext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhExpendRecordController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
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
            return View(await _context.BrhExpendRecord.Where(x => x.Branch == _user.Branch).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            var list_paymenttype = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymenttype, "PaymentType", "PaymentType");
            var list_expendtype = _context.FncExpendType.ToList();
            ViewData["ExpendType"] = new SelectList(list_expendtype, "ExpendType", "ExpendType");
            return PartialView("~/Areas/Branch/Views/BrhExpendRecord/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpendRecordId,EnteringDate,ExpendType,Purpose,Amount,PaymentType,ConnectNumber,EnteringStaff,Branch,Note")] BrhExpendRecord brhExpendRecord)
        {
            if (ModelState.IsValid)
            {
                brhExpendRecord.EnteringDate = TimeZoneInfo.ConvertTime(brhExpendRecord.EnteringDate, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                _context.Add(brhExpendRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhExpendRecord/CreateEdit.cshtml", brhExpendRecord);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brhExpendRecord = await _context.BrhExpendRecord.SingleOrDefaultAsync(m => m.ExpendRecordId == id);
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            var list_paymenttype = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymenttype, "PaymentType", "PaymentType",brhExpendRecord.PaymentType);
            var list_expendtype = _context.FncExpendType.ToList();
            ViewData["ExpendType"] = new SelectList(list_expendtype, "ExpendType", "ExpendType",brhExpendRecord.ExpendType);
            if (brhExpendRecord == null)
            {
                return NotFound();
            }
               return PartialView("~/Areas/Branch/Views/BrhExpendRecord/CreateEdit.cshtml", brhExpendRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpendRecordId,EnteringDate,ExpendType,Purpose,Amount,PaymentType,ConnectNumber,EnteringStaff,Branch,Note")] BrhExpendRecord brhExpendRecord)
        {
            if (id != brhExpendRecord.ExpendRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //brhExpendRecord.EnteringDate = TimeZoneInfo.ConvertTime(brhExpendRecord.EnteringDate, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                    _context.Update(brhExpendRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhExpendRecordExists(brhExpendRecord.ExpendRecordId))
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
                return PartialView("~/Areas/Branch/Views/BrhExpendRecord/CreateEdit.cshtml", brhExpendRecord);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhExpendRecord = await _context.BrhExpendRecord
                .SingleOrDefaultAsync(m => m.ExpendRecordId == id);
            if (brhExpendRecord == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhExpendRecord/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhExpendRecord = await _context.BrhExpendRecord.SingleOrDefaultAsync(m => m.ExpendRecordId == id);
            _context.BrhExpendRecord.Remove(brhExpendRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhExpendRecordExists(int id)
        {
            return _context.BrhExpendRecord.Any(e => e.ExpendRecordId == id);
        }
    }
}
