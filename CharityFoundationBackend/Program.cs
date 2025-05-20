using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using CharityFoundationBackend.Services;
using System.Text;
using CharityFoundationBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// 📦 1. Konekcija na bazu
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 📦 2. Kontroleri i MVC
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // Za [ApiController]
builder.Services.AddScoped<JwtService>();

// 🌐 3. CORS (omogućava React frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🔐 4. JWT autentikacija
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = key
        };
    });

// 🧠 5. Role-based authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("DonatorPolicy", policy => policy.RequireRole("Donator"));
    options.AddPolicy("PrimalacPolicy", policy => policy.RequireRole("Primalac"));
    options.AddPolicy("VolonterPolicy", policy => policy.RequireRole("Volonter"));
});

var app = builder.Build();

// 🧭 6. Error handling i routing
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🛡️ 7. Middleware za CORS i autentikaciju
app.UseCors("AllowFrontend");

app.UseAuthentication(); // ⬅️ Mora biti prije Authorization
app.UseAuthorization();

// 🧭 8. Mape
app.MapControllers(); // API kontrolleri

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
