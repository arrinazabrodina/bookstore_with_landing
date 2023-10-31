using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApplication.Models;

public partial class Book
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "Рік видавництва")]
    public short PublicationYear { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "Тип обкладинки")]
    public string? CoverType { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "Ціна")]
    public double Price { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "Назва")]
    public string Name { get; set; }

    [Display(Name = "Жанри")]
    public virtual string Genres
    {
        get
        {
            return string.Join(", ", BooksGenres.Select(b => b.Genre.Name));
        }
    }

	[Display(Name = "Автори")]
	public virtual string Authors
	{
		get
		{
			return string.Join(", ", AuthorsBooks.Select(a => a.Author.Name));
		}
	}

    public string FullInfo
    {
        get
        {
            return $"{Name}, {Authors}";
        }
    }

	public virtual ICollection<AuthorsBook> AuthorsBooks { get; } = new List<AuthorsBook>();

    public virtual ICollection<Availability> Availabilities { get; } = new List<Availability>();

    public virtual ICollection<BooksGenre> BooksGenres { get; } = new List<BooksGenre>();


    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "Жанри")]
    public virtual ICollection<int> GenreIds { get; } = new List<int>();

    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "Автори")]
	public virtual ICollection<int> AuthorIds {  get; } = new List<int>();

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
}
