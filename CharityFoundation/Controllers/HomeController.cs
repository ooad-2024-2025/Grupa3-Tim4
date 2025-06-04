using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
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

        [HttpGet]
        public IActionResult Contact() => View();

        [HttpPost]
        public async Task<IActionResult> Contact(string ime, string email, string poruka)
        {
            if (string.IsNullOrWhiteSpace(ime) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(poruka))
            {
                TempData["Greska"] = "Sva polja su obavezna.";
                return View();
            }

            var subject = $"Kontakt poruka od {ime}";
            var body = $@"
                <h4>Nova kontakt poruka</h4>
                <p><strong>Ime:</strong> {ime}</p>
                <p><strong>Email:</strong> <a href='mailto:{email}'>{email}</a></p>
                <p><strong>Poruka:</strong></p>
                <p>{poruka}</p>";

            await _emailSender.SendEmailAsync("info@charity.com", subject, body);

            var korisnickiBody = $@"
                <h4>Poštovani {ime},</h4>
                <p>Primili smo vašu poruku i odgovorit ćemo vam uskoro. Ovo je kopija vaše poruke:</p>
                <hr />
                <p>{poruka}</p>
                <hr />
                <p><i>Charity Foundation</i></p>";

            await _emailSender.SendEmailAsync(email, "Kopija vaše poruke - Charity Foundation", korisnickiBody);

            TempData["Uspjeh"] = "Poruka je uspješno poslata. Primili ste kopiju na email.";
            return RedirectToAction("Contact");
        }

        public IActionResult About() => View();
    }
}
