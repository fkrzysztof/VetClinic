using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class PermissionsController : AbstractPolicyController
    {
        public PermissionsController(VetClinicContext context) : base(context) { }

        // GET: Permissions
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.Permissions.Include(p => p.PermissionAddedUser).Include(p => p.PermissionUpdatedUser);
            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }


        // GET: Permissions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PermissionID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Permission permission)
        {
            if (ModelState.IsValid)
            {
                int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                permission.AddedUserID = UserId;
                permission.AddedDate = DateTime.Now;
                permission.IsActive = true;
                var result = _context.Permissions.FirstOrDefault(f => f.Name == permission.Name);
                if (result == null)
                {
                    _context.Add(permission);
                    _context.SaveChanges();
                }
                else
                {
                    result.IsActive = true;
                    result.Description = permission.Description;
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(permission);
        }

        // GET: Permissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PermissionID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Permission permission)
        {

            if (id != permission.PermissionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    permission.UpdatedUserID = UserId;
                    permission.IsActive = true;
                    permission.UpdatedDate = DateTime.Now;
                    _context.Update(permission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionExists(permission.PermissionID))
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
            return View(permission);
        }


        // POST: Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);

            permission.IsActive = false;
            permission.UpdatedDate = DateTime.Now;
            _context.SaveChanges();

            var permissionList = _context.UserTypePermissions.Where(w => w.PermissionID == id).ToList();
            foreach (var itemPermissionType in permissionList)
            {
                itemPermissionType.IsActive = false;
                _context.SaveChanges();
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            permission.IsActive = true;
            permission.UpdatedDate = DateTime.Now;

            (
             from utp in _context.UserTypePermissions
             where utp.PermissionID == id
             select utp
             ).ToList().ForEach(x => x.IsActive = true );

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PermissionExists(int id)
        {
            return _context.Permissions.Any(e => e.PermissionID == id);
        }
    }
}
