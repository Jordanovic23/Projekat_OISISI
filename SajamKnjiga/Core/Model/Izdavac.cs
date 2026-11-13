using System;
using System.Collections.Generic;
using System.Text;
using Core.Storage.Serialization;

namespace Core.Model
{
    public class Izdavac : ISerializable
    {
        public const int BEZ_SEFA = -1;
        public const int MIN_GODINA_ISKUSTVA = 5;

        public int Id { get; set; }
        public string Sifra { get; set; } = "";
        public string Naziv { get; set; } = "";

        // Serijalizuje se. BEZ_SEFA (-1) znaci da izdavac jos nema sefa,
        // jer je 0 validan Id koji GenerateId() dodeljuje prvom entitetu.
        public int IdSefa { get; set; } = BEZ_SEFA;
        public Autor? Sef { get; set; }

        public List<Knjiga> SpisakKnjiga { get; set; } = new List<Knjiga>();

        // Izvodi se, nigde se ne cuva, racuna sa na osnovu SpiskaKnjiga svaki put kad se pozove
        // DistinctBy da bi se izbeglo dupliranje autora koji su napisali vise knjiga iz SpiskaKnjiga
        public List<Autor> ListaAutora => 
            SpisakKnjiga.SelectMany(k => k.Autori).DistinctBy(a => a.Id).ToList();

        public Izdavac() { }

        public Izdavac(int id, string sifra, string naziv, int idSefa = BEZ_SEFA)
        {
            Id = id;
            Sifra = sifra;
            Naziv = naziv;
            IdSefa = idSefa;
        }

        public bool MozeBitiSef(Autor autor) 
        {
            return autor.GodineIskustva >= MIN_GODINA_ISKUSTVA && ListaAutora.Any(a => a.Id == autor.Id);
        }

        public bool PostaviSefa(Autor autor) 
        {
            if (!MozeBitiSef(autor)) 
            {
                return false;
            }
            IdSefa = autor.Id;
            Sef = autor;
            return true;
        }

        public string ToCSV()
        {
            return $"{Id};{Sifra};{Naziv};{IdSefa}";
        }
        
        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 4)
            {
                Id = int.Parse(csvValues[0]);
                Sifra = csvValues[1];
                Naziv = csvValues[2];
                IdSefa = int.Parse(csvValues[3]);
            }
        }

        public override string ToString()
        {
            return $"[ID: {Id}], Sifra: {Sifra}, Naziv: {Naziv}, IdSefa: {IdSefa}";
        }
    }
}
