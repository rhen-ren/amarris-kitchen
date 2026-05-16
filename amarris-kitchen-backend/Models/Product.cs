using System;
using System.Collections.Generic;

namespace amarris_kitchen_backend.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public string? ImageUrl { get; set; }

    public int CategoryId { get; set; }

    public int AdminId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ComboItem> ComboItemCombos { get; set; } = new List<ComboItem>();

    public virtual ICollection<ComboItem> ComboItemProducts { get; set; } = new List<ComboItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
