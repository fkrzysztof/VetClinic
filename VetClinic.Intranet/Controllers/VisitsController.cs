using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Intranet.Controllers.Abastract;
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class VisitsController : AbstractPolicyController
    {
        public VisitsController(VetClinicContext context) : base(context) { }


        //private readonly VetClinicContext _context;

        //public VisitsController(VetClinicContext context)
        //{
        //    _context = context;
        //}

        // GET: Visits
        public async Task<IActionResult> Index()
        {
            //var vetClinicContext = _context.Visits.Include(v => v.Patient).Include(v => v.Treatment).Include(v => v.VetUser).Include(v => v.VisitAddedUser).Include(v => v.VisitUpdatedUser).Include(v => v.VisitUser);
            //return View(await vetClinicContext.ToListAsync());
            return View();
        }

        // GET: Visits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Treatment)
                .Include(v => v.VetUser)
                .Include(v => v.VisitAddedUser)
                .Include(v => v.VisitUpdatedUser)
                .Include(v => v.VisitUser)
                .FirstOrDefaultAsync(m => m.VisitID == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // GET: Visits/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name");
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name");
            ViewData["VetID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitID,VisitUserID,PatientID,VetID,TreatmentID,DateOfVisit,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", visit.PatientID);
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name", visit.TreatmentID);
            ViewData["VetID"] = new SelectList(_context.Users, "UserID", "City", visit.VetID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", visit.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", visit.UpdatedUserID);
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "City", visit.VisitUserID);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", visit.PatientID);
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name", visit.TreatmentID);
            ViewData["VetID"] = new SelectList(_context.Users, "UserID", "City", visit.VetID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", visit.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", visit.UpdatedUserID);
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "City", visit.VisitUserID);
            return View(visit);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitID,VisitUserID,PatientID,VetID,TreatmentID,DateOfVisit,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Visit visit)
        {
            if (id != visit.VisitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(visit.VisitID))
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
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", visit.PatientID);
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name", visit.TreatmentID);
            ViewData["VetID"] = new SelectList(_context.Users, "UserID", "City", visit.VetID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", visit.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", visit.UpdatedUserID);
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "City", visit.VisitUserID);
            return View(visit);
        }

        // GET: Visits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Treatment)
                .Include(v => v.VetUser)
                .Include(v => v.VisitAddedUser)
                .Include(v => v.VisitUpdatedUser)
                .Include(v => v.VisitUser)
                .FirstOrDefaultAsync(m => m.VisitID == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitExists(int id)
        {
            return _context.Visits.Any(e => e.VisitID == id);
        }
    }
}
