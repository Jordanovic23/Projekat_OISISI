using System;
using System.Collections.Generic;
using System.Text;
using Core.Model;
using Core.Storage;

namespace Core.DAO
{
    public class PosetilacDAO
    {
        private readonly List<Posetilac> _posetioci;

        private readonly Storage<Posetilac> _storage;

        public PosetilacDAO() 
        {
            _storage = new Storage<Posetilac>("posetioci.csv");
            _posetioci = _storage.Load();
        }

        private int GenerateId()
        {
            if (_posetioci.Count == 0)
                return 0;
            else
                return _posetioci.Max(p => p.Id) + 1;
        }

        public Posetilac Add(Posetilac posetilac) 
        {
            posetilac.Id = GenerateId();
            _posetioci.Add(posetilac);
            _storage.Save(_posetioci);
            return posetilac;
        }

        public Posetilac? Update(Posetilac posetilac) 
        {
            Posetilac? stari = GetById(posetilac.Id);
            if(stari == null)
                return null;
            stari.Ime = posetilac.Ime;
            stari.Prezime = posetilac.Prezime;
            stari.DatumRodjenja = posetilac.DatumRodjenja;
            stari.AdresaId = posetilac.AdresaId;
            stari.KontaktTelefon = posetilac.KontaktTelefon;
            stari.Email = posetilac.Email;
            stari.BrojClanskeKarte = posetilac.BrojClanskeKarte;
            stari.TrenutnaGodinaClanstva = posetilac.TrenutnaGodinaClanstva;
            stari.Status = posetilac.Status;

            _storage.Save(_posetioci);
            return stari;
        }

        public Posetilac? Remove(int id) 
        {
            Posetilac? posetilac = GetById(id);
            if (posetilac == null)
            {
                return null;
            }
            _posetioci.Remove(posetilac);
            _storage.Save(_posetioci);
            return posetilac;
        }

        public Posetilac? GetById(int id)
        {
            return _posetioci.Find(p => p.Id == id);
        }

        public Posetilac? GetByBrojClanskeKarte(string brojClanskeKarte)
        {
            return _posetioci.Find(p => p.BrojClanskeKarte == brojClanskeKarte);
        }

        // Vraca kopiju liste posetilaca, da bi se izbeglo da neko spolja menja listu direktno
        public List<Posetilac> GetAll()
        {
            return _posetioci.ToList();
        }
    }
}
