using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
