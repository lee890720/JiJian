using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,管理员,人事,财务")]
    public class FncBranchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;

        public FncBranchController(AppDbContext context, AppIdentityDbContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FncBranch.ToListAsync());
        }

        public IActionResult Create()
        {
            return PartialView("~/Areas/Finance/Views/FncBranch/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchId,BranchName")] FncBranch fncBranch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncBranch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Finance/Views/FncBranch/CreateEdit.cshtml", fncBranch);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncBranch = await _context.FncBranch.SingleOrDefaultAsync(m => m.BranchId == id);
            if (fncBranch == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Finance/Views/FncBranch/CreateEdit.cshtml", fncBranch);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BranchId,BranchName")] FncBranch fncBranch)
        {
            if (id != fncBranch.BranchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncBranch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncBranchExists(fncBranch.BranchId))
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
            return PartialView("~/Areas/Finance/Views/FncBranch/CreateEdit.cshtml", fncBranch);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncBranch = await _context.FncBranch
                .SingleOrDefaultAsync(m => m.BranchId == id);
            if (fncBranch == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Finance/Views/FncBranch/Delete.cshtml", fncBranch.BranchName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncBranch = await _context.FncBranch.SingleOrDefaultAsync(m => m.BranchId == id);
            _context.FncBranch.Remove(fncBranch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FncBranchExists(int id)
        {
            return _context.FncBranch.Any(e => e.BranchId == id);
        }
    }
}
