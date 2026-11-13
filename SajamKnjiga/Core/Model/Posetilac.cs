using Core.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;

namespace Core.Model
{

    public enum StatusPosetioca { Redovan, VIP, }
    public class Posetilac : ISerializable
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public DateOnly DatumRodjenja { get; set; }
        public int AdresaId { get; set; }
        public string KontaktTelefon { get; set; } = "";
        public string Email { get; set; } = "";
        public string BrojClanskeKarte { get; set; } = "";
        public int TrenutnaGodinaClanstva { get; set; }
        public StatusPosetioca Status { get; set; }
        

        public Posetilac() { }

        public Posetilac(int id, string ime, string prezime, DateOnly datumRodjenja, int adresaId, string kontaktTelefon, string email, string brojClanskeKarte, int trenutnaGodinaClanstva, StatusPosetioca status)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            AdresaId = adresaId;
            KontaktTelefon = kontaktTelefon;
            Email = email;
            BrojClanskeKarte = brojClanskeKarte;
            TrenutnaGodinaClanstva = trenutnaGodinaClanstva;
            Status = status;
        }
        public string ToCSV()
        {
            return $"{Id};{Ime};{Prezime};{DatumRodjenja.ToString("yyyy-MM-dd")};{AdresaId};{KontaktTelefon};{Email};{BrojClanskeKarte};{TrenutnaGodinaClanstva};{Status}";
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 10)
            {
                Id = int.Parse(csvValues[0]);
                Ime = csvValues[1];
                Prezime = csvValues[2];
                DatumRodjenja = DateOnly.ParseExact(csvValues[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                AdresaId = int.Parse(csvValues[4]);
                KontaktTelefon = csvValues[5];
                Email = csvValues[6];
                BrojClanskeKarte = csvValues[7];
                TrenutnaGodinaClanstva = int.Parse(csvValues[8]);
                Status = Enum.Parse<StatusPosetioca>(csvValues[9]);

            }
        }
        public override string ToString()
        {
            return $"[ID: {Id}] {Ime} {Prezime} ({Status}), karta: {BrojClanskeKarte}, " +
                   $"rođen: {DatumRodjenja:dd.MM.yyyy}, tel: {KontaktTelefon}, email: {Email}";
        }
    }
}
