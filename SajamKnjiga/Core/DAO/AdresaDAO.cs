using System.Collections.Generic;
using System.Linq;
using Core.Model;
using Core.Storage;

namespace Core.DAO
{
    public class AdresaDAO
    {
        private readonly Storage<Adresa> _storage;
        private readonly List<Adresa> _adrese;

        public AdresaDAO()
        {
            _storage = new Storage<Adresa>("adrese.csv");
            _adrese = _storage.Load();
        }

        public List<Adresa> GetAll()
        {
            return new List<Adresa>(_adrese);
        }

        public Adresa? GetById(int id)
        {
            return _adrese.FirstOrDefault(a => a.Id == id);
        }

        public Adresa? Add(Adresa novaAdresa)
        {
            novaAdresa.Id = GenerateId();
            _adrese.Add(novaAdresa);
            _storage.Save(_adrese);
            return novaAdresa;
        }

        public Adresa? Update(Adresa izmenjenaAdresa)
        {
            var postojeca = GetById(izmenjenaAdresa.Id);
            if (postojeca == null)
            {
                return null;
            }

            postojeca.Ulica = izmenjenaAdresa.Ulica;
            postojeca.Broj = izmenjenaAdresa.Broj;
            postojeca.Grad = izmenjenaAdresa.Grad;
            postojeca.Drzava = izmenjenaAdresa.Drzava;

            _storage.Save(_adrese);
            return postojeca;
        }

        public Adresa? Remove(int id)
        {
            var adresa = GetById(id);
            if (adresa == null)
            {
                return null;
            }

            _adrese.Remove(adresa);
            _storage.Save(_adrese);
            return adresa;
        }

        private int GenerateId()
        {
            return _adrese.Any() ? _adrese.Max(a => a.Id) + 1 : 1;
        }
    }
}