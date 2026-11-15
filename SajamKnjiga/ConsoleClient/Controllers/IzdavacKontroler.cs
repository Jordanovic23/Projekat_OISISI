using ConsoleClient.Utils;
using Core.Model;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class IzdavacKontroler
    {
        private readonly IzdavacService _servis;

        public IzdavacKontroler(IzdavacService servis)
        {
            _servis = servis;
        }

        public void Meni()
        {
            KonzolaUtils.Meni("IZDAVAČI",
                ("Prikaži sve izdavače", PrikaziSve),
                ("Prikaži knjige i autore izdavača", PrikaziDetalje),
                ("Dodaj izdavača", Dodaj),
                ("Izmeni izdavača", Izmeni),
                ("Obriši izdavača", Obrisi),
                ("Postavi šefa izdavača", PostaviSefa));
        }

        public void PrikaziSve()
        {
            Console.WriteLine("\n--- Izdavači ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "Šifra", "Naziv", "Šef", "Knjiga", "Autora" },
                _servis.GetAll().Select(i =>
                {
                    Autor? sef = _servis.GetSef(i);
                    return new[]
                    {
                        i.Id.ToString(), i.Sifra, i.Naziv,
                        sef == null ? "-" : $"{sef.Ime} {sef.Prezime}",
                        _servis.BrojKnjiga(i.Id).ToString(),
                        _servis.GetAutori(i.Id).Count.ToString()
                    };
                }));
        }

        private void PrikaziDetalje()
        {
            Izdavac i = Izaberi();
            Console.WriteLine($"\n--- Knjige izdavača {i.Naziv} ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "ISBN", "Naziv", "Žanr", "Godina", "Cena" },
                _servis.GetKnjige(i.Id).Select(k => new[]
                {
                    k.Id.ToString(), k.Sifra, k.Naziv, k.Zanr, k.GodinaIzdanja.ToString(), k.Cena.ToString("N2")
                }));

            Console.WriteLine($"\n--- Autori izdavača {i.Naziv} ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "Ime", "Prezime", "God. iskustva" },
                _servis.GetAutori(i.Id).Select(a => new[]
                {
                    a.Id.ToString(), a.Ime, a.Prezime, a.GodineIskustva.ToString()
                }));
        }

        private void Dodaj()
        {
            Console.WriteLine("\n--- Dodavanje izdavača ---");
            Izdavac dodat = _servis.Add(UnesiPodatke(null));
            Console.WriteLine($"  Izdavač dodat (ID: {dodat.Id}).");
        }

        private void Izmeni()
        {
            Izdavac stari = Izaberi();
            Console.WriteLine("\n--- Izmena izdavača (Enter zadržava staru vrednost) ---");
            Izdavac izmenjen = UnesiPodatke(stari);
            izmenjen.Id = stari.Id;
            _servis.Update(izmenjen);
            Console.WriteLine("  Izdavač izmenjen.");
        }

        private void Obrisi()
        {
            Izdavac i = Izaberi();
            if (!KonzolaUtils.Potvrda($"Obrisati izdavača {i.Naziv}?"))
                return;
            _servis.Remove(i.Id);
            Console.WriteLine("  Izdavač obrisan.");
        }

        private void PostaviSefa()
        {
            Izdavac i = Izaberi();
            var kandidati = _servis.GetAutori(i.Id);
            Console.WriteLine($"Autori koji su izdavali knjige kod izdavača {i.Naziv}:");
            KonzolaUtils.Tabela(
                new[] { "ID", "Ime", "Prezime", "God. iskustva" },
                kandidati.Select(a => new[] { a.Id.ToString(), a.Ime, a.Prezime, a.GodineIskustva.ToString() }));
            if (kandidati.Count == 0) return;

            _servis.PostaviSefa(i.Id, KonzolaUtils.Broj("ID autora koji postaje šef"));
            Console.WriteLine("  Šef izdavača postavljen.");
        }

        private Izdavac Izaberi()
        {
            PrikaziSve();
            int id = KonzolaUtils.Broj("ID izdavača");
            return _servis.GetById(id) ?? throw new InvalidOperationException("Izdavač nije pronađen.");
        }

        private static Izdavac UnesiPodatke(Izdavac? stari)
        {
            return new Izdavac
            {
                Sifra = KonzolaUtils.Tekst("Šifra", stari?.Sifra),
                Naziv = KonzolaUtils.Tekst("Naziv", stari?.Naziv)
            };
        }
    }
}