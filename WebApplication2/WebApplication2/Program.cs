using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication2;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebApplication2.Entities;
using System.Text;
using System.Security.Claims;
using NuGet.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Db1Context>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/register", (User newUser, Db1Context db) =>
{
    var searchUserByEmail = db.Users.FirstOrDefault(c => c.Email == newUser.Email);
    var searchUserByLogin = db.Users.FirstOrDefault(c => c.Login == newUser.Login);

    if (searchUserByEmail != null)
    {
        return Results.BadRequest("Почта занята!");
    };

    if (searchUserByLogin != null)
    {
        return Results.BadRequest("Логин занят!");
    }

    newUser.Password = Hash.createHash(newUser.Password);

    db.Users.Add(newUser);

    db.SaveChangesAsync();

    return Results.Ok();
});

app.MapPost("/login", (User user, Db1Context db) =>
{
    var seachedUserByEmail = db.Users.SingleOrDefault(c => c.Email == user.Email);

    if (seachedUserByEmail == null)
    {
        return Results.NotFound("Почта не найдена.");
    }

    var seachedUser = db.Users.SingleOrDefault(c => c.Email == user.Email && c.Password == user.Password);

    if (seachedUser == null)
    {
        return Results.Unauthorized();
    }

    var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };

    var jwt = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256)
    );


    return Results.Ok( new { Token = new JwtSecurityTokenHandler().WriteToken(jwt)} );
});

app.MapGet("products/", [Authorize] async (Db1Context db) =>
{
        var products = await db.Products.ToListAsync();

        return Results.Ok(products);
});

app.MapGet("users/{login}", (string login, Db1Context db) =>
{
    var searchedUser = db.Users.FirstOrDefault(c => c.Login == login);

    if (searchedUser != null)
    {
        return Results.Ok("Логин занят!");
    }

    return Results.NotFound();
});

app.MapPost("products/", async (Product product, Db1Context db) =>
{
    db.Products.Add(product);

    await db.SaveChangesAsync();

    return Results.Created();
});

app.MapDelete("products/{idProduct}", async (int idProduct, Db1Context db) =>
{
    await db.Products.Where(c => idProduct == c.Idproduct).ExecuteDeleteAsync();

    return TypedResults.Ok();
});

app.MapGet("products/{idProduct}", async (int idProduct, Db1Context db) =>
{
    var searchedProduct = await db.Products.FirstOrDefaultAsync(c => idProduct == c.Idproduct);

    return TypedResults.Ok(searchedProduct);
});

app.MapGet("categories/", async (ILogger<Program> logger, Db1Context db) =>
{
    try
    {
        List<Category> categories = await db.Categories.ToListAsync();

        return Results.Ok(categories);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ошибка при добавлении продукта");

        return Results.StatusCode(500);
    }
});

app.Run();
