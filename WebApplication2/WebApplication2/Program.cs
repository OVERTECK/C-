using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/users", () => Db1Context.GetContext().Users);

app.MapPost("/login", (HttpContext httpContext) =>
{
    var user = httpContext.Request.ReadFromJsonAsync<User>().Result;

    if (user == null)
    {
        return (IResult)TypedResults.NotFound();
    }

    var email = user.Email;

    var password = user.Password;

    var seachedUser = Db1Context.GetContext()
        .Users
        .SingleOrDefault(c => c.Email == user.Email && c.Password == user.Password
    );

    if (seachedUser != null)
        return TypedResults.Ok();
    else
        return TypedResults.NotFound();
});

app.MapGet("/users/{id}", (int id) =>
{
    var searchedUser = Db1Context.GetContext()
    .Users
    .SingleOrDefault(c => c.IdUser == id);

    return searchedUser;
});

app.MapPost("users/create", (User newUser) =>
{
    //if (Db1Context.GetContext().Users.FirstOrDefault(c => c.Email == newUser.Email) != null)
    //    return TypedResults.BadRequest();

    Db1Context.GetContext().Users.Add(newUser);

    Db1Context.GetContext().SaveChanges();

    return TypedResults.Created($"users/{newUser.IdUser}", newUser);
});

app.MapDelete("users/delete/{idUser}", (int idUser) =>
{
    Db1Context.GetContext().Users.Where(c => idUser == c.IdUser).ExecuteDelete();

    return TypedResults.Ok();
});

app.Run();
