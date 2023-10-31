using System;
using System.Collections.Generic;

namespace BookStoreWebApplication.Models;

public partial class BooksGenre
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int GenreId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public string? GenreName
    {
        get
        {
            if (Genre != null)
            {
                return Genre.Name;
            }
            return null;
        }
    }
}
