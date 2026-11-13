using System;
using Core.Storage.Serialization;

namespace Core.Model
{
    public class Autor : ISerializable
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateOnly DatumRodjenja { get; set; }
        public int AdresaId { get; set; } // Strani ključ ka adresi
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string BrojLK { get; set; }
        public int GodineIskustva { get; set; }

        public Autor() { }

        public Autor(int id, string ime, string prezime, DateOnly datumRodjenja, int adresaId, string telefon, string email, string brojLK, int godineIskustva)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            AdresaId = adresaId;
            Telefon = telefon;
            Email = email;
            BrojLK = brojLK;
            GodineIskustva = godineIskustva;
        }

        public string ToCSV()
        {
            return $"{Id};{Ime};{Prezime};{DatumRodjenja:yyyy-MM-dd};{AdresaId};{Telefon};{Email};{BrojLK};{GodineIskustva}";
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 9)
            {
                Id = int.Parse(csvValues[0]);
                Ime = csvValues[1];
                Prezime = csvValues[2];
                DatumRodjenja = DateOnly.Parse(csvValues[3]);
                AdresaId = int.Parse(csvValues[4]);
                Telefon = csvValues[5];
                Email = csvValues[6];
                BrojLK = csvValues[7];
                GodineIskustva = int.Parse(csvValues[8]);
            }
        }

        public override string ToString()
        {
            return $"[ID: {Id}] {Ime} {Prezime} | LK: {BrojLK} | Adresa ID: {AdresaId}";
        }
    }
}