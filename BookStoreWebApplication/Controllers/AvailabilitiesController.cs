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
    public class AvailabilitiesController : Controller
    {
        private readonly DbbookStoreContext _context;

        public AvailabilitiesController(DbbookStoreContext context)
        {
            _context = context;
        }

        // GET: Availabilities
        public async Task<IActionResult> Index()
        {
            var dbbookStoreContext = _context.Availabilities
                .Include(a => a.Book).ThenInclude(b => b.AuthorsBooks).ThenInclude(b => b.Author)
                .Include(a => a.Bookstore);
            return View(await dbbookStoreContext.ToListAsync());
        }

        // GET: Availabilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Availabilities == null)
            {
                return NotFound();
            }

            var availability = await _context.Availabilities
                .Include(a => a.Book).ThenInclude(b => b.AuthorsBooks).ThenInclude(a => a.Author)
                .Include(a => a.Bookstore)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (availability == null)
            {
                return NotFound();
            }

            return View(availability);
        }

        // GET: Availabilities/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books.Include(b => b.AuthorsBooks).ThenInclude(a => a.Author), "Id", "FullInfo");
            ViewData["BookstoreId"] = new SelectList(_context.Bookstores, "Id", "FullAddress");
            return View();
        }

        // POST: Availabilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,BookstoreId,Count")] Availability availability)
        {
            if (ModelState.IsValid)
            {
                var oldAvailability = _context.Availabilities.FirstOrDefault(a => a.BookId == availability.BookId && a.BookstoreId == availability.BookstoreId);
                if (oldAvailability != null)
                {
                    oldAvailability.Count += availability.Count;
                    _context.Update(oldAvailability);
                }
                else
                {
                    _context.Add(availability);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "CoverType", availability.BookId);
            ViewData["BookstoreId"] = new SelectList(_context.Bookstores, "Id", "Id", availability.BookstoreId);
            return View(availability);
        }

        // GET: Availabilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Availabilities == null)
            {
                return NotFound();
            }

            var availability = await _context.Availabilities.FindAsync(id);
            if (availability == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = _context.Books.ToList();
            ViewData["BookstoreId"] = _context.Bookstores.ToList();
            return View(availability);
        }

        // POST: Availabilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string BookstoreAddress, string BookName, [Bind("Id,BookId,BookstoreId,Count")] Availability availability)
        {
            var bookstore = _context.Bookstores.FirstOrDefault(store => store.Address == BookstoreAddress);
            var book = _context.Books.FirstOrDefault(book => book.Name == BookName);
            if (id != availability.Id)
            {
                return NotFound();
            }
            if (bookstore != null && book != null)
            {
                availability.BookstoreId = bookstore.Id;
                availability.BookId = book.Id;
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(availability);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AvailabilityExists(availability.Id))
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

            ViewData["BookId"] = _context.Books.ToList();
            ViewData["BookstoreId"] = _context.Bookstores.ToList();
            return View(availability);
        }

        // GET: Availabilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Availabilities == null)
            {
                return NotFound();
            }

            var availability = await _context.Availabilities
                .Include(a => a.Book)
                .Include(a => a.Bookstore)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (availability == null)
            {
                return NotFound();
            }

            return View(availability);
        }

        // POST: Availabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Availabilities == null)
            {
                return Problem("Entity set 'DbbookStoreContext.Availabilities'  is null.");
            }
            var availability = await _context.Availabilities.FindAsync(id);
            if (availability != null)
            {
                _context.Availabilities.Remove(availability);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvailabilityExists(int id)
        {
          return _context.Availabilities.Any(e => e.Id == id);
        }
    }
}
