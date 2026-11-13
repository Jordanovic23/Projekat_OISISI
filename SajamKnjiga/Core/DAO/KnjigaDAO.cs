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

        private int GenerateId()
        {
            return _knjige.Any() ? _knjige.Max(k => k.Id) + 1 : 1;
        }
    }
}
