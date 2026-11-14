using ConsoleClient.Utils;
using Core.Model;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class AutorKontroler
    {
        private readonly AutorService _servis;

        public AutorKontroler(AutorService servis)
        {
            _servis = servis;
        }

        public void Meni()
        {
            KonzolaUtils.Meni("AUTORI",
                ("Prikaži sve autore", PrikaziSve),
                ("Dodaj autora", Dodaj),
                ("Izmeni autora", Izmeni),
                ("Obriši autora", Obrisi));
        }

        public void PrikaziSve()
        {
            Console.WriteLine("\n--- Autori ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "Ime", "Prezime", "Rođen", "Adresa", "Telefon", "E-mail", "Br. LK", "God. iskustva", "Knjiga" },
                _servis.GetAll().Select(a => new[]
                {
                    a.Id.ToString(), a.Ime, a.Prezime, a.DatumRodjenja.ToString("dd.MM.yyyy"),
                    _servis.GetAdresa(a)?.ToString() ?? "-", a.Telefon, a.Email, a.BrojLK,
                    a.GodineIskustva.ToString(), _servis.BrojKnjiga(a.Id).ToString()
                }));
        }

        private void Dodaj()
        {
            Console.WriteLine("\n--- Dodavanje autora ---");
            var (autor, adresa) = UnesiPodatke(null, null);
            Autor dodat = _servis.Add(autor, adresa);
            Console.WriteLine($"  Autor dodat (ID: {dodat.Id}).");
        }

        private void Izmeni()
        {
            Autor stari = Izaberi();
            Console.WriteLine("\n--- Izmena autora (Enter zadržava staru vrednost) ---");
            var (izmenjen, adresa) = UnesiPodatke(stari, _servis.GetAdresa(stari));
            izmenjen.Id = stari.Id;
            _servis.Update(izmenjen, adresa);
            Console.WriteLine("  Autor izmenjen.");
        }

        private void Obrisi()
        {
            Autor a = Izaberi();
            if (!KonzolaUtils.Potvrda($"Obrisati autora {a.Ime} {a.Prezime}? Njegove knjige ostaju bez ovog autora"))
                return;
            _servis.Remove(a.Id);
            Console.WriteLine("  Autor obrisan.");
        }

        private Autor Izaberi()
        {
            PrikaziSve();
            int id = KonzolaUtils.Broj("ID autora");
            return _servis.GetById(id) ?? throw new InvalidOperationException("Autor nije pronađen.");
        }

        // stari == null -> dodavanje, inace izmena
        private static (Autor, Adresa) UnesiPodatke(Autor? stari, Adresa? staraAdresa)
        {
            var autor = new Autor
            {
                Ime = KonzolaUtils.Tekst("Ime", stari?.Ime),
                Prezime = KonzolaUtils.Tekst("Prezime", stari?.Prezime),
                DatumRodjenja = KonzolaUtils.Datum("Datum rođenja", stari?.DatumRodjenja),
                Telefon = KonzolaUtils.Tekst("Telefon", stari?.Telefon),
                Email = KonzolaUtils.Tekst("E-mail", stari?.Email),
                BrojLK = KonzolaUtils.Tekst("Broj lične karte", stari?.BrojLK),
                GodineIskustva = KonzolaUtils.Broj("Godine iskustva", stari?.GodineIskustva)
            };
            var adresa = new Adresa
            {
                Ulica = KonzolaUtils.Tekst("Ulica", staraAdresa?.Ulica),
                Broj = KonzolaUtils.Tekst("Broj", staraAdresa?.Broj),
                Grad = KonzolaUtils.Tekst("Grad", staraAdresa?.Grad),
                Drzava = KonzolaUtils.Tekst("Država", staraAdresa?.Drzava)
            };
            return (autor, adresa);
        }
    }
}