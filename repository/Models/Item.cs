using System;
using System.Collections.Generic;

namespace RepositoryAPI.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemDesc { get; set; } = null!;

    public virtual ICollection<Size> Sizes { get; set; } = new List<Size>();
}
