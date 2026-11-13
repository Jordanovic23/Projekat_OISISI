using System;
using System.Collections.Generic;
using System.Text;
using Core.Storage.Serialization;

namespace Core.Model
{
    public enum Ocena { Jedan = 1, Dva = 2, Tri = 3, Cetiri = 4, Pet =  5 }
    public class Kupovina : ISerializable
    {
        public int Id { get; set; }
        public int PosetilacId { get; set; }
        public int KnjigaId { get; set; }
        public DateOnly DatumKupovine { get; set; }
        public Ocena Ocena { get; set; }
        public string Komentar { get; set; } = "";
        public Kupovina() { }
        public Kupovina(int id, int posetilacId, int knjigaId, DateOnly datumKupovine, Ocena ocena, string komentar)
        {
            this.Id = id;
            this.PosetilacId = posetilacId;
            this.KnjigaId = knjigaId;
            this.DatumKupovine = datumKupovine;
            this.Ocena = ocena;
            this.Komentar = komentar ?? "";
        }
        public string ToCSV()
        {
            // U CSV se ocena cuva kao broj a ne kao string, zbog citljivosti i lakseg sortiranja
            string komentar = Komentar.Replace("\r", " ").Replace("\n", " ");
            return $"{Id};{PosetilacId};{KnjigaId};{DatumKupovine.ToString("yyyy-MM-dd")};{(int)Ocena};{komentar}";
        }
        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length >= 6)
            {
                Id = int.Parse(csvValues[0]);
                PosetilacId = int.Parse(csvValues[1]);
                KnjigaId = int.Parse(csvValues[2]);
                DatumKupovine = DateOnly.ParseExact(csvValues[3], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
               
                int ocenaInt = int.Parse(csvValues[4]);
                if (!Enum.IsDefined(typeof(Ocena), ocenaInt))
                    throw new FormatException("Nedozvoljena vrednost za ocenu: {ocenaInt} (dozvoljene vrednosti su 1-5)");
                Ocena = (Ocena)ocenaInt;
                // Uzimamo sve delove od 5. indeksa nadalje i spajamo ih u jedan string
                Komentar = string.Join(";", csvValues, 5, csvValues.Length - 5);
            }
        }
        public override string ToString()
        {
            return $"[ID: {Id}], Posetilac ID: {PosetilacId}, Knjiga ID: {KnjigaId}, Datum kupovine: {DatumKupovine.ToString("yyyy-MM-dd")}, Ocena: {(int)Ocena}, Komentar: {Komentar}";
        }


    }
}
