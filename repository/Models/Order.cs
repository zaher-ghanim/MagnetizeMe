using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RepositoryAPI.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int SizeId { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal Price { get; set; }

    public bool Status { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
    public int Qty { get; set; }
    [JsonIgnore]
    public virtual Size Size { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
