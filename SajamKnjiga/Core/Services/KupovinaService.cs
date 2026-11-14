using Core.DAO;
using Core.Model;

namespace Core.Services
{
    public class KupovinaService
    {
        private readonly KupovinaDAO _kupovinaDao;
        private readonly PosetilacDAO _posetilacDao;
        private readonly KnjigaDAO _knjigaDao;

        public KupovinaService(KupovinaDAO kupovinaDao, PosetilacDAO posetilacDao, KnjigaDAO knjigaDao)
        {
            _kupovinaDao = kupovinaDao;
            _posetilacDao = posetilacDao;
            _knjigaDao = knjigaDao;
        }

        public List<Kupovina> GetAll() => _kupovinaDao.GetAll();

        public Kupovina? GetById(int id) => _kupovinaDao.GetById(id);

        // Pomocno za prikaz: ime posetioca i naziv knjige umesto samo ID-jeva
        public string ImePosetioca(int posetilacId)
        {
            Posetilac? p = _posetilacDao.GetById(posetilacId);
            return p == null ? "-" : $"{p.Ime} {p.Prezime}";
        }

        public string NazivKnjige(int knjigaId) => _knjigaDao.GetById(knjigaId)?.Naziv ?? "-";

        public Kupovina Add(Kupovina kupovina)
        {
            if (_posetilacDao.GetById(kupovina.PosetilacId) == null)
                throw new InvalidOperationException("Posetilac sa tim ID-jem ne postoji.");
            if (_knjigaDao.GetById(kupovina.KnjigaId) == null)
                throw new InvalidOperationException("Knjiga sa tim ID-jem ne postoji.");
            return _kupovinaDao.Add(kupovina);
        }

        // Menjaju se samo datum, ocena i komentar (posetilac i knjiga ostaju isti)
        public Kupovina Update(Kupovina izmenjena)
        {
            return _kupovinaDao.Update(izmenjena)
                ?? throw new InvalidOperationException("Kupovina nije pronađena.");
        }

        public Kupovina Remove(int id)
        {
            return _kupovinaDao.Remove(id)
                ?? throw new InvalidOperationException("Kupovina nije pronađena.");
        }
    }
}