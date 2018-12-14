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
    public class FncChannelController : Controller
    {
        private readonly AppDbContext _context;

        public FncChannelController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FncChannel.ToListAsync());
        }

        public IActionResult Create()
        {
               return PartialView("~/Areas/Finance/Views/FncChannel/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ChannelName,Sequence")] FncChannel fncChannel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncChannel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         return PartialView("~/Areas/Finance/Views/FncChannel/CreateEdit.cshtml",fncChannel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncChannel = await _context.FncChannel.SingleOrDefaultAsync(m => m.Id == id);
            if (fncChannel == null)
            {
                return NotFound();
            }
         return PartialView("~/Areas/Finance/Views/FncChannel/CreateEdit.cshtml",fncChannel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChannelName,Sequence")] FncChannel fncChannel)
        {
            if (id != fncChannel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncChannel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncChannelExists(fncChannel.Id))
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
         return PartialView("~/Areas/Finance/Views/FncChannel/CreateEdit.cshtml",fncChannel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncChannel = await _context.FncChannel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fncChannel == null)
            {
                return NotFound();
            }

         return PartialView("~/Areas/Finance/Views/FncChannel/Delete.cshtml",fncChannel.ChannelName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncChannel = await _context.FncChannel.SingleOrDefaultAsync(m => m.Id == id);
            _context.FncChannel.Remove(fncChannel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncChannelExists(int id)
        {
            return _context.FncChannel.Any(e => e.Id == id);
        }
    }
}
