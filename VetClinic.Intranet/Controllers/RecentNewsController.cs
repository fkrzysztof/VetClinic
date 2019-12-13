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
using VetClinic.Data.Data.CMS;

namespace VetClinic.Intranet.Controllers
{
    public class RecentNewsController : Controller
    {
        private readonly VetClinicContext _context;

        public RecentNewsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: RecentNews
        public async Task<IActionResult> Index()
        {
            return View(await _context.RecentNews.ToListAsync());
        }

        // GET: RecentNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recentNews = await _context.RecentNews
                .FirstOrDefaultAsync(m => m.RecentNewsID == id);
            if (recentNews == null)
            {
                return NotFound();
            }

            return View(recentNews);
        }

        // GET: RecentNews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecentNews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecentNewsID,LinkTitle,Title,Text,Position,Photo,Image,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] RecentNews recentNews, List<Microsoft.AspNetCore.Http.IFormFile> Image)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in Image)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        recentNews.Image = stream.ToArray();
                    }
                }
                recentNews.AddedDate = DateTime.Now;
                recentNews.IsActive = true;
                _context.Add(recentNews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recentNews);
        }

        // GET: RecentNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recentNews = await _context.RecentNews.FindAsync(id);
            if (recentNews == null)
            {
                return NotFound();
            }
            return View(recentNews);
        }

        // POST: RecentNews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecentNewsID,LinkTitle,Title,Text,Position,Photo,Image,IsActive,UpdatedDate,AddedUserID,UpdatedUserID")] RecentNews recentNews, List<Microsoft.AspNetCore.Http.IFormFile> Image)
        {
            if (id != recentNews.RecentNewsID)
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
                                recentNews.Image = stream.ToArray();
                            }
                        }
                    }
                    recentNews.UpdatedDate = DateTime.Now;
                    _context.Update(recentNews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecentNewsExists(recentNews.RecentNewsID))
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
            return View(recentNews);
        }

        // GET: RecentNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recentNews = await _context.RecentNews
                .FirstOrDefaultAsync(m => m.RecentNewsID == id);
            if (recentNews == null)
            {
                return NotFound();
            }

            return View(recentNews);
        }

        // POST: RecentNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recentNews = await _context.RecentNews.FindAsync(id);
            _context.RecentNews.Remove(recentNews);
            recentNews.IsActive = false;
            recentNews.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecentNewsExists(int id)
        {
            return _context.RecentNews.Any(e => e.RecentNewsID == id);
        }
    }
}
