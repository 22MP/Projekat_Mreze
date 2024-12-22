using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.PretragaKnjigaServisi
{
    public class PronadjiKnjiguServis
    {
        public (bool, Knjiga) PronadjiKnjigu(List<Knjiga> listaKnjiga, string naslov, string autor)
        {
            foreach (Knjiga knjiga in listaKnjiga)
            {
                if (string.Equals(knjiga.Naslov, naslov, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(knjiga.Autor, autor, StringComparison.OrdinalIgnoreCase) &&
                    knjiga.Kolicina > 0)
                {
                    return (true, knjiga);
                }
            }

            return (false, new Knjiga());       // Izbegavanje da se vrati null
        }
    }
}
