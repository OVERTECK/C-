using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication2.Entities;

public partial class Product
{
    public uint Idproduct { get; set; }

    public string Title { get; set; } = null!;

    public int CategoriesId { get; set; }

    public string? TitlePath { get; set; }

    public byte[]? Image { get; set; }

    [JsonIgnore]
    public virtual Category Categories { get; set; } = null!;
}
