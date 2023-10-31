using BookStoreWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWebApplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChartController : ControllerBase
	{
		private readonly DbbookStoreContext _context;

		public ChartController(DbbookStoreContext context)
		{
			_context = context;
		}

		[HttpGet("OrdersToBookstores")]
		public async Task<JsonResult> JsonData()
		{
			var bookstores = await _context.Bookstores.ToListAsync();
			var orders = await _context.Orders.Include(o => o.Seller).ToListAsync();
			var bookstoresResult = new List<object>();
			bookstoresResult.Add(new object[] { "Магазин", "Кількість продаж" });
			foreach (var bookstore in bookstores)
			{
				var bookstoreOrders = orders.Where(o => o.Seller.BookstoreId == bookstore.Id).ToList();

				bookstoresResult.Add(new object[] { bookstore.FullAddress, bookstoreOrders.Count });
			}

			return new JsonResult(bookstoresResult);
		}

        [HttpGet("GenresToBooks")]
        public async Task<JsonResult> GenresToBooksStatistics()
        {
			var genres = await _context.Genres.ToListAsync();
			var books = await _context.Books.Include(b => b.BooksGenres).ToListAsync();
            var genresResult = new List<object>();
            genresResult.Add(new object[] { "Жанр", "Кількість книг" });
            foreach (var genre in genres)
            {
				var genreBooks = books.Where(b => b.BooksGenres.Select(b => b.GenreId).Contains(genre.Id));

                genresResult.Add(new object[] { genre.Name, genreBooks.Count() });
            }

            return new JsonResult(genresResult);
        }
    }
}
