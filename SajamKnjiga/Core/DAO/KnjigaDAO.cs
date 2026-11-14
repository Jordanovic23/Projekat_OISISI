using System.Collections.Generic;
using System.Linq;
using Core.Model;
using Core.Storage;

namespace Core.DAO
{
    public class KnjigaDAO
    {
        private readonly Storage<Knjiga> _storage;
        private readonly List<Knjiga> _knjige;

        public KnjigaDAO()
        {
            _storage = new Storage<Knjiga>("knjige.csv");
            _knjige = _storage.Load();
        }
        public List<Knjiga> GetAll()
        {
            return new List<Knjiga>(_knjige);
        }
        public Knjiga? GetById(int id)
        {
            return _knjige.FirstOrDefault(k => k.Id == id);
        }
        public bool Delete(int id)
        {
            var knjiga = GetById(id);
            if (knjiga == null)
            {
                return false;
            }
            _knjige.Remove(knjiga);
            _storage.Save(_knjige);
            return true;
        }

        public Knjiga? Add(Knjiga novaKnjiga)
        {
            // Proveravamo da li već postoji knjiga sa istim ISBN-om
            bool postoji = _knjige.Any(k => k.Sifra == novaKnjiga.Sifra);
            if (postoji)
            {
                // Već postoji, ne dozvoljavamo duplikat
                return null;
            }
            // Ako ne postoji, dodeljujemo ID i čuvamo
            novaKnjiga.Id = GenerateId();
            _knjige.Add(novaKnjiga);
            _storage.Save(_knjige);
            return novaKnjiga;
        }

        public Knjiga? Update(Knjiga izmenjenaKnjiga)
        {
            var postojeca = GetById(izmenjenaKnjiga.Id);
            if (postojeca == null)
            {
                return null;
            }
            postojeca.Sifra = izmenjenaKnjiga.Sifra;
            postojeca.Naziv = izmenjenaKnjiga.Naziv;
            postojeca.GodinaIzdanja = izmenjenaKnjiga.GodinaIzdanja;
            postojeca.IzdavacId = izmenjenaKnjiga.IzdavacId;
            postojeca.Zanr= izmenjenaKnjiga.Zanr;
            postojeca.Cena = izmenjenaKnjiga.Cena;
            postojeca.BrojStrana = izmenjenaKnjiga.BrojStrana;
            _storage.Save(_knjige);
            return postojeca;
        }


        // Na isti nacin kao i kod autora sortirano po ugledu na vezbe

        public List<Knjiga> GetSorted(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
            IEnumerable<Knjiga> knjige = _knjige;

            // Sortiranje prema prosleđenom kriterijumu
            switch (sortCriteria)
            {
                case "Id":
                    knjige = _knjige.OrderBy(k => k.Id);
                    break;
                case "Naziv":
                    knjige = _knjige.OrderBy(k => k.Naziv);
                    break;
                case "Zanr":
                    knjige = _knjige.OrderBy(k => k.Zanr);
                    break;
                default:
                    knjige = _knjige.OrderBy(k => k.Id);
                    break;
            }

            // Promena smera ukoliko je opadajući
            if (sortDirection == SortDirection.Descending)
            {
                knjige = knjige.Reverse();
            }

            // Paginacija
            knjige = knjige.Skip((page - 1) * pageSize).Take(pageSize);

            return knjige.ToList();
        }



        private int GenerateId()
        {
            return _knjige.Any() ? _knjige.Max(k => k.Id) + 1 : 1;
        }
    }
}
