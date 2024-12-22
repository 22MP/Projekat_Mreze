using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Services.CuvanjePodatakaServisi
{
    public class CuvanjeKorisnikaServis
    {
        public void SacuvajKorisnike(string putanja, List<int> listaIdKorisnika)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(putanja))
                {
                    foreach (int id in listaIdKorisnika)
                    {
                        sw.WriteLine(id);
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
