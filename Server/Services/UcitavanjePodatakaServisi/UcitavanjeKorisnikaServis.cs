using System;
using System.Collections.Generic;

namespace Server.Services.UcitavanjePodatakaServisi
{
    public class UcitavanjeKorisnikaServis
    {
        public List<int> UcitajKorisnike(List<string> listaStringova)
        {
            List<int> listaIdKorisnika = new List<int>();

            foreach (string str in listaStringova)
            {
                try
                {
                    int id = Int32.Parse(str);
                    listaIdKorisnika.Add(id);
                }
                catch (Exception)
                {
                    // Greska prilikom obradjivanje jedne linije iz datoteke -> preskacemo tu liniju
                    continue;
                }
            }

            return listaIdKorisnika;
        }
    }
}
