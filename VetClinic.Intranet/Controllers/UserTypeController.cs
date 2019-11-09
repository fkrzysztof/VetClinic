using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.HelpersClass;

namespace VetClinic.Intranet.Controllers
{
    public class UserTypeController : Controller
    {
        private readonly VetClinicContext _context;

        public UserTypeController(VetClinicContext context)
        {
            _context = context;
        }

        //widok przdstawia uprawnienia - chyba nie potrzebny odrazu mozna dac edycje, lub przerobic na partial
        public async Task<IActionResult> Permissions(int id)
        {
            var vetClinicContext = _context.UserTypePermissions.Include(u => u.Permission).Include(u => u.UserType)
                .Include(u => u.UserTypePermissionAddedUser).Include(u => u.UserTypePermissionUpdatedUser)
                .Where(p => p.UserTypeID == id);
            return View(await vetClinicContext.ToListAsync());
        }


        //public async Task<IActionResult> AddNewPermisssonsToType()
        public string AddNewPermisssonsToType(string name)
        {
            return "jest ok" + name;
        }


        // POST: UserTypePermissions/Delete/5
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermissionsDelete(int id)
        {
            var userTypePermission = await _context.UserTypePermissions.FindAsync(id);
            _context.UserTypePermissions.Remove(userTypePermission);
            await _context.SaveChangesAsync();

            //lub zmienic na active = 0 ?

            return RedirectToAction(nameof(Index));
        }





        // GET: UserTypes
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.UserTypes.Include(u => u.UserTypeAddedUser).Include(u => u.UserTypeUpdatedUser);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: UserTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes
                .Include(u => u.UserTypeAddedUser)
                .Include(u => u.UserTypeUpdatedUser)
                .FirstOrDefaultAsync(m => m.UserTypeID == id);            
            
            //var userToAdd = await _context.Users.FirstOrDefaultAsync(m => m.UserTypeID == id);

            //List<UserType> list_userType = new List<UserType>();
            //list_userType.Add(userType);
            //List<User> list_user = new List<User>();
            //list_user.Add(userToAdd);

            
            //UsersAndUserType usersAndUser = new UsersAndUserType();
            //usersAndUser.User = _context.Users.Where(u => u.UserTypeID == id).ToList();
            //usersAndUser.UserType = _context.UserTypes.Where(t => t.UserTypeID == id).ToList();

            if (userType == null)
            {
                return NotFound();
            }
            ViewBag.UsersInType = _context.Users.Where(u => u.UserTypeID == id);

            //return View(
            //    new UsersAndUserType
            //    {

            //        UserTypeList = list_userType,
            //        UserList =list_user
            //    }
            //    );

            return View(userType);
            //return View(usersAndUser);
        }

        // GET: UserTypes/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.PermissionsList = _context.Permissions.Include(p => p.PermissionAddedUser).Include(p => p.PermissionUpdatedUser);
            //ViewBag.PermissionsList = _context.UserTypePermissions;


            return View();



            //Dane beda pobierane automatycznie z loginu! ********************************

            //ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            //ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            //return View();
        }

        // POST: UserTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //calosc nie bedzie pobierana z form
       //  public async Task<IActionResult> Create([Bind("UserTypeID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] UserType userType)
        //public async Task<IActionResult> Create([Bind("UserTypeID,Name,Description,IsActive")] UserType userType)
        public async Task<IActionResult> Create(HelpersCreate hc)
        {

            

            //if (ModelState.IsValid)
            //{
            //    userType.AddedDate = DateTime.Now;
            //    userType.AddedUserID = null;
            //    _context.Add(userType);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //jest ok


            //to bedzie z automau..
            //ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.AddedUserID);
            //ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.UpdatedUserID);

            return View();
            
            //return View(userType);
        }

        // GET: UserTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.UpdatedUserID);
            return View(userType);
        }

        // POST: UserTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserTypeID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] UserType userType)
        {
            if (id != userType.UserTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTypeExists(userType.UserTypeID))
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.UpdatedUserID);
            return View(userType);
        }

        // GET: UserTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes
                .Include(u => u.UserTypeAddedUser)
                .Include(u => u.UserTypeUpdatedUser)
                .FirstOrDefaultAsync(m => m.UserTypeID == id);

           
            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // POST: UserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userType = await _context.UserTypes.FindAsync(id);
            _context.UserTypes.Remove(userType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTypeExists(int id)
        {
            return _context.UserTypes.Any(e => e.UserTypeID == id);
        }
    }
}
