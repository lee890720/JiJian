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
    public class FncPaymentTypeController : Controller
    {
        private readonly AppDbContext _context;

        public FncPaymentTypeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FncPaymentType.ToListAsync());
        }

        public IActionResult Create()
        {
               return PartialView("~/Areas/Finance/Views/FncPaymentType/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PaymentType,Sequence")] FncPaymentType fncPaymentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncPaymentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         return PartialView("~/Areas/Finance/Views/FncPaymentType/CreateEdit.cshtml", fncPaymentType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncPaymentType = await _context.FncPaymentType.SingleOrDefaultAsync(m => m.Id == id);
            if (fncPaymentType == null)
            {
                return NotFound();
            }
          return PartialView("~/Areas/Finance/Views/FncPaymentType/CreateEdit.cshtml", fncPaymentType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PaymentType,Sequence")] FncPaymentType fncPaymentType)
        {
            if (id != fncPaymentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncPaymentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncPaymentTypeExists(fncPaymentType.Id))
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
         return PartialView("~/Areas/Finance/Views/FncPaymentType/CreateEdit.cshtml", fncPaymentType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncPaymentType = await _context.FncPaymentType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fncPaymentType == null)
            {
                return NotFound();
            }

           return PartialView("~/Areas/Finance/Views/FncPaymentType/Delete.cshtml", fncPaymentType.PaymentType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncPaymentType = await _context.FncPaymentType.SingleOrDefaultAsync(m => m.Id == id);
            _context.FncPaymentType.Remove(fncPaymentType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncPaymentTypeExists(int id)
        {
            return _context.FncPaymentType.Any(e => e.Id == id);
        }
    }
}
