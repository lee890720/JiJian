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
    [Authorize(Roles = "Admins,管理员,人事,财务")]
    public class FncHouseTypeController : Controller
    {
        private readonly AppDbContext _context;

        public FncHouseTypeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            ViewData["BranchId"] = id;
            var fncHouseType = _context.FncHouseType.Include(f => f.FncBranch).Where(x => x.BranchId == id).ToList();
            var fncHouseNumber = _context.FncHouseNumber.Include(f => f.FncHouseType).Where(x => x.FncHouseType.BranchId == id).ToList();
            return View(Tuple.Create(fncHouseType, fncHouseNumber));
        }

        public IActionResult Create(int? id)
        {
            ViewData["BranchName"] = _context.FncBranch.SingleOrDefault(x => x.BranchId == id).BranchName;
            ViewData["BranchId"] = id;
            return PartialView("~/Areas/Finance/Views/FncHouseType/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HouseTypeId,BranchId,Order,HouseType")] FncHouseType fncHouseType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncHouseType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = fncHouseType.BranchId });
            }
            return PartialView("~/Areas/Finance/Views/FncHouseType/CreateEdit.cshtml", fncHouseType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncHouseType = await _context.FncHouseType.SingleOrDefaultAsync(m => m.HouseTypeId == id);
            if (fncHouseType == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Finance/Views/FncHouseType/CreateEdit.cshtml", fncHouseType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HouseTypeId,BranchId,Order,HouseType")] FncHouseType fncHouseType)
        {
            if (id != fncHouseType.HouseTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fncHouseType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FncHouseTypeExists(fncHouseType.HouseTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = fncHouseType.BranchId });
            }
            return PartialView("~/Areas/Finance/Views/FncHouseType/CreateEdit.cshtml", fncHouseType);
        }

        public IActionResult Create2(int? id)
        {
            ViewData["HouseTypeId"] = id;
            return PartialView("~/Areas/Finance/Views/FncHouseType/Create2.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2([Bind("HouseNumberId,HouseTypeId,HouseNumber")] FncHouseNumber fncHouseNumber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fncHouseNumber);
                await _context.SaveChangesAsync();
                var fid = _context.FncHouseNumber.Include(x => x.FncHouseType).SingleOrDefault(x => x.HouseNumberId == fncHouseNumber.HouseNumberId).FncHouseType.BranchId;
                return RedirectToAction(nameof(Index), new { id = fid });
            }
            return PartialView("~/Areas/Finance/Views/FncHouseType/Create2.cshtml", fncHouseNumber);
        }


        public async Task<IActionResult> Edit2(string id)
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
            return PartialView("~/Areas/Finance/Views/FncHouseType/Edit2.cshtml", fncHouseNumber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(string id, [Bind("HouseNumberId,HouseTypeId,HouseNumber")] FncHouseNumber fncHouseNumber)
        {
            if (id != fncHouseNumber.HouseNumberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var temp = _context.FncHouseNumber.SingleOrDefault(x => x.HouseNumberId == fncHouseNumber.HouseNumberId);
                    _context.Remove(temp);
                    fncHouseNumber.HouseNumberId = fncHouseNumber.HouseNumber;
                    _context.Add(fncHouseNumber);
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
                var fid = _context.FncHouseNumber.Include(x => x.FncHouseType).SingleOrDefault(x => x.HouseNumberId == fncHouseNumber.HouseNumberId).FncHouseType.BranchId;
                return RedirectToAction(nameof(Index), new { id = fid });
            }
            return PartialView("~/Areas/Finance/Views/FncHouseType/Edit2.cshtml", fncHouseNumber);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fncHouseType = await _context.FncHouseType
                .Include(f => f.FncBranch)
                .SingleOrDefaultAsync(m => m.HouseTypeId == id);
            if (fncHouseType == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Finance/Views/FncHouseType/Delete.cshtml", fncHouseType.HouseType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var fncHouseType = await _context.FncHouseType.SingleOrDefaultAsync(m => m.HouseTypeId == id);
            _context.FncHouseType.Remove(fncHouseType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = fncHouseType.BranchId });
        }

        public async Task<IActionResult> Delete2(string id)
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

            return PartialView("~/Areas/Finance/Views/FncHouseType/Delete.cshtml", fncHouseNumber.HouseNumber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete2(string id, IFormCollection form)
        {
            var fncHouseNumber = await _context.FncHouseNumber.Include(x => x.FncHouseType).SingleOrDefaultAsync(m => m.HouseNumberId == id);
            _context.FncHouseNumber.Remove(fncHouseNumber);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = fncHouseNumber.FncHouseType.BranchId });
        }

        private bool FncHouseTypeExists(int id)
        {
            return _context.FncHouseType.Any(e => e.HouseTypeId == id);
        }

        private bool FncHouseNumberExists(string id)
        {
            return _context.FncHouseNumber.Any(e => e.HouseNumberId == id);
        }
    }
}
