using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStoreWebApplication.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreWebApplication.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DbbookStoreContext _context;

        public OrdersController(DbbookStoreContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var dbbookStoreContext = _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.OrderItems).ThenInclude(o => o.Book);
            return View(await dbbookStoreContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.OrderItems).ThenInclude(o => o.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "Address");
            ViewData["SellerId"] = new SelectList(_context.Workers, "Id", "Address");
            var a = await _context.Availabilities.ToListAsync();

			//ViewBag.AvailabilityOfBooks = 
			//return View();
			return View(viewName: "SelectBookstore", await _context.Bookstores.ToListAsync());
		}

        public async Task<IActionResult> SelectWorker(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }
            var workers = await _context.Workers
                .Where(w => w.BookstoreId == id)
                .Include(w => w.Bookstore)
                .ToListAsync();

			return View(viewName: "SelectWorker", workers);
		}

        public async Task<IActionResult> SelectBuyer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.WorkerId = id!;

            return View(viewName: "SelectBuyer", await _context.Buyers.ToListAsync());
        }

        // GET: Orders/CreateBuyer
        public async Task<IActionResult> CreateBuyer(int? worker)
        {
            if (worker == null)
            {
                return NotFound();
            }
            ViewBag.WorkerId = worker!;

            return View(viewName: "CreateBuyer");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,Address,Email,BirthDate")] Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buyer);
                await _context.SaveChangesAsync();
                ViewBag.SellerName = buyer.Name;
                
                //return View(viewName: "SelectBooks", )
            }
            return View(buyer);
        }

        private class BooksCounts
        {
            public Book book { get; set; }
            public int count { get; set; }
            public BooksCounts(Book book, int count)
            {
                this.book = book;
                this.count = count;
            }
        }

        public async Task<IActionResult> SelectBooks([Bind("CurrentBookId,CurrentBookCount")]Order? order, int? buyerId, int? workerId, string previousItems)
        {
            if (buyerId == null || workerId == null)
            {
                return NotFound();
            }

            var seller = await _context.Workers.Include(w => w.Bookstore).FirstOrDefaultAsync(w => w.Id == workerId);
            var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.Id == buyerId);
            var books = await _context.Books.ToListAsync();
            var bookCounts = new List<BooksCounts>();
            if (previousItems != null)
            {
                var prevItems = previousItems.Split(';').Select(item => item.Split(',')).ToList();
                foreach (var item in prevItems)
                {
                    if (item.Length >= 2)
                    {
                        var currentBookId = Convert.ToInt32(item[0]);
                        var currentCount = Convert.ToInt32(item[1]);
                        var book = books.FirstOrDefault(b => currentBookId == b.Id);
                        if (book != null)
                        {
                            var existingBookCount = bookCounts.FirstOrDefault(b => b.book.Id == book.Id);
                            if (existingBookCount == null)
                            {
                                BooksCounts newBookCount = new(book, currentCount);
                                bookCounts.Add(newBookCount);
                            }
                            else
                            {
                                existingBookCount.count += currentCount;
                            }
                        }
                    }
                }
            }

            if (order != null && order.CurrentBookId != null && order.CurrentBookCount != null)
            {
                var book = books.FirstOrDefault(b => order.CurrentBookId == b.Id);
                if (book != null)
                {
                    var existingBookCount = bookCounts.FirstOrDefault(b => b.book.Id == book.Id);
                    if (existingBookCount == null)
                    {
                        BooksCounts newBookCount = new(book, (int)order.CurrentBookCount);
                        bookCounts.Add(newBookCount);
                    }
                    else
                    {
                        existingBookCount.count += (int)order.CurrentBookCount;
                    }
                }
            }

            var availabilities = await _context.Availabilities.ToListAsync();
            var itemsToRemove = new List<BooksCounts>();
            foreach (var bookCount in bookCounts)
            {
                var availability = availabilities.FirstOrDefault(a => a.BookId == bookCount.book.Id);
                if (availability != null)
                {
                    if (availability.Count < bookCount.count)
                    {
                        bookCount.count = availability.Count;
                    }
                }
                else
                {
                    itemsToRemove.Add(bookCount);
                }
            }

            foreach (var item in itemsToRemove)
            {
                bookCounts.Remove(item);
            }

            List<(string, int)> currentItemsViewData = new List<(string, int)>();
            string currentItemsData = "";
            double currentPrice = 0;

            foreach (var item in bookCounts)
            {
                currentItemsData += $"{item.book.Id},{item.count};";
                currentItemsViewData.Add(new(item.book.Name, item.count));
                currentPrice += item.book.Price * item.count;
            }

            var availableBooks = await _context.Availabilities
                .Where(a => a.BookstoreId == seller.BookstoreId && a.Count != 0)
                .Include(a => a.Book)
                .Select(a =>
                new
                {
                    BookId = a.Book.Id,
                    BookName = $"{a.Book.Name}, кількість {a.Count}"
                }).ToListAsync();
            var availableBooksList = new SelectList(availableBooks, "BookId", "BookName");
            var availableCounts = Enumerable.Range(1, 100).Select(x => new { Number = x });
            var countList = new SelectList(availableCounts, "Number", "Number");

            ViewBag.SellerName = seller.Name;
            ViewBag.SellerId= seller.Id;
            ViewBag.BuyerName = buyer.Name;
            ViewBag.BuyerId = buyer.Id;
            ViewBag.BookstoreFullAddress = seller.Bookstore.FullAddress;
            ViewBag.Price = string.Format("{0:f2}", currentPrice);
            ViewBag.ItemsData = currentItemsData;
            ViewBag.ItemsViewData = currentItemsViewData;
            ViewBag.AvailableBooks = availableBooksList;
            ViewBag.AvailableCounts = countList;

            return View(viewName: "SelectBook", new Order());
        }

        public async Task<IActionResult> FinishOrder(int? buyerId, int? workerId, string items)
        {
            if (buyerId == null || workerId == null)
            {
                return NotFound();
            }

            if (items.IsNullOrEmpty())
            {
                return RedirectToAction(nameof(Index));
            }

            var seller = await _context.Workers.Include(w => w.Bookstore).FirstOrDefaultAsync(w => w.Id == workerId);
            var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.Id == buyerId);
            var books = await _context.Books.ToListAsync();
            var bookCounts = new List<BooksCounts>();
            double price = 0;
            var order = new Order();
            order.BuyerId = (int)buyerId;
            order.SellerId = (int)workerId;
            order.Date = DateTime.Now;

            if (items != null)
            {
                var prevItems = items.Split(';').Select(item => item.Split(',')).ToList();
                foreach (var item in prevItems)
                {
                    if (item.Length >= 2)
                    {
                        var currentBookId = Convert.ToInt32(item[0]);
                        var currentCount = Convert.ToInt32(item[1]);
                        var book = books.FirstOrDefault(b => currentBookId == b.Id);
                        if (book != null)
                        {
                            var orderItem = new OrderItem();
                            orderItem.BookId = book.Id;
                            orderItem.Count = currentCount;
                            orderItem.Buy = order;
                            price += book.Price * currentCount;
                            _context.Add(orderItem);

                            var availability = await _context.Availabilities.FirstOrDefaultAsync(a => a.BookId == book.Id && a.BookstoreId == seller.BookstoreId);
                            availability.Count -= currentCount;
                            _context.Update(availability);
                        }
                    }
                }
            }

            order.Price = price;
            _context.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BuyerId,Date,SellerId,Price")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "Address", order.BuyerId);
            ViewData["SellerId"] = new SelectList(_context.Workers, "Id", "Address", order.SellerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.OrderItems).ThenInclude(o => o.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DbbookStoreContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                var orderItems = await _context.OrderItems.Where(o => o.BuyId == order.Id).ToListAsync();
                _context.OrderItems.RemoveRange(orderItems);
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return _context.Orders.Any(e => e.Id == id);
        }
    }
}
