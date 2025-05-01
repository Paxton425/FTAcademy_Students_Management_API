
using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FtacademyStudentManagementContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "FT_Academy_student_management_CSRF";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Add Authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.CallbackPath = "/auth/google";
    options.SaveTokens = true;

    options.Scope.Add("profile");
    options.Scope.Add("email");
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
