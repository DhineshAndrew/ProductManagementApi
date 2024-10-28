using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ProductManagementApi.Data;
using ProductManagementApi.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProductService>();

builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer("Server=SAI-5CD335HY45\\SQLEXPRESS;Database=ProductDatabase;Trusted_Connection=True;TrustServerCertificate=True;"));


builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<ProductValidator>();
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"
});
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();
