using System;
using System.Collections.Generic;
using System.IO;

namespace Services.CitanjeDatotekeServisi
{
    public class CitanjeDatotekeServis
    {
        public List<string> ProcitajIzDatoteke(string putanja)
        {
            List<string> lista = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(putanja))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lista.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska prilikom citanja datoteke \'{putanja}\', opis: {ex.Message}");
            }

            return lista;
        }
    }
}
