using Domain.Models;
using System;
using System.Collections.Generic;


namespace Server.Services.OdredjivanjeDuznikaServisi
{
    public class OdredjivanjeDuznikaServis
    {
        public List<Iznajmljivanje> odredjivanjeDuznika(List<Iznajmljivanje> listaIznajmljivanja)
        {

            List<Iznajmljivanje> duznici = new List<Iznajmljivanje>();

            foreach (Iznajmljivanje iz in listaIznajmljivanja)
            {

                if (iz.DatumVracanja <= DateTime.Now.Date)
                {

                    duznici.Add(iz);
                }
            }

            if (duznici.Count > 0)
            {
                Console.Write("\nID vrijednosti clanova koji su duzni da vrate iznajmljene knjige:");
                foreach (Iznajmljivanje iz in duznici)
                {
                    Console.Write($"{iz.Clan} ");
                }
                Console.WriteLine();
            }
            return duznici;
        }
    }
}
