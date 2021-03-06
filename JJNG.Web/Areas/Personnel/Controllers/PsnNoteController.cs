﻿using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Personnel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Personnel.Controllers
{
    [Area("Personnel")]
    [Authorize(Roles = "Admins,人事")]
    public class PsnNoteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public PsnNoteController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
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
            ViewData["NoteAccountId"] = id;

            return View(await _context.PsnNote.Where(x => x.NoteAccountId == id).ToListAsync());
        }

        public async Task<IActionResult> Create(int? id)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["NoteAccountId"] = id;
            return PartialView("~/Areas/Personnel/Views/PsnNote/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoteId,NoteAccountId,Account,Password,Platform,Phone,EnteringStaff,Branch,Note")] PsnNote psnNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(psnNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = psnNote.NoteAccountId });
            }
            return PartialView("~/Areas/Personnel/Views/PsnNote/CreateEdit.cshtml", psnNote);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var psnNote = await _context.PsnNote.SingleOrDefaultAsync(m => m.NoteId == id);
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            if (psnNote == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Personnel/Views/PsnNote/CreateEdit.cshtml", psnNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoteId,NoteAccountId,Account,Password,Platform,Phone,EnteringStaff,Branch,Note")] PsnNote psnNote)
        {
            if (id != psnNote.NoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(psnNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PsnNoteExists(psnNote.NoteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = psnNote.NoteAccountId });
            }
            return PartialView("~/Areas/Personnel/Views/PsnNote/CreateEdit.cshtml", psnNote);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var psnNote = await _context.PsnNote
                .SingleOrDefaultAsync(m => m.NoteId == id);
            if (psnNote == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Personnel/Views/PsnNote/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var psnNote = await _context.PsnNote.SingleOrDefaultAsync(m => m.NoteId == id);
            _context.PsnNote.Remove(psnNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PsnNoteExists(int id)
        {
            return _context.PsnNote.Any(e => e.NoteId == id);
        }
    }
}
