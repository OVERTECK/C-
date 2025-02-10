using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using WebApplication2.Data;
using WebApplication2.Entities;
namespace WebApplication2;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(User));

        group.MapGet("/{id}", async  (int iduser, Db1Context db) =>
        {
            return await db.Users.AsNoTracking()
                .FirstOrDefaultAsync(model => model.IdUser == iduser)
                is User model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetUserById")
        .WithOpenApi()
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async  (int iduser, User user, Db1Context db) =>
        {
            var affected = await db.Users
                .Where(model => model.IdUser == iduser)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.IdUser, user.IdUser)
                    .SetProperty(m => m.Login, user.Login)
                    .SetProperty(m => m.Email, user.Email)
                    .SetProperty(m => m.Password, user.Password)
                    );
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateUser")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/", async (User user, Db1Context db) =>
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/api/User/{user.IdUser}",user);
        })
        .WithName("CreateUser")
        .WithOpenApi()
        .Produces<User>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", async  (int iduser, Db1Context db) =>
        {
            var affected = await db.Users
                .Where(model => model.IdUser == iduser)
                .ExecuteDeleteAsync();
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteUser")
        .WithOpenApi()
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
