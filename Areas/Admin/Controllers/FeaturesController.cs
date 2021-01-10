using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PinBackendSystem.Areas.Identity;
using PinBackendSystem.Data;
using PinBackendSystem.Models;
using PinBackendSystem.Util;

namespace PinBackendSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FeaturesController : Controller
    {
        private readonly PinContext _context;
        private UserManager<PinrumahUser> _userManager;

        public FeaturesController(PinContext context, UserManager<PinrumahUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Admin/Features
        public async Task<IActionResult> Index()
        {
            return View(await _context.Features.Where(s=>s.Status== PinrumahConstants.AdsStatus).ToListAsync());
        }

        // GET: Admin/Features/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // GET: Admin/Features/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Features/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FeatureName,Icon,Status,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                //get user detail
                var user = await _userManager.GetUserAsync(User);

                feature.CreatedBy = user.Id;
                feature.CreatedOn = DateTime.UtcNow;
                _context.Add(feature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feature);
        }

        // GET: Admin/Features/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }
            return View(feature);
        }

        // POST: Admin/Features/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FeatureName,Icon,Status,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Feature feature)
        {
            if (id != feature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //get user detail
                    var user = await _userManager.GetUserAsync(User);

                    feature.ModifiedBy = user.Id;
                    feature.ModifiedOn = DateTime.UtcNow;
                    _context.Update(feature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeatureExists(feature.Id))
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
            return View(feature);
        }

        // GET: Admin/Features/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // POST: Admin/Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feature = await _context.Features.FindAsync(id);
            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeatureExists(int id)
        {
            return _context.Features.Any(e => e.Id == id);
        }
    }
}
