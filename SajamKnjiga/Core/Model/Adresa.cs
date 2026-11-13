using Core.Storage.Serialization;

namespace Core.Model
{
    public class Adresa : ISerializable
    {
        public int Id { get; set; }
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }

        public Adresa() { }

        public Adresa(int id, string ulica, string broj, string grad, string drzava)
        {
            Id = id;
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }

        public string ToCSV()
        {
            return $"{Id};{Ulica};{Broj};{Grad};{Drzava}";
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 5)
            {
                Id = int.Parse(csvValues[0]);
                Ulica = csvValues[1];
                Broj = csvValues[2];
                Grad = csvValues[3];
                Drzava = csvValues[4];
            }
        }

        public override string ToString()
        {
            return $"[ID: {Id}] {Ulica} {Broj}, {Grad}, {Drzava}";
        }
    }
}