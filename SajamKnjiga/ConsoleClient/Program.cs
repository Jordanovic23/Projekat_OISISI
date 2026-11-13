using Core.DAO;
using Core.Model;

Console.WriteLine("Hello, World!");

var posetilacDao = new PosetilacDAO();
posetilacDao.Add(new Posetilac(0, "Marko", "Marković", new DateOnly(2001, 4, 15), 0,
    "0641234567", "marko@mail.com", "CK-1-2026", 1, StatusPosetioca.Redovan));

foreach (Posetilac p in posetilacDao.GetAll())
    Console.WriteLine(p);