using Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.UcitavanjePodatakaServisi
{
    public class UcitavanjeIznajmljivanjaServis
    {
        public List<Iznajmljivanje> UcitajIznajmljivanja(List<string> listaStringova)
        {
            List<Iznajmljivanje> listaIznajmljivanja = new List<Iznajmljivanje>();

            foreach (string str in listaStringova)
            {
                try
                {
                    string[] delovi = str.Split(',');
                    DateTime datumVracanja = DateTime.ParseExact(delovi[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    Iznajmljivanje iznajmljivanje = new Iznajmljivanje(delovi[0], Int32.Parse(delovi[1]), datumVracanja);
                    listaIznajmljivanja.Add(iznajmljivanje);
                }
                catch (Exception)
                {
                    // Greska prilikom obradjivanje jedne linije iz datoteke -> preskacemo tu liniju
                    continue;
                }
            }

            return listaIznajmljivanja;
        }
    }
}
