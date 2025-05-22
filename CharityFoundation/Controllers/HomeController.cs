using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CharityFoundation.Models;

public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Index");

        return user.TipKorisnika switch
        {
            "Administrator" => RedirectToAction("AdminDashboard"),
            "Donator" => RedirectToAction("DonatorDashboard"),
            "PrimalacPomoci" => RedirectToAction("PrimalacDashboard"),
            "Volonter" => RedirectToAction("VolonterDashboard"),
            _ => RedirectToAction("Index")
        };
    }

    public IActionResult AdminDashboard() => View();
    public IActionResult DonatorDashboard() => View();
    public IActionResult PrimalacDashboard() => View();
    public IActionResult VolonterDashboard() => View();

    public IActionResult Index() => View();
    public IActionResult Contact() => View();
    public IActionResult About() => View();
}
