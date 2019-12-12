using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Helpers;

namespace VetClinic.Intranet.Controllers
{
    public class NewsController : Controller
    {
        private readonly VetClinicContext _context;
        

        public NewsController(VetClinicContext context)
        {
            _context = context;
        }

        //// GET: News
        //public async Task<IActionResult> Index()
        //{
        //    var vetClinicContext = _context.News.Include(n => n.NewsUpdatedUser).Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser);
        //    return View(await vetClinicContext.ToListAsync());
        //}

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            var vetClinicContext = _context.News.Include(n => n.NewsUpdatedUser).Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser)
                .Where(n => n.UserTypeID == usertypeid && n.IsActive == true);

            return View(await vetClinicContext.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.NewsUpdatedUser)
                .Include(n => n.ReceiverUserTypes)
                .Include(n => n.SenderUser)
                .FirstOrDefaultAsync(m => m.NewsID == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name");
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsID,UserID,UserTypeID,Title,Message,IsReaded,IsActive,StartDate,ExpirationDate,AddedDate,UpdatedDate,UpdatedUserID")] News news)
        {
            if (ModelState.IsValid)
            {
                var LoggedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                news.UserID = LoggedUserID;
                news.AddedDate = DateTime.Now;
                news.IsActive = true;
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", news.UpdatedUserID);
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", news.UserTypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "City", news.UserID);
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", news.UpdatedUserID);
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", news.UserTypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "City", news.UserID);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsID,UserID,UserTypeID,Title,Message,IsReaded,IsActive,StartDate,ExpirationDate,UpdatedDate,UpdatedUserID")] News news)
        {
            if (id != news.NewsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    news.UpdatedDate = DateTime.Now;
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.NewsID))
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
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", news.UpdatedUserID);
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", news.UserTypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "City", news.UserID);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.NewsUpdatedUser)
                .Include(n => n.ReceiverUserTypes)
                .Include(n => n.SenderUser)
                .FirstOrDefaultAsync(m => m.NewsID == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsID == id);
        }
    }
}
