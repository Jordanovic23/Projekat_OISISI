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

        public ListaZelja? Add(ListaZelja novaListaZelja)
        {
            bool postoji = _listaZelja.Any(l => l.PosetilacId == novaListaZelja.PosetilacId && l.KnjigaId == novaListaZelja.KnjigaId);
            if (postoji)
            {
                return null;
            }
            novaListaZelja.Id = GenerateId();
            _listaZelja.Add(novaListaZelja);
            _storage.Save(_listaZelja);
            return novaListaZelja;
        }

        private int GenerateId()
        {
            if (_listaZelja.Count == 0)
                return 1;
            else
                return _listaZelja.Max(l => l.Id) + 1;
        }



    }
}
