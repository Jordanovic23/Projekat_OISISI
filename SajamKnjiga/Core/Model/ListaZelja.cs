using System;
using Core.Storage.Serialization;

namespace Core.Model
{
    public class ListaZelja : ISerializable
    {
        public int Id { get; set; }
        public int PosetilacId { get; set; }
        public int KnjigaId { get; set; }

        public ListaZelja() { }

        public ListaZelja(int id, int posetilacId, int knjigaId)
        {
            Id = id;
            PosetilacId = posetilacId;
            KnjigaId = knjigaId;
        }

        public string ToCSV()
        {
            return $"{Id};{PosetilacId};{KnjigaId}";
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 3)
            {
                Id = int.Parse(csvValues[0]);
                PosetilacId = int.Parse(csvValues[1]);
                KnjigaId = int.Parse(csvValues[2]);
            }
        }

        public override string ToString()
        {
            return $"[Lista želja ID: {Id}] Posetilac: {PosetilacId} -> Knjiga: {KnjigaId}";
        }
    }
}