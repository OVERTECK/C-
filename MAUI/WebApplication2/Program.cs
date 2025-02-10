using WebApplication2;
using WebApplication2.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Db1Context>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapUserEndpoints();

app.MapProductEndpoints();

app.MapCategoryEndpoints();

app.Run();