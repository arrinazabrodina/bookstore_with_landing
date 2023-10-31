using System;
using System.Collections.Generic;

namespace BookStoreWebApplication.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int Count { get; set; }

    public int BuyId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Buy { get; set; } = null!;
}
