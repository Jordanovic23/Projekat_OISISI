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
        private readonly Storage<Autor> storage;
        private List<Autor> autori;

        public AutorDAO()
        {
            storage = new Storage<Autor>("autori.csv");
            autori = storage.Load();
        }
        public List<Autor> GetSviAutori()
        {
            return autori;
        }
        public Autor NadjiPoId(int id)
        {
            return autori.FirstOrDefault(a => a.Id == id);
        }

        public bool DodajAutora(Autor noviAutor)
        {
            // Proveravamo da li već postoji autor sa istim brojem lične karte
            bool postoji = autori.Any(a => a.BrojLK == noviAutor.BrojLK);

            if (postoji)
            {
                // Već postoji, ne dozvoljavamo duplikat
                return false;
            }

            // Ako ne postoji, dodeljujemo ID i čuvamo
            noviAutor.Id = GenerisiId();
            autori.Add(noviAutor);
            storage.Save(autori);
            return true;
        }

        public void IzmeniAutora(Autor izmenjeniAutor)
        {
            var postojeci = NadjiPoId(izmenjeniAutor.Id);
            if(postojeci != null)
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
                storage.Save(autori);
            }
        }
        public void ObrisiAutora(int id)
        {
            var aut = NadjiPoId(id);
            if(aut!= null)
            {
                autori.Remove(aut);
                storage.Save(autori);
            }
        }


        public int GenerisiId()
        {
            return autori.Any() ? autori.Max(a => a.Id) + 1 : 1;
        }
    }
}
