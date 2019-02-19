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
    [Authorize(Roles = "Admins,前台,前台审核")]
    public class BrhConnectRecordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identitycontext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhConnectRecordController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
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
            return View(await _context.BrhConnectRecord.Where(x => x.Branch == _user.Branch).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            return PartialView("~/Areas/Branch/Views/BrhConnectRecord/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConnectRecordId,EnteringDate,MorningStaff,NigthStaff,BillCount,HouseCash,OtherCash,CardCount,EnteringStaff,Branch,Note")] BrhConnectRecord brhConnectRecord)
        {
            if (ModelState.IsValid)
            {
                brhConnectRecord.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                _context.Add(brhConnectRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhConnectRecord/CreateEdit.cshtml", brhConnectRecord);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            var brhConnectRecord = await _context.BrhConnectRecord.SingleOrDefaultAsync(m => m.ConnectRecordId == id);
            if (brhConnectRecord == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Branch/Views/BrhConnectRecord/CreateEdit.cshtml", brhConnectRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConnectRecordId,EnteringDate,MorningStaff,NigthStaff,BillCount,HouseCash,OtherCash,CardCount,EnteringStaff,Branch,Note")] BrhConnectRecord brhConnectRecord)
        {
            if (id != brhConnectRecord.ConnectRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    brhConnectRecord.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                    _context.Update(brhConnectRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhConnectRecordExists(brhConnectRecord.ConnectRecordId))
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
            return PartialView("~/Areas/Branch/Views/BrhConnectRecord/CreateEdit.cshtml", brhConnectRecord);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhConnectRecord = await _context.BrhConnectRecord
                .SingleOrDefaultAsync(m => m.ConnectRecordId == id);
            if (brhConnectRecord == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhConnectRecord/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhConnectRecord = await _context.BrhConnectRecord.SingleOrDefaultAsync(m => m.ConnectRecordId == id);
            _context.BrhConnectRecord.Remove(brhConnectRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhConnectRecordExists(int id)
        {
            return _context.BrhConnectRecord.Any(e => e.ConnectRecordId == id);
        }
    }
}
