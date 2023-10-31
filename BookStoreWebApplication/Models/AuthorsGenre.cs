using System;
using System.Collections.Generic;

namespace BookStoreWebApplication.Models;

public partial class AuthorsGenre
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public int GenreId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
