using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreWebApplication.Models;

public partial class Worker
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "ПІБ")]
    public string Name { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "Дата народження")]
    public DateTime BirthDate { get; set; }

    [Required(AllowEmptyStrings=false, ErrorMessage="Це поле є обов'язковим")]
    [Display(Name = "Адреса проживання")]
    public string? Address { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    [Display(Name = "Електронна пошта")]
    public string? Email { get; set; }

    [Display(Name = "Магазин")]
    public int BookstoreId { get; set; }

    [Display(Name = "Магазин")]
    [JsonIgnore]
    public virtual Bookstore Bookstore { get; set; } = null!;

    [Display(Name = "Замовлення")]
    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
