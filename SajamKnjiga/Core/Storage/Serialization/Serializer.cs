using System.Collections.Generic;

namespace Core.Storage.Serialization
{
    public class Serializer<T> where T : ISerializable, new()
    {
        // 1. Pretvara celu listu objekata u listu tekstualnih linija (za upis u fajl)
        public List<string> ToCSV(List<T> objects)
        {
            List<string> csvLines = new List<string>();

            foreach (var obj in objects)
            {
                // Poziva ToCSV() metodu definisanu u samom entitetu (npr. Posetilac)
                csvLines.Add(obj.ToCSV());
            }

            return csvLines;
        }

        // 2. Pretvara listu tekstualnih linija iz fajla nazad u listu objekata (deserijalizacija)
        public List<T> FromCSV(List<string> csvLines)
        {
            List<T> objects = new List<T>();

            foreach (var line in csvLines)
            {
                // Preskoči prazne linije u fajlu
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Podeli liniju na delove na osnovu separatora ';'
                string[] values = line.Split(';');

                // Kreiraj novi prazan objekat tipa T (zahvaljujući 'new()' ograničenju)
                T obj = new T();

                // Popuni njegove atribute preko FromCSV metode
                obj.FromCSV(values);

                // Dodaj ga u listu
                objects.Add(obj);
            }

            return objects;
        }
    }
}