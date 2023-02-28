using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using task2.BLL;
using task2.Context;
using task2.Controllers.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<BooksRepository>();
builder.Services.AddDbContext<AppDbContext>(x => {
    x.UseInMemoryDatabase("task2_db");
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SupportNonNullableReferenceTypes(); // enabling non-nullable string
                                                // to have corresponding representation
});

builder.Services.AddTransient<IValidatorInterceptor, UseCustomErrorModelInterceptor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorMiddleware>();

app.MapControllers();

using var scope = app.Services.CreateScope();
var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

Initializer.InitDatabase(appContext);
Initializer.Seed(appContext);

app.Run();
