using System;
using System.Collections.Generic;

namespace amarris_kitchen_backend.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? OrderId { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Trn { get; set; }

    public decimal AmountPaid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Vat { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order? Order { get; set; }
}
