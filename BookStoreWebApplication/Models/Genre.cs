using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApplication.Models;

public partial class Genre
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    public string Name { get; set; } = null!;

    public virtual ICollection<AuthorsGenre> AuthorsGenres { get; } = new List<AuthorsGenre>();

    public virtual ICollection<BooksGenre> BooksGenres { get; } = new List<BooksGenre>();
}
