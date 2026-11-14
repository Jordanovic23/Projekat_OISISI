using System.Globalization;

namespace ConsoleClient.Utils
{
    // Pomocne metode za unos i ispis u konzoli.
    //
    // Izvor -> ideja za bezbedan unos preuzeta iz materijala sa vezbi, klasa ConsoleViewUtils
    // (metoda SafeInputInt) iz primera CRUDExample, gde se unos ponavlja dok korisnik
    // ne unese ispravnu vrednost. Taj pristup je prosiren na tekst, decimalne brojeve,
    // datume i enume, a dodati su i ispis tabele i petlja menija.
    public static class KonzolaUtils
    {
        // Ako se ulaz zatvori, ne vrtimo petlju unosa unedogled
        public static string Citaj() => Console.ReadLine() ?? throw new EndOfStreamException();

        // trenutno != null -> izmena: prazan unos zadrzava staru vrednost
        public static string Tekst(string poruka, string? trenutno = null)
        {
            while (true)
            {
                Console.Write(trenutno == null ? $"{poruka}: " : $"{poruka} [{trenutno}]: ");
                string unos = Citaj().Trim();
                if (unos == "" && trenutno != null) return trenutno;
                if (unos == "") { Console.WriteLine("  Polje ne sme biti prazno."); continue; }
                if (unos.Contains(';')) { Console.WriteLine("  Znak ';' nije dozvoljen."); continue; }
                return unos;
            }
        }

        public static int Broj(string poruka, int? trenutno = null)
        {
            while (true)
            {
                Console.Write(trenutno == null ? $"{poruka}: " : $"{poruka} [{trenutno}]: ");
                string unos = Citaj().Trim();
                if (unos == "" && trenutno != null) return trenutno.Value;
                if (int.TryParse(unos, out int broj) && broj >= 0) return broj;
                Console.WriteLine("  Unesite ceo broj (0 ili veći).");
            }
        }

        public static decimal Decimalni(string poruka, decimal? trenutno = null)
        {
            while (true)
            {
                Console.Write(trenutno == null ? $"{poruka}: " : $"{poruka} [{trenutno}]: ");
                string unos = Citaj().Trim().Replace(',', '.');
                if (unos == "" && trenutno != null) return trenutno.Value;
                if (decimal.TryParse(unos, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal broj) && broj > 0)
                    return broj;
                Console.WriteLine("  Unesite broj veći od 0.");
            }
        }

        public static DateOnly Datum(string poruka, DateOnly? trenutno = null)
        {
            while (true)
            {
                Console.Write(trenutno == null ? $"{poruka} (dd.MM.yyyy): " : $"{poruka} [{trenutno:dd.MM.yyyy}]: ");
                string unos = Citaj().Trim();
                if (unos == "" && trenutno != null) return trenutno.Value;
                if (DateOnly.TryParseExact(unos, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly datum)
                    && datum <= DateOnly.FromDateTime(DateTime.Today))
                    return datum;
                Console.WriteLine("  Datum mora biti u formatu dd.MM.yyyy i ne sme biti u budućnosti.");
            }
        }

        // Izbor vrednosti enuma (npr. status posetioca). Enter zadrzava staru vrednost pri izmeni.
        public static T Izbor<T>(string poruka, T? trenutno = null) where T : struct, Enum
        {
            T[] vrednosti = Enum.GetValues<T>();
            while (true)
            {
                string opcije = string.Join(" / ", vrednosti.Select((v, i) => $"{i + 1}={v}"));
                Console.Write(trenutno == null ? $"{poruka} ({opcije}): " : $"{poruka} ({opcije}) [{trenutno}]: ");
                string unos = Citaj().Trim();
                if (unos == "" && trenutno != null) return trenutno.Value;
                if (int.TryParse(unos, out int redni) && redni >= 1 && redni <= vrednosti.Length)
                    return vrednosti[redni - 1];
                Console.WriteLine("  Unesite redni broj opcije.");
            }
        }

        public static bool Potvrda(string poruka)
        {
            Console.Write($"{poruka} (d/n): ");
            return Citaj().Trim().ToLower() is "d" or "da";
        }

        // Tabela sa kolonama poravnatim prema najduzem sadrzaju
        public static void Tabela(string[] zaglavlja, IEnumerable<string[]> redovi)
        {
            var sviRedovi = redovi.ToList();
            if (sviRedovi.Count == 0) { Console.WriteLine("  (nema podataka)"); return; }

            int[] sirine = zaglavlja.Select(z => z.Length).ToArray();
            foreach (var red in sviRedovi)
                for (int i = 0; i < sirine.Length; i++)
                    sirine[i] = Math.Max(sirine[i], red[i].Length);

            string Linija(string[] k) => "| " + string.Join(" | ", k.Select((x, i) => x.PadRight(sirine[i]))) + " |";
            string razdelnik = "+-" + string.Join("-+-", sirine.Select(s => new string('-', s))) + "-+";

            Console.WriteLine(razdelnik);
            Console.WriteLine(Linija(zaglavlja));
            Console.WriteLine(razdelnik);
            foreach (var red in sviRedovi) Console.WriteLine(Linija(red));
            Console.WriteLine(razdelnik);
        }

        // Petlja menija; poslovne greske iz servisa se prikazuju, aplikacija ne pada
        public static void Meni(string naslov, params (string Naziv, Action Akcija)[] opcije)
        {
            while (true)
            {
                Console.WriteLine($"\n=== {naslov} ===");
                for (int i = 0; i < opcije.Length; i++)
                    Console.WriteLine($"{i + 1}: {opcije[i].Naziv}");
                Console.WriteLine("0: Nazad / izlaz");
                Console.Write("Izbor: ");

                string unos = Citaj().Trim();
                if (unos == "0") return;
                if (!int.TryParse(unos, out int izbor) || izbor < 1 || izbor > opcije.Length)
                {
                    Console.WriteLine("  Nepoznata opcija.");
                    continue;
                }
                try { opcije[izbor - 1].Akcija(); }
                catch (InvalidOperationException ex) { Console.WriteLine($"  Greška: {ex.Message}"); }
            }
        }
    }
}