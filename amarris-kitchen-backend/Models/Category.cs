using System;
using System.Collections.Generic;

namespace amarris_kitchen_backend.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? AdminId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
