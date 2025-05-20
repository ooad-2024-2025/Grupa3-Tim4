using CharityFoundationBackend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CharityFoundationBackend.Services
{
    public class JwtService
    {
        private readonly string _key;
        private readonly string _issuer;

        public JwtService(IConfiguration config)
        {
            _key = config["Jwt:Key"];
            _issuer = config["Jwt:Issuer"];

            if (string.IsNullOrEmpty(_key) || string.IsNullOrEmpty(_issuer))
                throw new InvalidOperationException("JWT konfiguracija nije pravilno postavljena. Provjeri appsettings.json -> Jwt:Key i Jwt:Issuer.");
        }

        public string GenerateToken(Korisnik korisnik)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, korisnik.Email),
                new Claim("id", korisnik.Id.ToString()),
                new Claim(ClaimTypes.Role, korisnik.TipKorisnika.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
