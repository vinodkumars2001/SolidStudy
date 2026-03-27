using SolidStudy.DTOs;
using SolidStudy.Repositories;
using SolidStudy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProductRepository, InMemoryRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 10).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

var productGroup = app.MapGroup("/api/products").WithTags("Products");

productGroup.MapGet("/", async (IProductService productService) =>
{
    var results = await productService.GetAllProductAsync();
    return Results.Ok(results);
})
    .WithName("GetAllProducts");

productGroup.MapGet("/{id:guid}", async (Guid id, IProductService productService) =>
{
    var result = await productService.GetProductByIdAsync(id);
    if (result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
})
    .WithName("GetProductById");
// .WithOpenApi();

productGroup.MapPost("/", async (CreateProductRequest productRequest, IProductService productService) =>
{
    try
    {
        var createdProduct = await productService.CreateProductAsync(productRequest);
        return Results.Created($"/api/products/{createdProduct.Id}", createdProduct);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
    .WithName("CreateProduct");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
