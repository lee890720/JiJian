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
    [Authorize(Roles = "Admins,管理员")]
    public class FncExpendTypeController : Controller
    {
        private readonly AppDbContext _context;

        public FncExpendTypeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FncExpendType.ToListAsync());
        }

        public IActionResult Create()
        {
               return PartialView("~/Areas/Finance/Views/FncExpendType/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExpendType,Sequence")] FncExpendType fncExpendType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncExpendType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         return PartialView("~/Areas/Finance/Views/FncExpendType/CreateEdit.cshtml", fncExpendType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncExpendType = await _context.FncExpendType.SingleOrDefaultAsync(m => m.Id == id);
            if (fncExpendType == null)
            {
                return NotFound();
            }
          return PartialView("~/Areas/Finance/Views/FncExpendType/CreateEdit.cshtml", fncExpendType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExpendType,Sequence")] FncExpendType fncExpendType)
        {
            if (id != fncExpendType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncExpendType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncExpendTypeExists(fncExpendType.Id))
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
         return PartialView("~/Areas/Finance/Views/FncExpendType/CreateEdit.cshtml", fncExpendType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncExpendType = await _context.FncExpendType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fncExpendType == null)
            {
                return NotFound();
            }

           return PartialView("~/Areas/Finance/Views/FncExpendType/Delete.cshtml", fncExpendType.ExpendType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncExpendType = await _context.FncExpendType.SingleOrDefaultAsync(m => m.Id == id);
            _context.FncExpendType.Remove(fncExpendType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncExpendTypeExists(int id)
        {
            return _context.FncExpendType.Any(e => e.Id == id);
        }
    }
}
