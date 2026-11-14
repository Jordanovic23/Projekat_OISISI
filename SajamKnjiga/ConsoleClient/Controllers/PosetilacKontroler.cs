using ConsoleClient.Utils;
using Core.Model;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class PosetilacKontroler
    {
        private readonly PosetilacService _servis;

        public PosetilacKontroler(PosetilacService servis)
        {
            _servis = servis;
        }

        public void Meni()
        {
            KonzolaUtils.Meni("POSETIOCI",
                ("Prikaži sve posetioce", PrikaziSve),
                ("Dodaj posetioca", Dodaj),
                ("Izmeni posetioca", Izmeni),
                ("Obriši posetioca", Obrisi));
        }

        public void PrikaziSve()
        {
            Console.WriteLine("\n--- Posetioci ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "Br. karte", "Ime", "Prezime", "Rođen", "Adresa", "Telefon", "E-mail", "God. članstva", "Status" },
                _servis.GetAll().Select(p => new[]
                {
                    p.Id.ToString(), p.BrojClanskeKarte, p.Ime, p.Prezime, p.DatumRodjenja.ToString("dd.MM.yyyy"),
                    _servis.GetAdresa(p)?.ToString() ?? "-", p.KontaktTelefon, p.Email,
                    p.TrenutnaGodinaClanstva.ToString(), p.Status.ToString()
                }));
        }

        private void Dodaj()
        {
            Console.WriteLine("\n--- Dodavanje posetioca ---");
            var (posetilac, adresa) = UnesiPodatke(null, null);
            Posetilac dodat = _servis.Add(posetilac, adresa);
            Console.WriteLine($"  Posetilac dodat (ID: {dodat.Id}).");
        }

        private void Izmeni()
        {
            Posetilac stari = Izaberi();
            Console.WriteLine("\n--- Izmena posetioca (Enter zadržava staru vrednost) ---");
            var (izmenjen, adresa) = UnesiPodatke(stari, _servis.GetAdresa(stari));
            izmenjen.Id = stari.Id;
            _servis.Update(izmenjen, adresa);
            Console.WriteLine("  Posetilac izmenjen.");
        }

        private void Obrisi()
        {
            Posetilac p = Izaberi();
            if (!KonzolaUtils.Potvrda($"Obrisati posetioca {p.Ime} {p.Prezime}? Biće obrisane i njegove kupovine i lista želja"))
                return;
            _servis.Remove(p.Id);
            Console.WriteLine("  Posetilac obrisan.");
        }

        private Posetilac Izaberi()
        {
            PrikaziSve();
            int id = KonzolaUtils.Broj("ID posetioca");
            return _servis.GetById(id) ?? throw new InvalidOperationException("Posetilac nije pronađen.");
        }

        // stari == null -> dodavanje, inace izmena
        private static (Posetilac, Adresa) UnesiPodatke(Posetilac? stari, Adresa? staraAdresa)
        {
            var posetilac = new Posetilac
            {
                BrojClanskeKarte = KonzolaUtils.Tekst("Broj članske karte", stari?.BrojClanskeKarte),
                Ime = KonzolaUtils.Tekst("Ime", stari?.Ime),
                Prezime = KonzolaUtils.Tekst("Prezime", stari?.Prezime),
                DatumRodjenja = KonzolaUtils.Datum("Datum rođenja", stari?.DatumRodjenja),
                KontaktTelefon = KonzolaUtils.Tekst("Kontakt telefon", stari?.KontaktTelefon),
                Email = KonzolaUtils.Tekst("E-mail", stari?.Email),
                TrenutnaGodinaClanstva = KonzolaUtils.Broj("Trenutna godina članstva", stari?.TrenutnaGodinaClanstva),
                Status = KonzolaUtils.Izbor("Status", stari?.Status)
            };
            var adresa = new Adresa
            {
                Ulica = KonzolaUtils.Tekst("Ulica", staraAdresa?.Ulica),
                Broj = KonzolaUtils.Tekst("Broj", staraAdresa?.Broj),
                Grad = KonzolaUtils.Tekst("Grad", staraAdresa?.Grad),
                Drzava = KonzolaUtils.Tekst("Država", staraAdresa?.Drzava)
            };
            return (posetilac, adresa);
        }
    }
}