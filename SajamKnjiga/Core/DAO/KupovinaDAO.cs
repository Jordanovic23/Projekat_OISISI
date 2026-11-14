using System.Collections.Generic;
using System.Linq;
using Core.Model;
using Core.Storage;

namespace Core.DAO
{
    public class KupovinaDAO
    {
        private readonly Storage<Kupovina> _storage;
        private readonly List<Kupovina> _kupovine;


        public KupovinaDAO()
        {
            _storage = new Storage<Kupovina>("kupovina.csv");
            _kupovine = _storage.Load();
        }
        public List<Kupovina> GetAll()
        {
            return new List<Kupovina>(_kupovine);
        }

        public List<Kupovina> GetByPosetilacId(int posetilacId)
        {
            return _kupovine
                .Where(k => k.PosetilacId == posetilacId)
                .ToList();
        }

        public Kupovina Add(Kupovina novaKupovina)
        {
            novaKupovina.Id = GenerateId();
            _kupovine.Add(novaKupovina);
            _storage.Save(_kupovine);
            return novaKupovina;
        }

        public Kupovina? Remove(int kupovinaId) 
        {
            Kupovina? kupovina = GetById(kupovinaId);
            if (kupovina == null) 
            {
                return null;
            }
            _kupovine.Remove(kupovina);
            _storage.Save(_kupovine);
            return kupovina;
        }

        public int RemoveByPosetilac(int posetilacId) 
        {
            int obrisano = _kupovine.RemoveAll(k => k.PosetilacId == posetilacId);
            if(obrisano > 0)
            {
                _storage.Save(_kupovine);
            }
            return obrisano;
        }

        public int RemoveByKnjiga(int knjigaId)
        {
            int obrisano = _kupovine.RemoveAll(k => k.KnjigaId == knjigaId);
            if (obrisano > 0)
            {
                _storage.Save(_kupovine);
            }
            return obrisano;
        }

        private int GenerateId()
        {
            return _kupovine.Any() ? _kupovine.Max(k => k.Id) + 1 : 1;
        }

        public Kupovina? GetById(int id)
        {
            return _kupovine.FirstOrDefault(k => k.Id == id);
        }
        public Kupovina? Update(Kupovina izmenjenaKupovina)
        {
            Kupovina? postojeca = GetById(izmenjenaKupovina.Id);
            if (postojeca == null)
            {
                return null;
            }
            postojeca.DatumKupovine = izmenjenaKupovina.DatumKupovine;
            postojeca.Ocena = izmenjenaKupovina.Ocena;
            postojeca.Komentar = izmenjenaKupovina.Komentar;
            _storage.Save(_kupovine);
            return postojeca;
        }

        public List<Kupovina> GetByKnjigaId(int knjigaId)
        {
            return _kupovine
                .Where(k => k.KnjigaId == knjigaId)
                .ToList();
        }

    }
}
