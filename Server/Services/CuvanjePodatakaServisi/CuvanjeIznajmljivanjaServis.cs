using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.CuvanjePodatakaServisi
{
    public class CuvanjeIznajmljivanjaServis
    {
        public void SacuvajIznajmljivanja(string putanja, List<Iznajmljivanje> listaIznajmljivanja)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(putanja))
                {
                    foreach (Iznajmljivanje iz in listaIznajmljivanja)
                    {
                        string csvForamt = $"{iz.Knjiga},{iz.Clan},{iz.DatumVracanja.ToString("dd.MM.yyyy")}";
                        sw.WriteLine(csvForamt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska prilikom upisivanja u datoteku \'{putanja}\', opis: {ex.Message}");
            }
        }
    }
}
