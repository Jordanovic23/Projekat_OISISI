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
        public int IzdavacId { get; set; } // Veza sa izdavačem (One-to-Many)

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
        }

        public string ToCSV()
        {
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
            }
        }

        public override string ToString()
        {
            return $"[ID: {Id}] [{Sifra}] {Naziv} ({Zanr}) - Cena: {Cena} RSD";
        }
    }
}