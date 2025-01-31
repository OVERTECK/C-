﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication2.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
