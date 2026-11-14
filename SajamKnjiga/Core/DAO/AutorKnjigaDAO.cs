using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Model;
using Core.Storage;

namespace Core.DAO
{
    public class AutorKnjigaDAO
    {
        private readonly Storage<AutorKnjiga> _storage;
        private readonly List<AutorKnjiga> _veze;

        public AutorKnjigaDAO()
        {
            _storage = new Storage<AutorKnjiga>("autor_knjiga.csv");
            _veze = _storage.Load();
        }

        public AutorKnjiga? Add(int autorId, int knjigaId)
        {
            if (Postoji(autorId, knjigaId))
            {
                return null;

            }
            AutorKnjiga veza = new AutorKnjiga(autorId, knjigaId);
            _veze.Add(veza);
            _storage.Save(_veze);
            return veza;
        }

        public AutorKnjiga? Remove(int autorId, int knjigaId) 
        {
            AutorKnjiga? veza = _veze.FirstOrDefault(v => v.AutorId == autorId && v.KnjigaId == knjigaId);
            if (veza == null)
            {
                return null;
            }
            _veze.Remove(veza);
            _storage.Save(_veze);
            return veza;
        }

        public int RemoveByAutorId(int autorId)
        {
            int obrisano = _veze.RemoveAll(v => v.AutorId == autorId);
            if (obrisano > 0)
            {
                _storage.Save(_veze);
            }
            return obrisano;
        }

        public int RemoveByKnjigaId(int knjigaId)
        {
            int obrisano = _veze.RemoveAll(v => v.KnjigaId == knjigaId);
            if (obrisano > 0)
            {
                _storage.Save(_veze);
            }
            return obrisano;
        }

        public bool Postoji(int autorId, int knjigaId)
        {
            return _veze.Any(v => v.AutorId == autorId && v.KnjigaId == knjigaId);
        }

        public List<int> GetKnjigeIds(int autorId)
        {
            return _veze.Where(v => v.AutorId == autorId).Select(v => v.KnjigaId).ToList();
        }

        public List<int> GetAutoriIds(int knjigaId)
        {
            return _veze.Where(v => v.KnjigaId == knjigaId).Select(v => v.AutorId).ToList();
        }

        public List<AutorKnjiga> GetAll()
        {
            return new List<AutorKnjiga>(_veze);
        }
    }
}
