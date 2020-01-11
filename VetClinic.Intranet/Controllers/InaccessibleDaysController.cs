using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class InaccessibleDaysController : AbstractPolicyController
    {
        public InaccessibleDaysController(VetClinicContext context) : base(context) { }

        // GET: InaccessibleDays
        public async Task<IActionResult> Index()
        {
            return View(await _context.InaccessibleDays.OrderBy(t => t.Date).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: InaccessibleDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InaccessibleDayID,Date")] InaccessibleDay inaccessibleDay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inaccessibleDay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inaccessibleDay);
        }

        // POST: InaccessibleDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inaccessibleDay = await _context.InaccessibleDays.FindAsync(id);
            _context.InaccessibleDays.Remove(inaccessibleDay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
