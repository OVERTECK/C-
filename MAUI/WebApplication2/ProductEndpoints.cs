using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using WebApplication2.Data;
using WebApplication2.Entities;
namespace WebApplication2;

public static class ProductEndpoints
{
    public static void MapProductEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Product").WithTags(nameof(Product));

        group.MapGet("/", async (Db1Context db) =>
        {
            return await db.Products.ToListAsync();
        })
        .WithName("GetAllProducts")
        .WithOpenApi()
        .Produces<List<Product>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async  (uint idproduct, Db1Context db) =>
        {
            return await db.Products.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Idproduct == idproduct)
                is Product model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithOpenApi()
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async  (uint idproduct, Product product, Db1Context db) =>
        {
            var affected = await db.Products
                .Where(model => model.Idproduct == idproduct)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Idproduct, product.Idproduct)
                    .SetProperty(m => m.Title, product.Title)
                    .SetProperty(m => m.CategoriesId, product.CategoriesId)
                    .SetProperty(m => m.TitlePath, product.TitlePath)
                    .SetProperty(m => m.Image, product.Image)
                    );
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateProduct")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/", async (Product product, Db1Context db) =>
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Results.Created($"/api/Product/{product.Idproduct}",product);
        })
        .WithName("CreateProduct")
        .WithOpenApi()
        .Produces<Product>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", async  (uint idproduct, Db1Context db) =>
        {
            var affected = await db.Products
                .Where(model => model.Idproduct == idproduct)
                .ExecuteDeleteAsync();
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteProduct")
        .WithOpenApi()
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
