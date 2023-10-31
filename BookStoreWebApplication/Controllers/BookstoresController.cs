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
    public class BookstoresController : Controller
    {
        private readonly DbbookStoreContext _context;

        public BookstoresController(DbbookStoreContext context)
        {
            _context = context;
        }

        // GET: Bookstores
        public async Task<IActionResult> Index()
        {
              return View(await _context.Bookstores.ToListAsync());
        }

        // GET: Bookstores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookstores == null)
            {
                return NotFound();
            }

            var bookstore = await _context.Bookstores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookstore == null)
            {
                return NotFound();
            }

            return View(bookstore);
        }

        // GET: Bookstores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bookstores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,Address")] Bookstore bookstore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookstore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookstore);
        }

        // GET: Bookstores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookstores == null)
            {
                return NotFound();
            }

            var bookstore = await _context.Bookstores.FindAsync(id);
            if (bookstore == null)
            {
                return NotFound();
            }
            return View(bookstore);
        }

        // POST: Bookstores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Address")] Bookstore bookstore)
        {
            if (id != bookstore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookstore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookstoreExists(bookstore.Id))
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
            return View(bookstore);
        }

        // GET: Bookstores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookstores == null)
            {
                return NotFound();
            }

            var bookstore = await _context.Bookstores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookstore == null)
            {
                return NotFound();
            }

            return View(bookstore);
        }

        public async Task<IActionResult> ShowAllWorkers(int? id)
        {
            if (id == null || _context.Bookstores == null)
            {
                return NotFound();
            }

            var bookstore = await _context.Bookstores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookstore == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Workers", new { BookstoreId = bookstore.Id });
            return RedirectToAction(nameof(Index));
        }

        // POST: Bookstores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookstores == null)
            {
                return Problem("Entity set 'DbbookStoreContext.Bookstores'  is null.");
            }
            var bookstore = await _context.Bookstores.FindAsync(id);
            if (bookstore != null)
            {
                _context.Bookstores.Remove(bookstore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookstoreExists(int id)
        {
          return _context.Bookstores.Any(e => e.Id == id);
        }
    }
}
