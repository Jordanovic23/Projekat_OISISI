    using System;
    using System.Collections.Generic;
    using System.Text;
    using Core.Model;
    using Core.Storage;

    namespace Core.DAO
    {
        public class IzdavacDAO
        {
            private readonly List<Izdavac> _izdavaci;
            private readonly Storage<Izdavac> _storage;

            public IzdavacDAO()
            {
                _storage = new Storage<Izdavac>("izdavaci.csv");
                _izdavaci = _storage.Load();
            }

            private int GenerateId()
            {
                if (_izdavaci.Count == 0)
                    return 1;
                else
                    return _izdavaci.Max(i => i.Id) + 1;
            }

            public List<Izdavac> GetAll()
            {
                return new List<Izdavac>(_izdavaci);
            }

            public Izdavac? GetById(int id)
            {
                return _izdavaci.FirstOrDefault(i => i.Id == id);
            }

            public Izdavac? GetBySifra(string sifra) 
            {
                return _izdavaci.FirstOrDefault(i => i.Sifra == sifra);
            }

            public Izdavac? Add(Izdavac izdavac)
            {
                bool postoji = _izdavaci.Any(i => i.Sifra == izdavac.Sifra);
                if (postoji)
                {
                    return null;
                }
                izdavac.Id = GenerateId();
                _izdavaci.Add(izdavac);
                _storage.Save(_izdavaci);
                return izdavac;
            }

            public Izdavac? Update(Izdavac izdavac)
            {
                Izdavac? stari = GetById(izdavac.Id);
                if (stari == null)
                    return null;
                bool zauzetaSifra = _izdavaci.Any(i => i.Sifra == izdavac.Sifra && i.Id != izdavac.Id);
                if(zauzetaSifra)
                {
                    return null;
                }
                stari.Sifra = izdavac.Sifra;
                stari.Naziv = izdavac.Naziv;
                _storage.Save(_izdavaci);
                return stari;
            }

            public Izdavac? UpdateSef(int izdavacId, int noviSefId) 
            {
                Izdavac? izdavac = GetById(izdavacId);
                if (izdavac == null)
                {
                    return null;
                }
                izdavac.IdSefa = noviSefId;
                _storage.Save(_izdavaci);
                return izdavac;
            }

            public int ResetSefByAutorId(int autorId) 
            {
                List<Izdavac> pogodjeni = _izdavaci.Where(i => i.IdSefa == autorId).ToList();
                foreach (Izdavac izdavac in pogodjeni) 
                {
                    izdavac.IdSefa = Izdavac.BEZ_SEFA;
                    izdavac.Sef = null;
                }
                if (pogodjeni.Count > 0)
                {
                    _storage.Save(_izdavaci);
                }
                return pogodjeni.Count;
            }

            public Izdavac? Remove(int id) 
            {
                Izdavac? izdavac = GetById(id);
                if (izdavac == null)
                {
                    return null;
                }
                _izdavaci.Remove(izdavac);
                _storage.Save(_izdavaci);
                return izdavac;
            }

        }
    }
