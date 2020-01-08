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
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class NewsController : AbstractPolicyController
    {
        public NewsController(VetClinicContext context) : base(context) { }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            var vetClinicContext = _context.News.Include(n => n.NewsUpdatedUser).Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser)
                .Where(n => n.UserTypeID == usertypeid && n.IsActive == true && n.IsReaded == false && n.StartDate <= DateTime.Now && n.ExpirationDate >= DateTime.Now);

            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }
        
        public async Task<IActionResult> Readed()
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            var vetClinicContext = _context.News.Include(n => n.NewsUpdatedUser).Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser)
                .Where(n => n.UserTypeID == usertypeid && n.IsActive == true && n.IsReaded == true);

            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }
        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes.Where(ut => ut.IsActive == true), "UserTypeID", "Name");

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

            ViewData["UserTypeID"] = new SelectList(_context.UserTypes.Where(ut => ut.IsActive == true), "UserTypeID", "Name", news.UserTypeID);

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
                    news.IsActive = true;

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

            return View(news);
        }
        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.LoggedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes.Where(s => s.IsActive == true), "UserTypeID", "Name");
            var news = await _context.News
                .Include(n => n.SenderUser)
                .Include(n => n.NewsUpdatedUser)
                .Include(n => n.ReceiverUserTypes)
                .FirstOrDefaultAsync(m => m.NewsID == id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }


        // POST: News/Delete/5
        [HttpPost, ActionName("Read")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            news.UpdatedDate = DateTime.Now;
            news.IsReaded = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            news.UpdatedDate = DateTime.Now;
            news.IsActive = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            news.IsReaded = false;
            news.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsID == id);
        }
    }
}
