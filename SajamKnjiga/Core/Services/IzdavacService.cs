using Core.DAO;
using Core.Model;

namespace Core.Services
{
    public class IzdavacService
    {
        private readonly IzdavacDAO _izdavacDao;
        private readonly KnjigaDAO _knjigaDao;
        private readonly AutorDAO _autorDao;
        private readonly AutorKnjigaDAO _autorKnjigaDao;

        public IzdavacService(IzdavacDAO izdavacDao, KnjigaDAO knjigaDao, AutorDAO autorDao, AutorKnjigaDAO autorKnjigaDao)
        {
            _izdavacDao = izdavacDao;
            _knjigaDao = knjigaDao;
            _autorDao = autorDao;
            _autorKnjigaDao = autorKnjigaDao;
        }

        public List<Izdavac> GetAll() => _izdavacDao.GetAll();

        public Izdavac? GetById(int id) => _izdavacDao.GetById(id);

        public int BrojKnjiga(int izdavacId) => GetKnjige(izdavacId).Count;

        // Knjige koje je izdavac izdao
        public List<Knjiga> GetKnjige(int izdavacId)
        {
            return _knjigaDao.GetAll().Where(k => k.IzdavacId == izdavacId).ToList();
        }

        // Autori ciji su naslovi kod ovog izdavaca (svaki autor jednom, i ako ima vise knjiga)
        public List<Autor> GetAutori(int izdavacId)
        {
            return GetKnjige(izdavacId)
                .SelectMany(k => _autorKnjigaDao.GetAutoriIds(k.Id))
                .Distinct()
                .Select(id => _autorDao.GetById(id))
                .OfType<Autor>()
                .ToList();
        }

        // Sef izdavaca ili null ako ga nema
        public Autor? GetSef(Izdavac izdavac)
        {
            return izdavac.IdSefa == Izdavac.BEZ_SEFA ? null : _autorDao.GetById(izdavac.IdSefa);
        }

        // Novi izdavac nema sefa (sef se postavlja kasnije, kad izdavac dobije knjige)
        public Izdavac Add(Izdavac izdavac)
        {
            izdavac.IdSefa = Izdavac.BEZ_SEFA;
            return _izdavacDao.Add(izdavac)
                ?? throw new InvalidOperationException($"Izdavač sa šifrom {izdavac.Sifra} već postoji.");
        }

        public Izdavac Update(Izdavac izmenjen)
        {
            if (_izdavacDao.GetById(izmenjen.Id) == null)
                throw new InvalidOperationException("Izdavač nije pronađen.");
            return _izdavacDao.Update(izmenjen)
                ?? throw new InvalidOperationException($"Šifru {izmenjen.Sifra} već koristi drugi izdavač.");
        }

        // Sef moze biti samo autor sa najmanje 5 godina iskustva koji je izdavao knjige kod ovog izdavaca
        public void PostaviSefa(int izdavacId, int autorId)
        {
            if (_izdavacDao.GetById(izdavacId) == null)
                throw new InvalidOperationException("Izdavač nije pronađen.");
            Autor autor = _autorDao.GetById(autorId)
                ?? throw new InvalidOperationException("Autor nije pronađen.");
            if (autor.GodineIskustva < Izdavac.MIN_GODINA_ISKUSTVA)
                throw new InvalidOperationException(
                    $"Autor mora imati najmanje {Izdavac.MIN_GODINA_ISKUSTVA} godina iskustva (ima {autor.GodineIskustva}).");
            if (!GetAutori(izdavacId).Any(a => a.Id == autorId))
                throw new InvalidOperationException("Autor nije izdavao knjige kod ovog izdavača.");

            _izdavacDao.UpdateSef(izdavacId, autorId);
        }

        // Izdavac koji ima knjige ne moze se obrisati
        public Izdavac Remove(int id)
        {
            Izdavac izdavac = _izdavacDao.GetById(id)
                ?? throw new InvalidOperationException("Izdavač nije pronađen.");

            int brojKnjiga = BrojKnjiga(id);
            if (brojKnjiga > 0)
                throw new InvalidOperationException(
                    $"Izdavač ima {brojKnjiga} knjiga. Prvo obrišite knjige ili ih prebacite drugom izdavaču.");

            _izdavacDao.Remove(id);
            return izdavac;
        }
    }
}