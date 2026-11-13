using System;
using System.Collections.Generic;
using Core.Storage.Serialization;

namespace Core.Model
{
    public class Knjiga : ISerializable
    {
        public int Id { get; set; }
        public string Sifra { get; set; } // ISBN
        public string Naziv { get; set; }
        public string Zanr { get; set; }
        public int GodinaIzdanja { get; set; }
        public double Cena { get; set; }
        public int BrojStrana { get; set; }
        public int IzdavacId { get; set; } // Strani ključ ka izdavaču (One-to-Many)

        // Memorijska lista autora (Many-to-Many veza preko spojne klase)
        public List<Autor> Autori { get; set; } = new List<Autor>();

        public Knjiga() { }

        public Knjiga(int id, string sifra, string naziv, string zanr, int godinaIzdanja, double cena, int brojStrana, int izdavacId)
        {
            Id = id;
            Sifra = sifra;
            Naziv = naziv;
            Zanr = zanr;
            GodinaIzdanja = godinaIzdanja;
            Cena = cena;
            BrojStrana = brojStrana;
            IzdavacId = izdavacId;
            Autori = new List<Autor>();
        }

        public string ToCSV()
        {
            // Autori se ne upisuju ovde, već se čuvaju u spojnoj datoteci (autor_knjiga.csv)
            return $"{Id};{Sifra};{Naziv};{Zanr};{GodinaIzdanja};{Cena};{BrojStrana};{IzdavacId}";
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 8)
            {
                Id = int.Parse(csvValues[0]);
                Sifra = csvValues[1];
                Naziv = csvValues[2];
                Zanr = csvValues[3];
                GodinaIzdanja = int.Parse(csvValues[4]);
                Cena = double.Parse(csvValues[5]);
                BrojStrana = int.Parse(csvValues[6]);
                IzdavacId = int.Parse(csvValues[7]);
                // Lista autora se popunjava naknadno u memoriji preko DAO sloja i spojne datoteke
            }
        }

        public override string ToString()
        {
            return $"[ID: {Id}] [{Sifra}] {Naziv} ({Zanr}) - Godina: {GodinaIzdanja}, Cena: {Cena} RSD | Broj autora: {Autori.Count}";
        }
    }
}