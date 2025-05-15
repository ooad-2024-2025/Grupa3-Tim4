
# 🎗️ Charity Foundation

Kompletna full-stack aplikacija za upravljanje humanitarnim donacijama, zahtjevima za pomoć, volonterima i izvještajima. Omogućava efikasno povezivanje donatora, volontera i korisnika kojima je potrebna pomoć.

---

## 🔧 Tehnologije

- **Backend**: ASP.NET Core (.NET 8)
- **Frontend**: React + Vite + TypeScript
- **Baza**: Microsoft SQL Server Express
- **ORM**: Entity Framework Core

---

## 🧰 Potrebni alati

| Alat                        | Opis                                   | Link |
|----------------------------|----------------------------------------|------|
| [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download) | Pokretanje backend servera         | ✅   |
| [Node.js v18+](https://nodejs.org)             | Pokretanje frontend aplikacije     | ✅   |
| [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) | Lokalna baza podataka              | ✅   |
| [SSMS](https://aka.ms/ssmsfullsetup)           | GUI za bazu                        | ✅   |
| [Visual Studio Code](https://code.visualstudio.com) | Editor za kod (opcionalno)         | ✅   |

---

## 🛠️ Setup lokalne baze (SQL Server)

1. Pokreni SQL Server Express i otvori **SQL Server Management Studio (SSMS)**.
2. Kreiraj novu bazu:
   - Naziv: `CharityFoundationDB`
3. U fajlu `appsettings.json` unutar `CharityFoundationBackend`, postavi konekciju:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CharityFoundationDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

4. U konzoli pokreni migracije:

```bash
cd CharityFoundationBackend
dotnet ef database update
```

5. Ručno unesi sample podatke (npr. korisnike) preko SSMS-a.

---

## 🚀 Pokretanje projekta

### ▶️ Backend

```bash
cd CharityFoundationBackend
dotnet run
```

🔗 Otvara se na: `http://localhost:5000`

---

### 💻 Frontend

```bash
cd CharityFoundationFrontend
npm install
npm run dev
```

🌐 Otvara se na: `http://localhost:3000`

---

## 🔐 Testni korisnici (uneseni ručno u bazu)

| Uloga             | Email                  | Lozinka      | TipKorisnika |
|------------------|------------------------|--------------|--------------|
| Administrator  | admin@charity.com      | admin123     | 0            |
| Donator        | dino@donacije.com      | donator123   | 1            |
| Primalac pomoći | selma@pomoc.com        | pomoc123     | 3            |
| Volonter       | vedad@volontira.com    | volonter123  | 4            |

📌 `TipKorisnika` (enum):
- `0` = Administrator
- `1` = Donator
- `3` = Primalac pomoći
- `4` = Volonter

---

## 📋 Funkcionalnosti po korisniku

### Administrator
- Pristup svim korisnicima
- Pregled i raspodjela donacija
- Upravljanje izvještajima i obavijestima
- Praćenje aktivnosti volontera

### Donator
- Kreira donacije
- Prati status vlastitih donacija
- Pregled rang liste najaktivnijih donatora

### Primalac pomoći
- Kreira zahtjeve
- Vidi status zahtjeva
- Komunikacija sa sistemom

### Volonter
- Prijavljuje se na volonterske akcije
- Vidi zadatke i obaveze
- Upravljanje ličnim učešćima

---

## 📬 API Pregled (primjeri)

| Endpoint                  | Metod | Opis                         |
|--------------------------|-------|------------------------------|
| `/api/korisnik`          | GET   | Prikaz svih korisnika       |
| `/api/donacija`          | GET   | Prikaz svih donacija        |
| `/api/zahtjev`           | GET   | Prikaz zahtjeva za pomoć    |
| `/api/akcija`            | GET   | Prikaz volonterskih akcija  |

---

## ⚠️ Napomena

- Projekat **ne koristi seed** — svi podaci (korisnici, donacije, zahtjevi itd.) se unose **ručno preko SSMS-a**.
- Login koristi **fiktivne JWT tokene** (`"fake-jwt-token"`) bez validacije — za potrebe prototipa.

---

