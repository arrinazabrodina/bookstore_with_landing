using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApplication.Models;

public partial class Author
{
    public int Id { get; set; }

    [Display(Name = "ПІБ")]
    public string Name { get; set; } = null!;

    [Display(Name = "Дата народження")]
    public DateTime BirthDate { get; set; }

    [Display(Name = "Коротка біографія")]
    public string? ShortBiography { get; set; }

    public virtual ICollection<AuthorsBook> AuthorsBooks { get; } = new List<AuthorsBook>();

    public virtual ICollection<AuthorsGenre> AuthorsGenres { get; } = new List<AuthorsGenre>();

    [Display(Name = "Жанри")]
    public string Genres
    {
        get
        {
            string genres = "";

            foreach (var genre in AuthorsGenres)
            {
                genres += $"{genre.Genre.Name}; ";
            }

            return genres;
        }
    }
}
