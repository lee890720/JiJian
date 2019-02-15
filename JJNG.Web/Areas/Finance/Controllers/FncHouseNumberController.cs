using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJNG.Data;
using JJNG.Data.Finance;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class FncHouseNumberController : Controller
    {
        private readonly AppDbContext _context;

        public FncHouseNumberController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Finance/FncHouseNumber
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FncHouseNumber.Include(f => f.FncHouseType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Finance/FncHouseNumber/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncHouseNumber = await _context.FncHouseNumber
                .Include(f => f.FncHouseType)
                .SingleOrDefaultAsync(m => m.HouseNumberId == id);
            if (fncHouseNumber == null)
            {
                return NotFound();
            }

            return View(fncHouseNumber);
        }

        // GET: Finance/FncHouseNumber/Create
        public IActionResult Create()
        {
            ViewData["HouseTypeId"] = new SelectList(_context.FncHouseType, "HouseTypeId", "HouseTypeId");
            return View();
        }

        // POST: Finance/FncHouseNumber/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HouseNumberId,HouseTypeId,HouseNumber")] FncHouseNumber fncHouseNumber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncHouseNumber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HouseTypeId"] = new SelectList(_context.FncHouseType, "HouseTypeId", "HouseTypeId", fncHouseNumber.HouseTypeId);
            return View(fncHouseNumber);
        }

        // GET: Finance/FncHouseNumber/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncHouseNumber = await _context.FncHouseNumber.SingleOrDefaultAsync(m => m.HouseNumberId == id);
            if (fncHouseNumber == null)
            {
                return NotFound();
            }
            ViewData["HouseTypeId"] = new SelectList(_context.FncHouseType, "HouseTypeId", "HouseTypeId", fncHouseNumber.HouseTypeId);
            return View(fncHouseNumber);
        }

        // POST: Finance/FncHouseNumber/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("HouseNumberId,HouseTypeId,HouseNumber")] FncHouseNumber fncHouseNumber)
        {
            if (id != fncHouseNumber.HouseNumberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncHouseNumber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncHouseNumberExists(fncHouseNumber.HouseNumberId))
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
            ViewData["HouseTypeId"] = new SelectList(_context.FncHouseType, "HouseTypeId", "HouseTypeId", fncHouseNumber.HouseTypeId);
            return View(fncHouseNumber);
        }

        // GET: Finance/FncHouseNumber/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncHouseNumber = await _context.FncHouseNumber
                .Include(f => f.FncHouseType)
                .SingleOrDefaultAsync(m => m.HouseNumberId == id);
            if (fncHouseNumber == null)
            {
                return NotFound();
            }

            return View(fncHouseNumber);
        }

        // POST: Finance/FncHouseNumber/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var fncHouseNumber = await _context.FncHouseNumber.SingleOrDefaultAsync(m => m.HouseNumberId == id);
            _context.FncHouseNumber.Remove(fncHouseNumber);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncHouseNumberExists(string id)
        {
            return _context.FncHouseNumber.Any(e => e.HouseNumberId == id);
        }
    }
}
