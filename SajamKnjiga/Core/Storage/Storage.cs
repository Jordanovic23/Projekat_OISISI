using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Storage.Serialization;

namespace Core.Storage
{
    public class Storage<T> where T : ISerializable, new()
    {
        private readonly string _filePath;
        private readonly Serializer<T> _serializer;

        public Storage(string fileName)
        {
            string dataFolder = NadjiDataFolder();
            Directory.CreateDirectory(dataFolder);
            _filePath = Path.Combine(dataFolder, fileName);
            _serializer = new Serializer<T>();
        }

        // Ide navise od bin/Debug/netX dok ne nadje folder sa .sln (ili .slnx) fajlom,
        // tako da GUI i konzola dele isti Data folder u korenu solution-a.
        private static string NadjiDataFolder()
        {
            DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !dir.EnumerateFiles("*.sln*").Any())
            {
                dir = dir.Parent;
            }

            // Ako solution nije pronadjen (aplikacija pokrenuta van njega), koristi folder pored .exe
            string koren = dir?.FullName ?? AppContext.BaseDirectory;
            return Path.Combine(koren, "Data");
        }

        public List<T> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            string[] lines = File.ReadAllLines(_filePath);
            return _serializer.FromCSV(new List<string>(lines));
        }

        public void Save(List<T> objects)
        {
            List<string> lines = _serializer.ToCSV(objects);
            File.WriteAllLines(_filePath, lines);
        }
    }
}