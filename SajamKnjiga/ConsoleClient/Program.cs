using System.Text;
using ConsoleClient.Controllers;
using ConsoleClient.Utils;
using Core.DAO;
using Core.Services;

class Program
{
    static void Main(string[] args)
    {
        // Da se č, ć, š, đ, ž ispravno prikazuju i unose
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Svaki DAO se pravi SAMO JEDNOM i deli izmedju servisa
        var adresaDao = new AdresaDAO();
        var autorDao = new AutorDAO();
        var izdavacDao = new IzdavacDAO();
        var autorKnjigaDao = new AutorKnjigaDAO();

        var autorService = new AutorService(autorDao, adresaDao, autorKnjigaDao, izdavacDao);
        var autori = new AutorController(autorService);

        try
        {
            KonzolaUtils.Meni("SAJAM KNJIGA",
                ("Autori", autori.Meni));
        }
        catch (EndOfStreamException)
        {
            // ulaz je zatvoren, izlazimo
        }

        Console.WriteLine("Doviđenja!");
    }
}