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
                ("Dodaj izdavača", Dodaj),
                ("Izmeni izdavača", Izmeni),
                ("Obriši izdavača", Obrisi));
        }

        public void PrikaziSve()
        {
            Console.WriteLine("\n--- Izdavači ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "Šifra", "Naziv", "ID šefa", "Knjiga" },
                _servis.GetAll().Select(i => new[]
                {
                    i.Id.ToString(), i.Sifra, i.Naziv,
                    i.IdSefa == Izdavac.BEZ_SEFA ? "-" : i.IdSefa.ToString(),
                    _servis.BrojKnjiga(i.Id).ToString()
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