using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStoreWebApplication.Models;

namespace BookStoreWebApplication.Controllers
{
    public class WorkersController : Controller
    {
        private readonly DbbookStoreContext _context;

        public WorkersController(DbbookStoreContext context)
        {
            _context = context;
        }

        // GET: Workers
        //public async Task<IActionResult> Index()
        //{
        //    var dbbookStoreContext = _context.Workers.Include(w => w.Bookstore);
        //    return View(await dbbookStoreContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(int? bookstoreId, string? bookstoreAddress)
        {
            if (bookstoreId == null)
            {
                var workers = _context.Workers.Include(w => w.Bookstore);
				return View(await workers.ToListAsync());
            }
			var bookstore = _context.Bookstores.FirstOrDefault(b => b.Id == bookstoreId);
			var workersByBookstore = _context.Workers.Where(w => w.BookstoreId == bookstoreId).Include(w => w.Bookstore);
			ViewBag.BookstoreId = bookstore.Id;
			ViewBag.BookingstoreAddress = bookstore.FullAddress;
            ViewBag.NeedsToShowBookstoreAddress = false;

			return View(await workersByBookstore.ToListAsync());
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(w => w.Bookstore)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            ViewData["BookstoreId"] = new SelectList(_context.Bookstores, "Id", "FullAddress");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BirthDate,Address,Email,BookstoreId")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["BookstoreId"] = new SelectList(_context.Bookstores, "Id", "FullAddress", worker.BookstoreId);
            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            ViewData["BookstoreId"] = _context.Bookstores.ToList();
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string BookstoreAddress, [Bind("Id,Name,BirthDate,Email,Address")] Worker worker)
        {
            var bookstore = _context.Bookstores.FirstOrDefault(store => store.Address == BookstoreAddress);
            if (id != worker.Id)
            {
                return NotFound();
            }
            if(bookstore != null)
            {
                worker.BookstoreId = bookstore.Id;
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(worker);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!WorkerExists(worker.Id))
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
            }
            ViewData["BookstoreId"] = _context.Bookstores.ToList();
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(w => w.Bookstore)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workers == null)
            {
                return Problem("Entity set 'DbbookStoreContext.Workers' is null.");
            }
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null)
            {
                _context.Workers.Remove(worker);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
          return (_context.Workers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
