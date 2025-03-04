﻿using System;
using System.Collections.Generic;

namespace Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public override string ToString()
    {
        return Title;
    }
}
