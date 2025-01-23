using System;
using System.Collections.Generic;

namespace RepositoryAPI.Models;

public partial class Size
{
    public int SizeId { get; set; }

    public int ItemId { get; set; }

    public string SizeDesc { get; set; } = null!;

    public string Dimension { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal StepQuantity { get; set; }

    public decimal Price { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
