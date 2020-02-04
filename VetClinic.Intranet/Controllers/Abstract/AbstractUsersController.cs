using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Helpers;
using VetClinic.Intranet.Controllers.Abstract;
using VetClinic.Intranet.Policy;

namespace VetClinic.Intranet.Controllers.Abastract
{
    public abstract class AbstractUsersController : AbstractPolicyController
    {
        protected int _userTypeId;
        SmtpConfiguration SmtpConf = new SmtpConfiguration(); // konfuguracja smtp do wysyłki maila
        public AbstractUsersController(VetClinicContext context, int UserTypeId) : base(context)
        {
            _userTypeId = UserTypeId;
        }

        // GET: Admin
        public virtual async Task<IActionResult> Index(string searchString)
        {
            //UserPolicy policy = new UserPolicy(_context, HttpContext, this.ControllerContext.RouteData);
            //if (await policy.hasNoAccess()) return await policy.RedirectUser();
            //ViewData = policy.PopulateViewData(ViewData);

            ViewData["CurrentFilter"] = searchString;

            var vetClinicContext = _context.Users.Include(u => u.UserType).Where(u => u.UserType.UserTypeID == _userTypeId);

            if (!String.IsNullOrEmpty(searchString))
            {
                vetClinicContext = (from user in vetClinicContext
                                    where user.Login.Contains(searchString)
                                    || user.LastName.Contains(searchString)
                                    || user.Email.Contains(searchString)
                                    || user.City.Contains(searchString)
                                    select user)
                                            .Include(m => m.UserType);
            }

            ViewBag.RouteData = this.ControllerContext.RouteData.Values["controller"].ToString();

            return View("../AbstractUsers/Index", await vetClinicContext.OrderByDescending(u => u.IsActive).ThenByDescending(u => u.UpdatedDate).ToListAsync());
        }

        // GET: Admin/Create
        public virtual async Task<IActionResult> Create()
        {
            //UserPolicy policy = new UserPolicy(_context, HttpContext, this.ControllerContext.RouteData);
            //if (await policy.hasNoAccess()) return await policy.RedirectUser();
            //ViewData = policy.PopulateViewData(ViewData);

            return View("../AbstractUsers/Create");
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, List<Microsoft.AspNetCore.Http.IFormFile> Image)
        {
            var rnd = new Random();

            string email;
            string login;

            email = user.Email;
            login = user.Login;

           

            if (ModelState.IsValid)
            {
                foreach (var item in Image)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        user.Image = stream.ToArray();
                    }
                }
                var usersLogin =
              (from users in _context.Users
               select users.Login
                  ).ToList().ConvertAll(d => d.ToLower());

                var usersEmail =
                (from users in _context.Users
                 select users.Email
                    ).ToList().ConvertAll(d => d.ToLower());

                // check email
                if (usersEmail.Contains(email.ToLower()))
                {
                    ModelState.AddModelError("email", "Konto z takim adresem email już istnieje!");
                    return View("../AbstractUsers/Create");
                }

                //check login
                for (int i = 0; i < usersLogin.Count(); i++)
                {
                    if (usersLogin.Contains(login.ToLower()))
                    {
                        login += rnd.Next(0, 9);
                        ModelState.AddModelError("login", "Taki login już istnieje! Wybierz inny: " + login.ToLower());
                        return View("../AbstractUsers/Create");
                    }
                    else
                    {
                        break;
                    }
                }


                user.Login = login.ToLower();
                user.Email = email.ToLower();

                user.AddedDate = DateTime.Now;
                user.UpdatedDate = user.AddedDate;
                user.IsActive = true;
                var passwordUser = user.Password;

                user.UserTypeID = _userTypeId;
                user.Password = HashPassword.GetMd5Hash(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                SmtpConf.MessageTo = user.Email;
                SmtpConf.MessageText = user.FirstName + " witamy w zespole <br>"
                                    + "<br> "
                                    + "Twój login: " + user.Login + "<br>" + "Hasło: " + passwordUser + "<br>"
                                    + "<br>"
                                    + "Pozdrawiamy <br>"
                                    + "Zespół VetClinic";


                SmtpConf.MessageSubject = "Potwierdzenie dokonanej rejestracji";
                SmtpConf.send();

                return RedirectToAction(nameof(Index));
            }

            return View("../AbstractUsers/Create");
        }

        // GET: Admin/Edit/5
        public virtual async Task<IActionResult> Edit(int? id)
        {
            //UserPolicy policy = new UserPolicy(_context, HttpContext, this.ControllerContext.RouteData);
            //if (await policy.hasNoAccess()) return await policy.RedirectUser();
            //ViewData = policy.PopulateViewData(ViewData);

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.IdUser = user.UserID;
            ViewBag.Type = user.UserTypeID;
            ViewBag.MedicalSpecializations = _context.MedicalSpecializations.Include(m => m.MedicalSpecializationUser).Include(m => m.Specialization).Where(w => w.UserID == id);
            return View("../AbstractUsers/Edit", user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserTypeID,FirstName,LastName,HouseNumber,ApartmentNumber,Street,City,PostalCode,Email,Login,Password,Phone,Photo,Image,CardNumber,IsActive,Description,AddedDate")] User user, List<Microsoft.AspNetCore.Http.IFormFile> Image)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in Image)
                    {
                        using (var stream = new MemoryStream())
                        {
                            if (item != null)
                            {
                                await item.CopyToAsync(stream);
                                user.Image = stream.ToArray();
                            }
                        }
                    }
                    user.UpdatedDate = DateTime.Now;
                    user.IsActive = true;
                    user.AuthorizationEmail = true;
                    user.UserTypeID = _userTypeId;
                    if (user.Password.Length == 8)
                    {
                        var passwordUser = user.Password;
                        user.Password = HashPassword.GetMd5Hash(user.Password);
                        SmtpConf.MessageTo = user.Email;
                        SmtpConf.MessageText = user.FirstName + " Twoje nowe hasło: " + passwordUser;
                        SmtpConf.MessageSubject = "Potwierdzenie zmiany hasła";
                        SmtpConf.send();
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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

            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.IsActive = false;
            user.LoginAttempt = 5;
            user.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.IsActive = true;
            user.LoginAttempt = 0;
            user.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(string[] ids)
        {
            int[] getid = null;
            if (ids != null)
            {
                getid = new int[ids.Length];
                int j = 0;
                foreach (string i in ids)
                {
                    int.TryParse(i, out getid[j++]);
                }
            }

            List<User> getusrids = new List<User>();
            getusrids = _context.Users.Where(x => getid.Contains(x.UserID)).ToList();
            foreach (var s in getusrids)
            {
                _context.Users.Update(s);
                s.IsActive = false;
                s.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult VerifyLogin(string login, int UserID)
        {
            var test = _context.Users.FirstOrDefault(a => a.Login == login);
            if (UserID == 0 && test != null)
            {
                return Json($"Login {login} jest już zajęty.");
            }
            return Json(true);
        }
    }
}
