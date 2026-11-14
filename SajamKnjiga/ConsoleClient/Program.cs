using Core.DAO;
using Core.Model;

class Program
{
    static void Main(string[] args)
    {
        var listaZeljaDao = new ListaZeljaDAO();
        var kupovinaDao = new KupovinaDAO();

        Console.WriteLine("=== SAJAM KNJIGA - PRIKAZ ===");

        // 1. Prikaz svih stavki sa liste želja za posetioca sa ID-jem 1
        int posetilacId = 1;
        var zeljePosetioca = listaZeljaDao.GetByPosetilacId(posetilacId);

        Console.WriteLine($"\nLista želja za posetioca ID: {posetilacId}:");
        if (zeljePosetioca.Count == 0)
        {
            Console.WriteLine("  (Prazna lista želja)");
        }
        else
        {
            foreach (var zelja in zeljePosetioca)
            {
                Console.WriteLine($" - Knjiga ID: {zelja.KnjigaId}");
            }
        }

        // 2. Prikaz kupovina za istog posetioca
        var kupovinePosetioca = kupovinaDao.GetByPosetilacId(posetilacId);

        Console.WriteLine($"\nIstorija kupovina za posetioca ID: {posetilacId}:");
        if (kupovinePosetioca.Count == 0)
        {
            Console.WriteLine("  (Nema evidentiranih kupovina)");
        }
        else
        {
            foreach (var k in kupovinePosetioca)
            {
                Console.WriteLine($" - Knjiga ID: {k.KnjigaId} | Datum: {k.DatumKupovine} | Ocena: {k.Ocena}");
            }
        }
    }
}