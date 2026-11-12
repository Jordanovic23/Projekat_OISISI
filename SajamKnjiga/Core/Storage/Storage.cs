using System.Collections.Generic;
using System.IO;
using Core.Storage.Serialization;

namespace Core.Storage
{
    public class Storage<T> where T : ISerializable, new()
    {
        private readonly string filePath;
        private readonly Serializer<T> serializer;

        public Storage(string fileName)
        {
            // Putanja do fajla unutar "Data" foldera 
            // (u skladu sa nefunkcionalnim zahtevima koristimo Path.Combine)
            filePath = Path.Combine("Data", fileName);
            serializer = new Serializer<T>();

            // Ako folder "Data" ne postoji, automatski ga kreiraj da aplikacija ne pukne
            Directory.CreateDirectory("Data");
        }

        // Učitavanje podataka iz fajla (deserijalizacija)
        public List<T> Load()
        {
            if (!File.Exists(filePath))
            {
                // Ako fajl još uvek ne postoji, vrati praznu listu
                return new List<T>();
            }

            string[] lines = File.ReadAllLines(filePath);
            return serializer.FromCSV(new List<string>(lines));
        }

        // Čuvanje podataka u fajl (serijalizacija)
        public void Save(List<T> objects)
        {
            List<string> lines = serializer.ToCSV(objects);
            File.WriteAllLines(filePath, lines);
        }
    }
}