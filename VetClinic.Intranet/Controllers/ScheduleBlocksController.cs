using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Intranet.Controllers
{
    public class ScheduleBlocksController : Controller
    {
        private readonly VetClinicContext _context;

        public ScheduleBlocksController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: ScheduleBlocks
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScheduleBlocks.OrderBy(t=>t.Time).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: ScheduleBlocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleBlockID,Time,TimeInterval")] ScheduleBlock scheduleBlock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduleBlock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduleBlock);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleBlock = await _context.ScheduleBlocks
                .FirstOrDefaultAsync(m => m.ScheduleBlockID == id);
            if (scheduleBlock == null)
            {
                return NotFound();
            }

            return View(scheduleBlock);
        }

        // POST: ScheduleBlocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scheduleBlock = await _context.ScheduleBlocks.FindAsync(id);
            _context.ScheduleBlocks.Remove(scheduleBlock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleBlockExists(int id)
        {
            return _context.ScheduleBlocks.Any(e => e.ScheduleBlockID == id);
        }
    }
}
