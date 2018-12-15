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
    public class FncChannelTypeController : Controller
    {
        private readonly AppDbContext _context;

        public FncChannelTypeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FncChannelType.ToListAsync());
        }

        public IActionResult Create()
        {
               return PartialView("~/Areas/Finance/Views/FncChannelType/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ChannelType,Sequence")] FncChannelType fncChannelType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncChannelType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         return PartialView("~/Areas/Finance/Views/FncChannelType/CreateEdit.cshtml",fncChannelType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncChannelType = await _context.FncChannelType.SingleOrDefaultAsync(m => m.Id == id);
            if (fncChannelType == null)
            {
                return NotFound();
            }
         return PartialView("~/Areas/Finance/Views/FncChannelType/CreateEdit.cshtml",fncChannelType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChannelType,Sequence")] FncChannelType fncChannelType)
        {
            if (id != fncChannelType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncChannelType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncChannelTypeExists(fncChannelType.Id))
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
         return PartialView("~/Areas/Finance/Views/FncChannelType/CreateEdit.cshtml",fncChannelType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fnChannelType = await _context.FncChannelType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fnChannelType == null)
            {
                return NotFound();
            }

         return PartialView("~/Areas/Finance/Views/FncChannelType/Delete.cshtml",fnChannelType.ChannelType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncChannelType = await _context.FncChannelType.SingleOrDefaultAsync(m => m.Id == id);
            _context.FncChannelType.Remove(fncChannelType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncChannelTypeExists(int id)
        {
            return _context.FncChannelType.Any(e => e.Id == id);
        }
    }
}
