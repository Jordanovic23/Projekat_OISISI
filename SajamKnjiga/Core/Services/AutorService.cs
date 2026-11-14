using Core.DAO;
using Core.Model;

namespace Core.Services
{
    public class AutorService
    {
        private readonly AutorDAO _autorDao;
        private readonly AdresaDAO _adresaDao;
        private readonly AutorKnjigaDAO _autorKnjigaDao;
        private readonly IzdavacDAO _izdavacDao;

        public AutorService(AutorDAO autorDao, AdresaDAO adresaDao,
                            AutorKnjigaDAO autorKnjigaDao, IzdavacDAO izdavacDao)
        {
            _autorDao = autorDao;
            _adresaDao = adresaDao;
            _autorKnjigaDao = autorKnjigaDao;
            _izdavacDao = izdavacDao;
        }

        public List<Autor> GetAll() => _autorDao.GetAll();

        public Autor? GetById(int id) => _autorDao.GetById(id);

        public Adresa? GetAdresa(Autor autor) => _adresaDao.GetById(autor.AdresaId);

        public int BrojKnjiga(int autorId) => _autorKnjigaDao.GetKnjigeIds(autorId).Count;

        // Veza autor-adresa je 1-1, pa se adresa pravi zajedno sa autorom
        public Autor Add(Autor autor, Adresa adresa)
        {
            if (_autorDao.GetAll().Any(a => a.BrojLK == autor.BrojLK))
                throw new InvalidOperationException("Autor sa tim brojem lične karte već postoji.");

            Adresa sacuvana = _adresaDao.Add(adresa)!;
            autor.AdresaId = sacuvana.Id;
            return _autorDao.Add(autor)!;
        }

        public Autor Update(Autor izmenjen, Adresa adresa)
        {
            Autor postojeci = _autorDao.GetById(izmenjen.Id)
                ?? throw new InvalidOperationException("Autor nije pronađen.");
            if (_autorDao.GetAll().Any(a => a.BrojLK == izmenjen.BrojLK && a.Id != izmenjen.Id))
                throw new InvalidOperationException("Taj broj lične karte već koristi drugi autor.");

            adresa.Id = postojeci.AdresaId;
            Adresa sacuvana = _adresaDao.Update(adresa) ?? _adresaDao.Add(adresa)!;
            izmenjen.AdresaId = sacuvana.Id;
            return _autorDao.Update(izmenjen)!;
        }

        // Referencijalne zavisnosti: veze sa knjigama, status sefa izdavaca i adresa.
        // Knjige ostaju u sistemu (spec dozvoljava knjigu bez autora).
        public Autor Remove(int id)
        {
            Autor autor = _autorDao.GetById(id)
                ?? throw new InvalidOperationException("Autor nije pronađen.");

            _autorKnjigaDao.RemoveByAutorId(id);
            _izdavacDao.ResetSefByAutorId(id);
            _adresaDao.Remove(autor.AdresaId);
            _autorDao.Remove(id);
            return autor;
        }
    }
}