using System;
using System.Collections.Generic;

namespace RepositoryAPI.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int SizeId { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal Price { get; set; }

    public bool Statuse { get; set; }

    public string Date { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
