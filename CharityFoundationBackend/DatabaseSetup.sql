INSERT INTO Korisnici (Ime, Prezime, Email, Lozinka, TipKorisnika)
VALUES ('Admin', 'Sistem', 'admin@charity.com', 'admin123', 0);

INSERT INTO Korisnici (Ime, Prezime, Email, Lozinka, TipKorisnika)
VALUES ('Dino', 'Donator', 'dino@donacije.com', 'donator123', 1);

INSERT INTO Korisnici (Ime, Prezime, Email, Lozinka, TipKorisnika)
VALUES ('Selma', 'Pomoc', 'selma@pomoc.com', 'pomoc123', 2);

INSERT INTO Korisnici (Ime, Prezime, Email, Lozinka, TipKorisnika)
VALUES ('Vedad', 'Volonter', 'vedad@volontira.com', 'volonter123', 3);

INSERT INTO Donacije (IdKorisnika, Iznos, VrstaPomoci, Status, DatumDonacije)
VALUES
(2, 100.00, 'Hrana i higijena', 1, GETDATE()),
(2, 200.00, 'Školski pribor', 1, GETDATE()),
(2, 50.00, 'Topla odjeća', 0, GETDATE());

INSERT INTO ZahtjeviZaPomoc (IdKorisnika, OpisPotrebe, Urgentnost, Status)
VALUES
(3, 'Potreban paket hrane za porodicu sa troje djece', 2, 0),
(3, 'Molimo za školski pribor za djecu', 1, 1);

INSERT INTO Akcije (Naziv, Opis, Datum)
VALUES
('Distribucija paketa', 'Dijeljenje paketa pomoći u centru Sarajeva', '2025-06-10'),
('Pomoć starijim osobama', 'Volonteri obilaze stare i nemoćne korisnike', '2025-06-15');

INSERT INTO VolonterAkcije (IdVolontera, IdAkcije, StatusUcesca)
VALUES 
(4, 1, 0),
(4, 2, 0);

INSERT INTO Izvjestaji (Mjesec, Godina, BrojDonacija, UkupnaVrijednost)
VALUES
('Maj', 2025, 3, 350.00),
('April', 2025, 5, 680.00);



