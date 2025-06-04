# 🎗️ Charity Foundation

Kompletna full-stack aplikacija za upravljanje humanitarnim donacijama, zahtjevima za pomoć, volonterima i izvještajima. Omogućava efikasno povezivanje donatora, volontera i korisnika kojima je potrebna pomoć.

---

## Screenshots
[Pregled aplikacije](https://charity555.netlify.app/)

## Deployment
[Live deployed app](http://charity555-001-site1.ntempurl.com/)

`Username:` 11244910

`Password:` 60-dayfreetrial

Zbog tehničkog problema na hostingu molimo da prijavu i registraciju vršite preko `Account` rute za `Login` odnosno `Register`.

[Login](http://charity555-001-site1.ntempurl.com/Account/Login)

[Register](http://charity555-001-site1.ntempurl.com/Account/Register)

---

## 🔧 Tehnologije

- **Backend**: ASP.NET Core MVC (.NET 8)
- **Frontend**: Razor Views (MVC)
- **Baza**: Microsoft SQL Server (Cloud)
- **ORM**: Entity Framework Core

---

## 🧰 Potrebni alati

| Alat                                                                                  | Opis                           | Link |
| ------------------------------------------------------------------------------------- | ------------------------------ | ---- |
| [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)                            | Pokretanje backend servera     | ✅   |
| [Visual Studio Code](https://code.visualstudio.com)                                   | Editor za kod (opcionalno)     | ✅   |

---

## 🚀 Pokretanje projekta

```bash
cd CharityFoundation
dotnet run
```

🔗 Otvara se na: `https://localhost:3000`

---

## 🔐 Testni korisnici (uneseni u bazu)

| Uloga           | Email               | Lozinka     | 
| --------------- | ------------------- | ----------- | 
| Administrator   | admin@admin.com   | Admin123!    | 
| Donator         | dino@donator.com   | Donator123!  | 
| Primalac pomoći | tarik@primalac.com     | Primalac123!    | 
| Volonter        | vedad@volonter.com | Volonter123! |

---

## 📋 Funkcionalnosti po korisniku

### Administrator

- Upravljanje korisnicima
- Upravljanje donacijama
- Upravljanje zahtjevima za pomoć
- Upravljanje akcijama
- Upravljanje izvještajima

### Donator

- Kreira donacije
- Pregled svojih donacija

### Primalac pomoći

- Kreira zahtjeve
- Pregled svojih zahtjeva
- Vidi status zahtjeva

### Volonter

- Pregled akcija na koje je prijavljen
- Vidi zadatke i obaveze

---

