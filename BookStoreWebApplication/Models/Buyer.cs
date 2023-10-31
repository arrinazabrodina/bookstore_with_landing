using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApplication.Models;

public partial class Buyer
{
    public int Id { get; set; }

    [Display(Name = "ПІБ")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    public string Name { get; set; } = null!;

    [Display(Name = "Номер телефону")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Адреса")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    public string? Address { get; set; }

    [Display(Name = "Електронна пошта")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    public string? Email { get; set; }

    [Display(Name = "Дата народження")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Це поле є обов'язковим")]
    public DateTime? BirthDate { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
