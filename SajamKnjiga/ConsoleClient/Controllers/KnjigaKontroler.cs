using ConsoleClient.Utils;
using Core.Model;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class KnjigaKontroler
    {
        private readonly KnjigaService _servis;

        public KnjigaKontroler(KnjigaService servis)
        {
            _servis = servis;
        }

        public void Meni()
        {
            KonzolaUtils.Meni("KNJIGE",
                ("Prikaži sve knjige", PrikaziSve),
                ("Dodaj knjigu", Dodaj),
                ("Izmeni knjigu", Izmeni),
                ("Obriši knjigu", Obrisi));
        }

        public void PrikaziSve()
        {
            Console.WriteLine("\n--- Knjige ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "ISBN", "Naziv", "Žanr", "Godina", "Cena", "Strana", "ID izdavača", "Autori" },
                _servis.GetAll().Select(k =>
                {
                    var autori = _servis.GetAutori(k.Id);
                    return new[]
                    {
                        k.Id.ToString(), k.Sifra, k.Naziv, k.Zanr, k.GodinaIzdanja.ToString(),
                        k.Cena.ToString("N2"), k.BrojStrana.ToString(), k.IzdavacId.ToString(),
                        autori.Count == 0 ? "-" : string.Join(", ", autori.Select(a => $"{a.Ime} {a.Prezime}"))
                    };
                }));
        }

        private void Dodaj()
        {
            Console.WriteLine("\n--- Dodavanje knjige ---");
            Knjiga dodata = _servis.Add(UnesiPodatke(null));
            Console.WriteLine($"  Knjiga dodata (ID: {dodata.Id}).");
        }

        private void Izmeni()
        {
            Knjiga stara = Izaberi();
            Console.WriteLine("\n--- Izmena knjige (Enter zadržava staru vrednost) ---");
            Knjiga izmenjena = UnesiPodatke(stara);
            izmenjena.Id = stara.Id;
            _servis.Update(izmenjena);
            Console.WriteLine("  Knjiga izmenjena.");
        }

        private void Obrisi()
        {
            Knjiga k = Izaberi();
            if (!KonzolaUtils.Potvrda($"Obrisati knjigu '{k.Naziv}'? Biće obrisane i njene kupovine i stavke lista želja"))
                return;
            _servis.Remove(k.Id);
            Console.WriteLine("  Knjiga obrisana.");
        }

        private Knjiga Izaberi()
        {
            PrikaziSve();
            int id = KonzolaUtils.Broj("ID knjige");
            return _servis.GetById(id) ?? throw new InvalidOperationException("Knjiga nije pronađena.");
        }

        private static Knjiga UnesiPodatke(Knjiga? stara)
        {
            return new Knjiga
            {
                Sifra = KonzolaUtils.Tekst("ISBN", stara?.Sifra),
                Naziv = KonzolaUtils.Tekst("Naziv", stara?.Naziv),
                Zanr = KonzolaUtils.Tekst("Žanr", stara?.Zanr),
                GodinaIzdanja = KonzolaUtils.Broj("Godina izdanja", stara?.GodinaIzdanja),
                Cena = KonzolaUtils.Decimalni("Cena (RSD)", stara?.Cena),
                BrojStrana = KonzolaUtils.Broj("Broj strana", stara?.BrojStrana),
                IzdavacId = KonzolaUtils.Broj("ID izdavača", stara?.IzdavacId)
            };
        }
    }
}