using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.PortalWWW.Controllers
{
    public class UsersController : Controller
    {
        private readonly VetClinicContext _context;

        public UsersController(VetClinicContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var UserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
            User result = _context.Users.FirstOrDefault(u => u.UserID == UserID);
            ViewBag.Login = result.Login;
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Index(List<Microsoft.AspNetCore.Http.IFormFile> Image,string FirstName, string LastName, string Phone, string Email, string Street, string HouseNumber, string ApartmentNumber, string PostalCode, string City, string CardNumber)
        {
            var UserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
            User result = await _context.Users.FirstOrDefaultAsync(u => u.UserID == UserID);

            if (ModelState.IsValid)
            {
                foreach (var item in Image)
                {
                    using (var stream = new MemoryStream())
                    {
                        if (item != null)
                        {
                            await item.CopyToAsync(stream);
                            result.Image = stream.ToArray();
                        }
                    }
                }
                    result.FirstName = FirstName;
                    result.LastName = LastName;
                    result.Phone = Phone;
                    result.Email = Email;
                    result.Street = Street;
                    result.HouseNumber = HouseNumber;
                    result.ApartmentNumber = ApartmentNumber;
                    result.PostalCode = PostalCode;
                    result.City = City;
                    result.CardNumber = CardNumber;

               
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "ClientPanel");
            }

            return View();
        }
        public IActionResult Return()
        {
            
            return RedirectToAction("Index", "ClientPanel");
        }
        

    }
}