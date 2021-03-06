﻿using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Personnel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台,管家,前台审核,管家审核")]
    public class BrhAddressController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhAddressController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index(int? id)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["AddressAccountId"] = id;

            return View(await _context.PsnAddress.Where(x => x.AddressAccountId == id).ToListAsync());
        }

        public async Task<IActionResult> Create(int? id)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["AddressAccountId"] = id;
            return PartialView("~/Areas/Branch/Views/BrhAddress/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,AddressAccountId,Name,Purpose,Phone,EnteringStaff,Branch,Address")] PsnAddress psnAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(psnAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = psnAddress.AddressAccountId });
            }
            return PartialView("~/Areas/Branch/Views/BrhAddress/CreateEdit.cshtml", psnAddress);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var psnAddress = await _context.PsnAddress.SingleOrDefaultAsync(m => m.AddressId == id);
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            if (psnAddress == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Branch/Views/BrhAddress/CreateEdit.cshtml", psnAddress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AddressId,AddressAccountId,Name,Purpose,Phone,EnteringStaff,Branch,Address")] PsnAddress psnAddress)
        {
            if (id != psnAddress.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(psnAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PsnAddressExists(psnAddress.AddressId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = psnAddress.AddressAccountId });
            }
            return PartialView("~/Areas/Branch/Views/BrhAddress/CreateEdit.cshtml", psnAddress);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var psnAddress = await _context.PsnAddress
                .SingleOrDefaultAsync(m => m.AddressId == id);
            if (psnAddress == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhAddress/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var psnAddress = await _context.PsnAddress.SingleOrDefaultAsync(m => m.AddressId == id);
            _context.PsnAddress.Remove(psnAddress);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = psnAddress.AddressAccountId });
        }

        private bool PsnAddressExists(int id)
        {
            return _context.PsnAddress.Any(e => e.AddressId == id);
        }
    }
}
