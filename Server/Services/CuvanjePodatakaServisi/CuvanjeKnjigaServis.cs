using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Services.CuvanjePodatakaServisi
{
    public class CuvanjeKnjigaServis
    {
        public void SacuvajKnjige(string putanja, List<Knjiga> listaKnjiga)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(putanja))
                {
                    foreach (Knjiga knjiga in listaKnjiga)
                    {
                        string csvForamt = $"{knjiga.Naslov},{knjiga.Autor},{knjiga.Kolicina}";
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
