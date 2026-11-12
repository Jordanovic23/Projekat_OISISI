using Core.Storage.Serialization;

namespace Core.Model
{
    public class Adresa : ISerializable
    {
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }

        public Adresa() { }

        public Adresa(string ulica, string broj, string grad, string drzava)
        {
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }

        public string ToCSV()
        {
            return $"{Ulica};{Broj};{Grad};{Drzava}";
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 4)
            {
                Ulica = csvValues[0];
                Broj = csvValues[1];
                Grad = csvValues[2];
                Drzava = csvValues[3];
            }
        }
    }
}