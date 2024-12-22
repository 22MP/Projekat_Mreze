using Domain.Models;
using System;
using System.Collections.Generic;

namespace Server.Services.UcitavanjePodatakaServisi
{
    public class UcitavanjeKnjigaServis
    {
        public List<Knjiga> UcitajKnjige(List<string> listaStringova)
        {
            List<Knjiga> listaKnjiga = new List<Knjiga>();

            foreach (string str in listaStringova)
            {
                try
                {
                    string[] delovi = str.Split(',');
                    Knjiga knjiga = new Knjiga(delovi[0], delovi[1], Int32.Parse(delovi[2]));
                    listaKnjiga.Add(knjiga);
                }
                catch (Exception)
                {
                    // Greska prilikom obradjivanje jedne linije iz datoteke -> preskacemo tu liniju
                    continue;
                }
            }

            return listaKnjiga;
        }
    }
}
