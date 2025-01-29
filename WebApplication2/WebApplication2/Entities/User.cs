using System;
using System.Collections.Generic;

namespace WebApplication2.Entities;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
