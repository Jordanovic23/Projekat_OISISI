using Core.DAO;
using Core.Model;

namespace Core.Services
{
    public class IzdavacService
    {
        private readonly IzdavacDAO _izdavacDao;
        private readonly KnjigaDAO _knjigaDao;

        public IzdavacService(IzdavacDAO izdavacDao, KnjigaDAO knjigaDao)
        {
            _izdavacDao = izdavacDao;
            _knjigaDao = knjigaDao;
        }

        public List<Izdavac> GetAll() => _izdavacDao.GetAll();

        public Izdavac? GetById(int id) => _izdavacDao.GetById(id);

        public int BrojKnjiga(int izdavacId) => _knjigaDao.GetAll().Count(k => k.IzdavacId == izdavacId);

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

        // Referencijalna zavisnost: izdavac koji ima knjige ne moze se obrisati
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