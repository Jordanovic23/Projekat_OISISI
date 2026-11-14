using Core.DAO;
using Core.Model;

namespace Core.Services
{
    public class KnjigaService
    {
        private readonly KnjigaDAO _knjigaDao;
        private readonly AutorDAO _autorDao;
        private readonly IzdavacDAO _izdavacDao;
        private readonly AutorKnjigaDAO _autorKnjigaDao;
        private readonly KupovinaDAO _kupovinaDao;
        private readonly ListaZeljaDAO _listaZeljaDao;

        public KnjigaService(KnjigaDAO knjigaDao, AutorDAO autorDao, IzdavacDAO izdavacDao,
                             AutorKnjigaDAO autorKnjigaDao, KupovinaDAO kupovinaDao, ListaZeljaDAO listaZeljaDao)
        {
            _knjigaDao = knjigaDao;
            _autorDao = autorDao;
            _izdavacDao = izdavacDao;
            _autorKnjigaDao = autorKnjigaDao;
            _kupovinaDao = kupovinaDao;
            _listaZeljaDao = listaZeljaDao;
        }

        public List<Knjiga> GetAll() => _knjigaDao.GetAll();

        public Knjiga? GetById(int id) => _knjigaDao.GetById(id);

        // Autori knjige se citaju iz spojne tabele AutorKnjiga
        public List<Autor> GetAutori(int knjigaId)
        {
            return _autorKnjigaDao.GetAutoriIds(knjigaId)
                .Select(id => _autorDao.GetById(id))
                .OfType<Autor>()
                .ToList();
        }

        public Knjiga Add(Knjiga knjiga)
        {
            if (_izdavacDao.GetById(knjiga.IzdavacId) == null)
                throw new InvalidOperationException("Izdavač sa tim ID-jem ne postoji.");
            return _knjigaDao.Add(knjiga)
                ?? throw new InvalidOperationException("Knjiga sa tim ISBN-om već postoji.");
        }

        public Knjiga Update(Knjiga izmenjena)
        {
            if (_knjigaDao.GetById(izmenjena.Id) == null)
                throw new InvalidOperationException("Knjiga nije pronađena.");
            if (_izdavacDao.GetById(izmenjena.IzdavacId) == null)
                throw new InvalidOperationException("Izdavač sa tim ID-jem ne postoji.");
            return _knjigaDao.Update(izmenjena)
                ?? throw new InvalidOperationException("Taj ISBN već koristi druga knjiga.");
        }

        // Referencijalne zavisnosti: veze sa autorima, kupovine i stavke liste zelja
        public Knjiga Remove(int id)
        {
            Knjiga knjiga = _knjigaDao.GetById(id)
                ?? throw new InvalidOperationException("Knjiga nije pronađena.");

            _autorKnjigaDao.RemoveByKnjigaId(id);
            _kupovinaDao.RemoveByKnjiga(id);
            _listaZeljaDao.RemoveByKnjigaId(id);
            _knjigaDao.Remove(id);
            return knjiga;
        }
    }
}