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
        private readonly KnjigaDAO _knjigaDao;

        public PosetilacService(PosetilacDAO posetilacDao, AdresaDAO adresaDao,
                                KupovinaDAO kupovinaDao, ListaZeljaDAO listaZeljaDao,KnjigaDAO knjigaDAO)
        {
            _posetilacDao = posetilacDao;
            _adresaDao = adresaDao;
            _kupovinaDao = kupovinaDao;
            _listaZeljaDao = listaZeljaDao;
            _knjigaDao = knjigaDAO;
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

        // Kupovine posetioca
        public List<Kupovina> GetKupovine(int posetilacId) => _kupovinaDao.GetByPosetilacId(posetilacId);

        // Prosecna ocena svih recenzija posetioca; null ako nema kupovina
        public double? ProsecnaOcena(int posetilacId)
        {
            List<Kupovina> kupovine = GetKupovine(posetilacId);
            return kupovine.Count == 0 ? null : kupovine.Average(k => (int)k.Ocena);
        }

        // Knjige sa liste zelja posetioca
        public List<Knjiga> GetListaZelja(int posetilacId)
        {
            return _listaZeljaDao.GetByPosetilacId(posetilacId)
                .Select(z => _knjigaDao.GetById(z.KnjigaId))
                .OfType<Knjiga>()
                .ToList();
        }

        // Knjige koje posetilac moze da doda na listu zelja (nije ih kupio i nisu vec na listi)
        public List<Knjiga> GetKnjigeZaListuZelja(int posetilacId)
        {
            var kupljene = GetKupovine(posetilacId).Select(k => k.KnjigaId).ToHashSet();
            var naListi = _listaZeljaDao.GetByPosetilacId(posetilacId).Select(z => z.KnjigaId).ToHashSet();
            return _knjigaDao.GetAll().Where(k => !kupljene.Contains(k.Id) && !naListi.Contains(k.Id)).ToList();
        }

        public void DodajNaListuZelja(int posetilacId, int knjigaId)
        {
            if (_posetilacDao.GetById(posetilacId) == null)
                throw new InvalidOperationException("Posetilac nije pronađen.");
            if (_knjigaDao.GetById(knjigaId) == null)
                throw new InvalidOperationException("Knjiga nije pronađena.");
            if (GetKupovine(posetilacId).Any(k => k.KnjigaId == knjigaId))
                throw new InvalidOperationException("Posetilac je već kupio ovu knjigu.");
            if (_listaZeljaDao.Add(new ListaZelja { PosetilacId = posetilacId, KnjigaId = knjigaId }) == null)
                throw new InvalidOperationException("Knjiga je već na listi želja.");
        }

        public void UkloniSaListeZelja(int posetilacId, int knjigaId)
        {
            if (_listaZeljaDao.Remove(posetilacId, knjigaId) == null)
                throw new InvalidOperationException("Knjiga nije na listi želja ovog posetioca.");
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