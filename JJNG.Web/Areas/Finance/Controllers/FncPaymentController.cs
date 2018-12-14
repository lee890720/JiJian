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
    public class FncPaymentController : Controller
    {
        private readonly AppDbContext _context;

        public FncPaymentController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FncPayment.ToListAsync());
        }

        public IActionResult Create()
        {
               return PartialView("~/Areas/Finance/Views/FncPayment/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PaymentName,Sequence")] FncPayment fncPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         return PartialView("~/Areas/Finance/Views/FncPayment/CreateEdit.cshtml",fncPayment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncPayment = await _context.FncPayment.SingleOrDefaultAsync(m => m.Id == id);
            if (fncPayment == null)
            {
                return NotFound();
            }
          return PartialView("~/Areas/Finance/Views/FncPayment/CreateEdit.cshtml",fncPayment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PaymentName,Sequence")] FncPayment fncPayment)
        {
            if (id != fncPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncPaymentExists(fncPayment.Id))
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
         return PartialView("~/Areas/Finance/Views/FncPayment/CreateEdit.cshtml",fncPayment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncPayment = await _context.FncPayment
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fncPayment == null)
            {
                return NotFound();
            }

           return PartialView("~/Areas/Finance/Views/FncPayment/Delete.cshtml",fncPayment.PaymentName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncPayment = await _context.FncPayment.SingleOrDefaultAsync(m => m.Id == id);
            _context.FncPayment.Remove(fncPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncPaymentExists(int id)
        {
            return _context.FncPayment.Any(e => e.Id == id);
        }
    }
}
