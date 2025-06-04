using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;
using CharityFoundation.Services;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ✅ QuestPDF licenca
QuestPDF.Settings.License = LicenseType.Community;

// ✅ Konekcija na bazu iz appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ Dodaj Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>();

// ✅ Dodaj Email servis
builder.Services.AddTransient<IEmailSender, EmailSender>();

// ✅ MVC + Razor Pages (potrebno za Identity UI)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ✅ Automatski kreiraj Admin korisnika ako ne postoji
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    CreateAdminUser(userManager).Wait();
}

static async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
{
    var adminEmail = "admin@admin.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        var newAdmin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            Ime = "Admin",
            Prezime = "Sistem",
            TipKorisnika = "Administrator"
        };

        var result = await userManager.CreateAsync(newAdmin, "Admin123!");

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Greška pri kreiranju admina: {error.Description}");
            }
        }
        else
        {
            Console.WriteLine("✅ Admin korisnik uspješno kreiran.");
        }
    }
}

// 🔧 Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
