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
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class NewsController : AbstractPolicyController
    {
        public NewsController(VetClinicContext context) : base(context) { }

        // GET: Admin
        public async Task<IActionResult> Index(string searchString)
        {
            ViewBag.Tite = "Wiadomosci Nieodczytane";
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();
            
            ViewBag.NewMessage = _context.News
            .Include(i => i.NewsReadeds)
            .Where(w => w.UserTypeID == usertypeid &&
                w.SenderUser.UserID != userid &&
                w.StartDate <= DateTime.Now &&
                w.ExpirationDate >= DateTime.Now &&
                w.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
            .Count();
            //poprawione
            var vetClinicContext = _context.News
                .Include(n => n.NewsUpdatedUser).Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser).Include(n => n.NewsReadeds)
                .Where(n => n.UserTypeID == usertypeid && n.IsActive == true && n.StartDate <= DateTime.Now &&
                n.ExpirationDate >= DateTime.Now && n.SenderUser.UserID != userid);

            ViewData["CurrentFilter"] = searchString;

            if(!String.IsNullOrEmpty(searchString))
            {
                var searchResult = vetClinicContext.Where(w =>
                w.SenderUser.FirstName.Contains(searchString) ||
                w.SenderUser.LastName.Contains(searchString) ||
                w.Title.Contains(searchString) ||
                w.Message.Contains(searchString)
                );
                return View(await searchResult.OrderByDescending(u => u.UpdatedDate).ToListAsync());
            }

            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }        
        
        // GET: Moje
        public async Task<IActionResult> Own(string searchString)
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            ViewBag.NewMessage = _context.News
            .Include(i => i.NewsReadeds)
            .Where(w => w.UserTypeID == usertypeid &&
                w.SenderUser.UserID != userid &&
                w.StartDate <= DateTime.Now &&
                w.ExpirationDate >= DateTime.Now &&
                w.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
            .Count();

            var vetClinicContext = _context.News.Include(n => n.NewsUpdatedUser).Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser)
            .Where(n => n.SenderUser.UserID == userid && n.IsActive == true && n.StartDate <= DateTime.Now && n.ExpirationDate >= DateTime.Now);

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchResult = vetClinicContext.Where(w =>
                w.SenderUser.FirstName.Contains(searchString) ||
                w.SenderUser.LastName.Contains(searchString) ||
                w.Title.Contains(searchString) ||
                w.Message.Contains(searchString) 
                );
                return View(await searchResult.OrderByDescending(u => u.UpdatedDate).ToListAsync());
            }

            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }


        //Przeczytane / nie ogladam wlasnych
        public async Task<IActionResult> Readed()
        {
            ViewBag.Tite = "Wiadomosci przeczytane";
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            var vetClinicContext = _context.News.Include(n => n.NewsUpdatedUser).Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser)
                .Where(n => n.UserTypeID == usertypeid && n.IsActive == true && n.SenderUser.UserID != userid);

            return View("Index",await vetClinicContext.OrderBy(u => u.UpdatedDate).ToListAsync());
        }
        // GET: News/Create
        public IActionResult Create()
        {

            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            ViewBag.NewMessage = _context.News
            .Include(i => i.NewsReadeds)
            .Where(w => w.UserTypeID == usertypeid &&
                w.SenderUser.UserID != userid &&
                w.StartDate <= DateTime.Now &&
                w.ExpirationDate >= DateTime.Now &&
                w.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
            .Count();

            ViewData["UserTypeID"] = new SelectList(_context.UserTypes.Where(ut => ut.IsActive == true && 
            ut.Name != "No7818Permissions" && ut.Name != "Klienci" ) , "UserTypeID", "Name");

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
                news.UpdatedDate = news.AddedDate;
                news.IsActive = true;

                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction("Own","News");
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

            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            ViewBag.NewMessage = _context.News
            .Include(i => i.NewsReadeds)
            .Where(w => w.UserTypeID == usertypeid &&
                w.SenderUser.UserID != userid &&
                w.StartDate <= DateTime.Now &&
                w.ExpirationDate >= DateTime.Now &&
                w.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
            .Count();

            ViewData["UserTypeID"] = new SelectList(_context.UserTypes.Where(ut => ut.IsActive == true), "UserTypeID", "Name", news.UserTypeID);

            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsID,UserID,UserTypeID,Title,Message,IsReaded,IsActive,StartDate,ExpirationDate,AddedDate,UpdatedDate,UpdatedUserID")] News news)
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
        public async Task<IActionResult> OwnDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            ViewBag.NewMessage = _context.News
            .Include(i => i.NewsReadeds)
            .Where(w => w.UserTypeID == usertypeid &&
                w.SenderUser.UserID != userid &&
                w.StartDate <= DateTime.Now &&
                w.ExpirationDate >= DateTime.Now &&
                w.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
            .Count();


            List<int> userReaded = _context.NewsReadeds
            .Where(w => w.NewsID == id && w.UserId != userid).Select(s=> s.UserId).ToList();

            List<User> userList = new List<User>();
            foreach (var item in userReaded)
            {
                userList.Add(_context.Users.FirstOrDefault(f => f.UserID == item));
            }

            ViewBag.UserList = userList;
            var UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
            var newsItem = await _context.News.FindAsync(id);
            await _context.SaveChangesAsync();
            ViewBag.LoggedUserID = UserId;
            
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

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            ViewBag.NewMessage = _context.News
            .Include(i => i.NewsReadeds)
            .Where(w => w.UserTypeID == usertypeid &&
                w.SenderUser.UserID != userid &&
                w.StartDate <= DateTime.Now &&
                w.ExpirationDate >= DateTime.Now &&
                w.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
            .Count();

            var UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));

            //zmiana na przeczytana
            if (_context.NewsReadeds.FirstOrDefault(f => f.NewsID == id && f.UserId == UserId) == null)
            {
                _context.NewsReadeds.Add(
                    new NewsReaded
                    {
                        UserId = UserId,
                        NewsID = Convert.ToInt32(id)
                    }
                    );
            }
            
            var newsItem = await _context.News.FindAsync(id);
            await _context.SaveChangesAsync();
            ViewBag.LoggedUserID = UserId;
            
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes.Where(s => s.IsActive == true), "UserTypeID", "Name");
            ViewData["SenderUser"] = _context.News.Where(u=>u.UserID == id);
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
