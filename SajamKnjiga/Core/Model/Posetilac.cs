using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{

    public enum StatusPosetioca { Redovan, VIP, }
    public class Posetilac
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public DateOnly DatumRodjenja { get; set; }
        public int AdresaId { get; set; }
        public string KontaktTelefon { get; set; } = "";
        public string Email { get; set; } = "";
        public string BrojClanskeKarte { get; set; } = "";
        public int TrenutnaGodinaClanstva { get; set; }
        public StatusPosetioca Status { get; set; }
        public List<int> ListaZelja { get; set; } = new List<int>(); // Lista ID-ova knjiga
    }
}
