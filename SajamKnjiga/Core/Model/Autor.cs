using System;
using Core.Storage.Serialization;   // Za nasledjivanje interface-a

namespace Core.Model
{
    public class Autor : ISerializable
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateOnly DatumRodjenja { get; set; }
        public Adresa AdresaStanovanja { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string BrojLK { get; set; }
        public int GodineIskustva { get; set; }

        // Prazan konstruktor (obavezan za serijalizaciju)
        public Autor()
        {
            AdresaStanovanja = new Adresa();
        }

        // Konstruktor sa svim parametrima
        public Autor(string ime, string prezime, DateOnly datumRodjenja, Adresa adresaStanovanja, string telefon, string email, string brojLK, int godineIskustva)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            AdresaStanovanja = adresaStanovanja;
            Telefon = telefon;
            Email = email;
            BrojLK = brojLK;
            GodineIskustva = godineIskustva;
        }

        // Serijalizacija u CSV red (adresa je na kraju)
        public string ToCSV()
        {
            string adresaCsv = AdresaStanovanja != null ? AdresaStanovanja.ToCSV() : ";;;";
            return $"{Ime};{Prezime};{DatumRodjenja:yyyy-MM-dd};{Telefon};{Email};{BrojLK};{GodineIskustva};{adresaCsv}";
        }

        // Parsiranje iz CSV niza elemenata
        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 11)
            {
                Ime = csvValues[0];
                Prezime = csvValues[1];
                DatumRodjenja = DateOnly.Parse(csvValues[2]);
                Telefon = csvValues[3];
                Email = csvValues[4];
                BrojLK = csvValues[5];
                GodineIskustva = int.Parse(csvValues[6]);

                // Poslednja 4 polja pripadaju adresi
                AdresaStanovanja = new Adresa(csvValues[7], csvValues[8], csvValues[9], csvValues[10]);
            }
        }

        //  Metoda ToString() za lep ispis
        public override string ToString()
        {
            return $"{Ime} {Prezime} | LK: {BrojLK} | Datum rođenja: {DatumRodjenja:yyyy-MM-dd} | Tel: {Telefon} | Email: {Email} | Iskustvo: {GodineIskustva} god. | Adresa: [{AdresaStanovanja}]";
        }
    }
}