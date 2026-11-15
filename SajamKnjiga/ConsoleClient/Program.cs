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
        var knjigaDao = new KnjigaDAO();
        var izdavacDao = new IzdavacDAO();
        var posetilacDao = new PosetilacDAO();
        var autorKnjigaDao = new AutorKnjigaDAO();
        var kupovinaDao = new KupovinaDAO();
        var listaZeljaDao = new ListaZeljaDAO();

        var autorService = new AutorService(autorDao, adresaDao, autorKnjigaDao, izdavacDao,knjigaDao);
        var knjigaService = new KnjigaService(knjigaDao, autorDao, izdavacDao, autorKnjigaDao, kupovinaDao, listaZeljaDao);
        var izdavacService = new IzdavacService(izdavacDao, knjigaDao,autorDao,autorKnjigaDao);
        var posetilacService = new PosetilacService(posetilacDao, adresaDao, kupovinaDao, listaZeljaDao);
        var kupovinaService = new KupovinaService(kupovinaDao, posetilacDao, knjigaDao, listaZeljaDao);

        var autori = new AutorKontroler(autorService);
        var knjige = new KnjigaKontroler(knjigaService);
        var izdavaci = new IzdavacKontroler(izdavacService);
        var posetioci = new PosetilacKontroler(posetilacService);
        var kupovine = new KupovinaKontroler(kupovinaService);

        try
        {
            KonzolaUtils.Meni("SAJAM KNJIGA",
                ("Autori", autori.Meni),
                ("Knjige", knjige.Meni),
                ("Izdavači", izdavaci.Meni),
                ("Posetioci", posetioci.Meni),
                ("Kupovine", kupovine.Meni),
                ("Prikaži sve", () =>
                {
                    autori.PrikaziSve();
                    knjige.PrikaziSve();
                    izdavaci.PrikaziSve();
                    posetioci.PrikaziSve();
                    kupovine.PrikaziSve();
                }
            ));
        }
        catch (EndOfStreamException)
        {
            // ulaz je zatvoren, izlazimo
        }

        Console.WriteLine("Doviđenja!");
    }
}