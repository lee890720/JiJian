using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJNG.Data;
using JJNG.Data.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncEarningTypeController : Controller
    {
        private readonly AppDbContext _context;

        public FncEarningTypeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FncEarningType.ToListAsync());
        }

        public IActionResult Create()
        {
               return PartialView("~/Areas/Finance/Views/FncEarningType/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EarningType,Sequence")] FncEarningType fncEarningType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncEarningType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         return PartialView("~/Areas/Finance/Views/FncEarningType/CreateEdit.cshtml", fncEarningType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncEarningType = await _context.FncEarningType.SingleOrDefaultAsync(m => m.Id == id);
            if (fncEarningType == null)
            {
                return NotFound();
            }
          return PartialView("~/Areas/Finance/Views/FncEarningType/CreateEdit.cshtml", fncEarningType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EarningType,Sequence")] FncEarningType fncEarningType)
        {
            if (id != fncEarningType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncEarningType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncEarningTypeExists(fncEarningType.Id))
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
         return PartialView("~/Areas/Finance/Views/FncEarningType/CreateEdit.cshtml", fncEarningType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncEarningType = await _context.FncEarningType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fncEarningType == null)
            {
                return NotFound();
            }

           return PartialView("~/Areas/Finance/Views/FncEarningType/Delete.cshtml", fncEarningType.EarningType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncEarningType = await _context.FncEarningType.SingleOrDefaultAsync(m => m.Id == id);
            _context.FncEarningType.Remove(fncEarningType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncEarningTypeExists(int id)
        {
            return _context.FncEarningType.Any(e => e.Id == id);
        }
    }
}
