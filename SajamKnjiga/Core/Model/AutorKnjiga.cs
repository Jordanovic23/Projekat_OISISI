using Core.Storage.Serialization;

namespace Core.Model
{
    public class AutorKnjiga : ISerializable
    {
        public int AutorId { get; set; }
        public int KnjigaId { get; set; }

        public AutorKnjiga() { }

        public AutorKnjiga(int autorId, int knjigaId)
        {
            AutorId = autorId;
            KnjigaId = knjigaId;
        }

        public string ToCSV()
        {
            return $"{AutorId};{KnjigaId}";
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 2)
            {
                AutorId = int.Parse(csvValues[0]);
                KnjigaId = int.Parse(csvValues[1]);
            }
        }

        public override string ToString()
        {
            return $"Autor ID: {AutorId} <-> Knjiga ID: {KnjigaId}";
        }
    }
}