
using System.Security.Claims;
using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using FTACADEMY_STUDENT_MANAGEMENT_API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "FT_Academy_student_management_CSRF";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Add CORS services.  This is the important part!
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", //  Give it a name
        policy =>
        {
            policy.WithOrigins("https://localhost:7089") //  Replace with your frontend's origin
                   .AllowAnyMethod() //  Allow any HTTP method (GET, POST, etc.)
                   .AllowAnyHeader() //  Allow any headers
                   .AllowCredentials(); //  Important if you're using cookies for auth
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// **CORS Middleware:** Enable CORS *before* Authentication and Authorization
app.UseCors("AllowMyOrigin");
app.UseRouting();
app.UseAntiforgery(); 
app.MapControllers();

app.Run();
