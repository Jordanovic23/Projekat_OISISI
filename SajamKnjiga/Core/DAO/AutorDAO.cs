using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Core.Model;
using Core.Storage;

namespace Core.DAO
{
    public class AutorDAO
    {
        private readonly Storage<Autor> _storage;
        private readonly List<Autor> _autori;

        public AutorDAO()
        {
            _storage = new Storage<Autor>("autori.csv");
            _autori = _storage.Load();
        }
        public List<Autor> GetAll()
        {
            return _autori.ToList();
        }
        public Autor FindById(int id)
        {
            return _autori.FirstOrDefault(a => a.Id == id);
        }

        public bool Add(Autor noviAutor)
        {
            // Proveravamo da li već postoji autor sa istim brojem lične karte
            bool postoji = _autori.Any(a => a.BrojLK == noviAutor.BrojLK);

            if (postoji)
            {
                // Već postoji, ne dozvoljavamo duplikat
                return false;
            }

            // Ako ne postoji, dodeljujemo ID i čuvamo
            noviAutor.Id = GenerateId();
            _autori.Add(noviAutor);
            _storage.Save(_autori);
            return true;
        }

        public Autor? Update(Autor izmenjeniAutor)
        {
            var postojeci = FindById(izmenjeniAutor.Id);

            if (postojeci != null)
            {
                postojeci.Id = izmenjeniAutor.Id;
                postojeci.Ime = izmenjeniAutor.Ime;
                postojeci.Prezime = izmenjeniAutor.Prezime;
                postojeci.DatumRodjenja = izmenjeniAutor.DatumRodjenja;
                postojeci.AdresaId = izmenjeniAutor.AdresaId;
                postojeci.Telefon = izmenjeniAutor.Telefon;
                postojeci.Email = izmenjeniAutor.Email;
                postojeci.BrojLK = izmenjeniAutor.BrojLK;
                postojeci.GodineIskustva = izmenjeniAutor.GodineIskustva;
                _storage.Save(_autori);
            }
            return postojeci;
            
        }
        public void Remove(int id)
        {
            var aut = FindById(id);
            if(aut!= null)
            {
                _autori.Remove(aut);
                _storage.Save(_autori);
            }
        }


        private int GenerateId()
        {
            return _autori.Any() ? _autori.Max(a => a.Id) + 1 : 1;
        }
    }
}
