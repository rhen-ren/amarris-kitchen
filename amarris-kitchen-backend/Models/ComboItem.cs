using System;
using System.Collections.Generic;

namespace amarris_kitchen_backend.Models;

public partial class ComboItem
{
    public int ComboId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Combo { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
