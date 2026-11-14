using System.Collections.Generic;
using System.Linq;
using Core.Model;
using Core.Storage;


namespace Core.DAO
{
    public class ListaZeljaDAO
    {
        private readonly Storage<ListaZelja> _storage;
        private readonly List<ListaZelja> _listaZelja;

        public ListaZeljaDAO()
        {
            _storage = new Storage<ListaZelja>("lista_zelja.csv");
            _listaZelja = _storage.Load();
        }

        public List<ListaZelja> GetAll()
        {
            return new List<ListaZelja>(_listaZelja);
        }

        public ListaZelja GetById(int id)
        {
            return _listaZelja.FirstOrDefault(l => l.Id == id);
        }

        public List<ListaZelja> GetByPosetilacId(int posetilacId)
        {
            return _listaZelja
                .Where(l => l.PosetilacId == posetilacId)
                .ToList();
        }

        public ListaZelja? Remove(int id)
        {
            ListaZelja? lista_zelja = GetById(id);
            if(lista_zelja == null)
            {
                return null;
            }
            _listaZelja.Remove(lista_zelja);
            _storage.Save(_listaZelja);
            return lista_zelja;
        } 

    }
}
