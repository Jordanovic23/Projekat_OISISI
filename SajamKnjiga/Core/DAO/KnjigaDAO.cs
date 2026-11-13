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
        public Knjiga GetById(int id)
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
    }
}
