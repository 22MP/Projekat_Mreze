using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Iznajmljivanje
    {
        public string Knjiga {  get; set; }
        public int Clan {  get; set; }
        public DateTime DatumVracanja {  get; set; }

        public Iznajmljivanje() { }
        public Iznajmljivanje(string knjiga, int clan, DateTime datumVracanja)
        {
            Knjiga = knjiga;
            Clan = clan;
            DatumVracanja = datumVracanja;
        }

        public override string ToString()
        {
            return $"Clan ID={Clan} je iznajmio \'{Knjiga}\' sa rokom vracanja do {DatumVracanja.ToString("dd/MM/yyyy")}";
        }
    }
}
