namespace Core.Storage.Serialization
{
    public interface ISerializable
    {
        // Metoda koja pretvara objekat u tekstualni red za CSV fajl
        string ToCSV();

        // Metoda koja prima iseckane delove teksta iz fajla i popunjava atributima objekat
        void FromCSV(string[] csvValues);
    }
}