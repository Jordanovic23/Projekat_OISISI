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
                ("Detalji posetioca (kupovine i lista želja)", PrikaziDetalje),
                ("Dodaj posetioca", Dodaj),
                ("Izmeni posetioca", Izmeni),
                ("Obriši posetioca", Obrisi),
                ("Dodaj knjigu na listu želja", DodajNaListuZelja),
                ("Ukloni knjigu sa liste želja", UkloniSaListeZelja));
        }

        public void PrikaziSve()
        {
            Console.WriteLine("\n--- Posetioci ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "Br. karte", "Ime", "Prezime", "Rođen", "Adresa", "Telefon", "E-mail", "God. članstva", "Status", "Kupovina", "Lista želja", "Pros. ocena" },
                _servis.GetAll().Select(p =>
                {
                    double? prosek = _servis.ProsecnaOcena(p.Id);
                    return new[]
                    {
                        p.Id.ToString(), p.BrojClanskeKarte, p.Ime, p.Prezime, p.DatumRodjenja.ToString("dd.MM.yyyy"),
                        _servis.GetAdresa(p)?.ToString() ?? "-", p.KontaktTelefon, p.Email,
                        p.TrenutnaGodinaClanstva.ToString(), p.Status.ToString(),
                        _servis.GetKupovine(p.Id).Count.ToString(),
                        _servis.GetListaZelja(p.Id).Count.ToString(),
                        prosek == null ? "-" : prosek.Value.ToString("0.0")
                    };
                }));
        }

        private void PrikaziDetalje()
        {
            Posetilac p = Izaberi();

            Console.WriteLine($"\n--- Kupljene knjige: {p.Ime} {p.Prezime} ---");
            KonzolaUtils.Tabela(
                new[] { "ID kupovine", "Knjiga", "Datum", "Ocena", "Komentar" },
                _servis.GetKupovine(p.Id).Select(k => new[]
                {
                    k.Id.ToString(), k.KnjigaId.ToString(), k.DatumKupovine.ToString("dd.MM.yyyy"),
                    ((int)k.Ocena).ToString(), k.Komentar
                }));
            double? prosek = _servis.ProsecnaOcena(p.Id);
            Console.WriteLine(prosek == null ? "Prosečna ocena: -" : $"Prosečna ocena: {prosek.Value:0.0}");

            Console.WriteLine($"\n--- Lista želja: {p.Ime} {p.Prezime} ---");
            KonzolaUtils.Tabela(
                new[] { "ID", "ISBN", "Naziv", "Cena" },
                _servis.GetListaZelja(p.Id).Select(k => new[]
                {
                    k.Id.ToString(), k.Sifra, k.Naziv, k.Cena.ToString("N2")
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

        private void DodajNaListuZelja()
        {
            Posetilac p = Izaberi();
            var dostupne = _servis.GetKnjigeZaListuZelja(p.Id);
            Console.WriteLine("Knjige koje možete dodati na listu želja:");
            KonzolaUtils.Tabela(
                new[] { "ID", "ISBN", "Naziv", "Cena" },
                dostupne.Select(k => new[] { k.Id.ToString(), k.Sifra, k.Naziv, k.Cena.ToString("N2") }));
            if (dostupne.Count == 0) return;

            _servis.DodajNaListuZelja(p.Id, KonzolaUtils.Broj("ID knjige"));
            Console.WriteLine("  Knjiga dodata na listu želja.");
        }

        private void UkloniSaListeZelja()
        {
            Posetilac p = Izaberi();
            var naListi = _servis.GetListaZelja(p.Id);
            Console.WriteLine("Lista želja:");
            KonzolaUtils.Tabela(
                new[] { "ID", "ISBN", "Naziv" },
                naListi.Select(k => new[] { k.Id.ToString(), k.Sifra, k.Naziv }));
            if (naListi.Count == 0) return;

            int knjigaId = KonzolaUtils.Broj("ID knjige");
            if (!KonzolaUtils.Potvrda("Ukloniti knjigu sa liste želja?"))
                return;
            _servis.UkloniSaListeZelja(p.Id, knjigaId);
            Console.WriteLine("  Knjiga uklonjena sa liste želja.");
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