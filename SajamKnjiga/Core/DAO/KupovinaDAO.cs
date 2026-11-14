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


        private int GenerateId()
        {
            return _kupovine.Any() ? _kupovine.Max(k => k.Id) + 1 : 1;
        }
    }
}
