using Core.DAO;
using Core.Model;

namespace Core.Services
{
    public class PosetilacService
    {
        private readonly PosetilacDAO _posetilacDao;
        private readonly AdresaDAO _adresaDao;
        private readonly KupovinaDAO _kupovinaDao;
        private readonly ListaZeljaDAO _listaZeljaDao;

        public PosetilacService(PosetilacDAO posetilacDao, AdresaDAO adresaDao,
                                KupovinaDAO kupovinaDao, ListaZeljaDAO listaZeljaDao)
        {
            _posetilacDao = posetilacDao;
            _adresaDao = adresaDao;
            _kupovinaDao = kupovinaDao;
            _listaZeljaDao = listaZeljaDao;
        }

        public List<Posetilac> GetAll() => _posetilacDao.GetAll();

        public Posetilac? GetById(int id) => _posetilacDao.GetById(id);

        public Adresa? GetAdresa(Posetilac posetilac) => _adresaDao.GetById(posetilac.AdresaId);

        // Veza posetilac-adresa je 1-1 (dozvoljeno po FAQ), pa se adresa pravi zajedno sa posetiocem
        public Posetilac Add(Posetilac posetilac, Adresa adresa)
        {
            if (_posetilacDao.GetByBrojClanskeKarte(posetilac.BrojClanskeKarte) != null)
                throw new InvalidOperationException($"Posetilac sa brojem karte {posetilac.BrojClanskeKarte} već postoji.");

            Adresa sacuvana = _adresaDao.Add(adresa)!;
            posetilac.AdresaId = sacuvana.Id;
            return _posetilacDao.Add(posetilac)!;
        }

        public Posetilac Update(Posetilac izmenjen, Adresa adresa)
        {
            Posetilac postojeci = _posetilacDao.GetById(izmenjen.Id)
                ?? throw new InvalidOperationException("Posetilac nije pronađen.");

            Posetilac? istaKarta = _posetilacDao.GetByBrojClanskeKarte(izmenjen.BrojClanskeKarte);
            if (istaKarta != null && istaKarta.Id != izmenjen.Id)
                throw new InvalidOperationException("Taj broj članske karte već koristi drugi posetilac.");

            adresa.Id = postojeci.AdresaId;
            Adresa sacuvana = _adresaDao.Update(adresa) ?? _adresaDao.Add(adresa)!;
            izmenjen.AdresaId = sacuvana.Id;
            return _posetilacDao.Update(izmenjen)!;
        }

        // Referencijalne zavisnosti: brisu se kupovine, lista zelja i adresa posetioca
        public Posetilac Remove(int id)
        {
            Posetilac posetilac = _posetilacDao.GetById(id)
                ?? throw new InvalidOperationException("Posetilac nije pronađen.");

            _kupovinaDao.RemoveByPosetilacId(id);
            _listaZeljaDao.RemoveByPosetilacId(id);
            _adresaDao.Remove(posetilac.AdresaId);
            _posetilacDao.Remove(id);
            return posetilac;
        }
    }
}