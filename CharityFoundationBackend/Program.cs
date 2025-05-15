using Microsoft.EntityFrameworkCore;
using CharityFoundationBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Konekcija na SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 📦 Registruj sve kontrolere
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // važno za ApiController

var app = builder.Build();

// 🌐 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // prikaz detaljnih grešaka
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); // koristi ako planiraš JWT/token auth

// 🧭 Mapiranje API kontrolera
app.MapControllers();

// 🎯 Defaultna MVC ruta
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
