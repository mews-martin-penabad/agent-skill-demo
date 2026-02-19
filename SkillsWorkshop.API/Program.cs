using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Application.Services;
using SkillsWorkshop.Infrastructure.Data;
using SkillsWorkshop.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// EF Core - InMemory for workshop purposes
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("SkillsWorkshopDb"));

builder.Services.AddScoped<ICardPaymentService, CardPaymentService>();
builder.Services.AddScoped<ICardPaymentRepository, CardPaymentRepository>();
builder.Services.AddScoped<IRefundService, RefundService>();
builder.Services.AddScoped<IRefundRepository, RefundRepository>();
builder.Services.AddScoped<IApplePayService, ApplePayService>();
builder.Services.AddScoped<IApplePayRepository, ApplePayRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
