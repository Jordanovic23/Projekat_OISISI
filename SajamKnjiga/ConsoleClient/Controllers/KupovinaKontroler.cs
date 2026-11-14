using ConsoleClient.Utils;
using Core.Model;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class KupovinaKontroler
    {
        private readonly KupovinaService _servis;

        public KupovinaKontroler(KupovinaService servis)
        {
            _servis = servis;
        }

        public void Meni()
        {
            KonzolaUtils.Meni("KUPOVINE",
                ("Prikaži sve kupovine", PrikaziSve),
                ("Dodaj kupovinu", Dodaj),
                ("Izmeni kupovinu", Izmeni),
                ("Obriši kupovinu", Obrisi));
        }

        public void PrikaziSve()
        {
            Console.WriteLine("\n--- Kupovine ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "Posetilac", "Knjiga", "Datum", "Ocena", "Komentar" },
                _servis.GetAll().Select(k => new[]
                {
                    k.Id.ToString(),
                    $"{k.PosetilacId} - {_servis.ImePosetioca(k.PosetilacId)}",
                    $"{k.KnjigaId} - {_servis.NazivKnjige(k.KnjigaId)}",
                    k.DatumKupovine.ToString("dd.MM.yyyy"),
                    ((int)k.Ocena).ToString(),
                    k.Komentar
                }));
        }

        private void Dodaj()
        {
            Console.WriteLine("\n--- Dodavanje kupovine ---");
            var kupovina = new Kupovina
            {
                PosetilacId = KonzolaUtils.Broj("ID posetioca"),
                KnjigaId = KonzolaUtils.Broj("ID knjige"),
                DatumKupovine = KonzolaUtils.Datum("Datum kupovine"),
                Ocena = KonzolaUtils.Izbor<Ocena>("Ocena"),
                Komentar = UnesiKomentar(null)
            };
            Kupovina dodata = _servis.Add(kupovina);
            Console.WriteLine($"  Kupovina dodata (ID: {dodata.Id}).");
        }

        private void Izmeni()
        {
            Kupovina stara = Izaberi();
            Console.WriteLine("\n--- Izmena kupovine (Enter zadržava staru vrednost) ---");
            Console.WriteLine("  (posetilac i knjiga se ne menjaju)");
            var izmenjena = new Kupovina
            {
                Id = stara.Id,
                PosetilacId = stara.PosetilacId,
                KnjigaId = stara.KnjigaId,
                DatumKupovine = KonzolaUtils.Datum("Datum kupovine", stara.DatumKupovine),
                Ocena = KonzolaUtils.Izbor<Ocena>("Ocena", stara.Ocena),
                Komentar = UnesiKomentar(stara.Komentar)
            };
            _servis.Update(izmenjena);
            Console.WriteLine("  Kupovina izmenjena.");
        }

        private void Obrisi()
        {
            Kupovina k = Izaberi();
            if (!KonzolaUtils.Potvrda($"Obrisati kupovinu ID {k.Id}?"))
                return;
            _servis.Remove(k.Id);
            Console.WriteLine("  Kupovina obrisana.");
        }

        private Kupovina Izaberi()
        {
            PrikaziSve();
            int id = KonzolaUtils.Broj("ID kupovine");
            return _servis.GetById(id) ?? throw new InvalidOperationException("Kupovina nije pronađena.");
        }

        // Komentar je opcion (moze biti prazan); ';' se menja jer je to separator u CSV-u
        private static string UnesiKomentar(string? trenutni)
        {
            Console.Write(trenutni == null ? "Komentar (opciono): " : $"Komentar [{trenutni}]: ");
            string unos = KonzolaUtils.Citaj().Trim().Replace(';', ',');
            return unos == "" && trenutni != null ? trenutni : unos;
        }
    }
}