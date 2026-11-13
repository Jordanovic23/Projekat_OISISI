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
        public Izdavac Izdavac { get; set; } // Memorijska referenca na izdavača

        // Memorijske liste u skladu sa zvaničnom specifikacijom:
        public List<Autor> Autori { get; set; } = new List<Autor>();
        public List<Posetilac> PosetiociKupci { get; set; } = new List<Posetilac>(); // Lista posetilaca koji su je kupili
        public List<Posetilac> ListaZelja { get; set; } = new List<Posetilac>(); // Lista posetilaca koji je imaju na listi želja

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
            PosetiociKupci = new List<Posetilac>();
            ListaZelja = new List<Posetilac>();
        }

        public string ToCSV()
        {
            // U CSV fajl idu samo osnovni atributi i strani ključ izdavača
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
                // Reference i liste se popunjavaju naknadno u memoriji preko DAO sloja i spojnih datoteka
            }
        }

        public override string ToString()
        {
            return $"[ID: {Id}] [{Sifra}] {Naziv} ({Zanr}) - {Cena} RSD | Autora: {Autori.Count} | Kupljena: {PosetiociKupci.Count} | Na želji: {ListaZelja.Count}";
        }
    }
}