using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreWebApplication.Models;

public partial class Order
{
    public int Id { get; set; }

    public int BuyerId { get; set; }

    [Display(Name = "Дата замовлення")]
    public DateTime Date { get; set; }

    public int SellerId { get; set; }

    [Display(Name = "Ціна, грн")]
    public double Price { get; set; }

    public string Title
    {
        get
        {
            return $"{Price}";
        }
    }

    [Display(Name = "Покупець")]
    public virtual Buyer Buyer { get; set; } = null!;

    [Display(Name = "Товари")]
    public string Items
    {
        get
        {
            return string.Join(", ", OrderItems.Select(o => $"{o.Book.Name} {o.Count}шт"));
        }
    }
    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    [Display(Name = "Продавець")]
    public virtual Worker Seller { get; set; } = null!;

    [NotMapped()]
    [Display(Name = "Книга")]
    public virtual int? CurrentBookId { get; set; }

    [NotMapped()]
    [Display(Name = "Кількість")]
    public virtual int? CurrentBookCount { get; set; }
}

//public class Select