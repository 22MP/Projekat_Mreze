using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klijent.Services.PrijavaServisi
{
    public class IdUcitavanjeServis
    {
        public int UcitajId(string putanja)
        {
            if (!File.Exists(putanja))
                return -1;              // Nepostojeci ID -> Znaci da je ovo novi klijent

            int id;

            try
            {
                id = Int32.Parse(File.ReadAllText(putanja));
            }
            catch (Exception)
            {
                id = -1;
            }

            return id;
        }
    }
}
