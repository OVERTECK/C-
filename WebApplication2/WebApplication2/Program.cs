using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication2;
using WebApplication2.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/register", (User newUser) =>
{
    var searchUserByEmail = Db1Context.GetContext().Users.FirstOrDefault(c => c.Email == newUser.Email);
    var searchUserByLogin = Db1Context.GetContext().Users.FirstOrDefault(c => c.Login == newUser.Login);

    if (searchUserByEmail != null)
    {
        return Results.BadRequest("Почта занята!");
    };

    if (searchUserByLogin != null)
    {
        return Results.BadRequest("Логин занят!");
    }

    newUser.Password = Hash.createHash(newUser.Password);

    Db1Context.GetContext().Users.Add(newUser);

    Db1Context.GetContext().SaveChangesAsync();

    return Results.Ok();
});

app.MapPost("/login", (User user) =>
{
    var seachedUserByEmail = Db1Context.GetContext()
        .Users
        .SingleOrDefault(c => c.Email == user.Email
    );

    if (seachedUserByEmail == null)
    {
        return Results.NotFound("Почта не найдена.");
    }

    var seachedUser = Db1Context.GetContext()
        .Users
        .SingleOrDefault(c => c.Email == user.Email && c.Password == user.Password
    );

    if (seachedUser == null)
    {
        return Results.NotFound("Пароль не верный!");
    }

    return Results.Ok();
});

app.MapGet("products/", () =>
{
    return Results.Ok(Db1Context.GetContext().Products);
});

app.MapGet("users/{login}", (string login) =>
{
    var searchedUser = Db1Context.GetContext().Users.FirstOrDefault(c => c.Login == login);

    if (searchedUser != null)
    {
        return Results.Ok("Логин занят!");
    }

    return Results.NotFound();
});

app.MapPost("products/", (Product product) =>
{
    Db1Context.GetContext().Products.Add(product);

    Db1Context.GetContext().SaveChanges();

    return Results.Created();
});

app.MapDelete("products/{idProduct}", (int idProduct) =>
{
    Db1Context.GetContext().Products.Where(c => idProduct == c.Idproduct).ExecuteDelete();

    return TypedResults.Ok();
});

app.MapGet("categories/", () =>
{
    return Results.Ok(Db1Context.GetContext().Categories);
});

app.Run();
