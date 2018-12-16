using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJNG.Data;
using JJNG.Data.Branch;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    public class BrhMemoController : Controller
    {
        private readonly AppDbContext _context;

        public BrhMemoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Branch/BrhMemo
        public async Task<IActionResult> Index()
        {
            return View(await _context.BrhMemo.ToListAsync());
        }

        // GET: Branch/BrhMemo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhMemo = await _context.BrhMemo
                .SingleOrDefaultAsync(m => m.MemoId == id);
            if (brhMemo == null)
            {
                return NotFound();
            }

            return View(brhMemo);
        }

        // GET: Branch/BrhMemo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branch/BrhMemo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemoId,EnteringDate,Memo,EnteringStaff,Branch,Note")] BrhMemo brhMemo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brhMemo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brhMemo);
        }

        // GET: Branch/BrhMemo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhMemo = await _context.BrhMemo.SingleOrDefaultAsync(m => m.MemoId == id);
            if (brhMemo == null)
            {
                return NotFound();
            }
            return View(brhMemo);
        }

        // POST: Branch/BrhMemo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemoId,EnteringDate,Memo,EnteringStaff,Branch,Note")] BrhMemo brhMemo)
        {
            if (id != brhMemo.MemoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brhMemo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhMemoExists(brhMemo.MemoId))
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
            return View(brhMemo);
        }

        // GET: Branch/BrhMemo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhMemo = await _context.BrhMemo
                .SingleOrDefaultAsync(m => m.MemoId == id);
            if (brhMemo == null)
            {
                return NotFound();
            }

            return View(brhMemo);
        }

        // POST: Branch/BrhMemo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brhMemo = await _context.BrhMemo.SingleOrDefaultAsync(m => m.MemoId == id);
            _context.BrhMemo.Remove(brhMemo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhMemoExists(int id)
        {
            return _context.BrhMemo.Any(e => e.MemoId == id);
        }
    }
}
