using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.PrijavljivanjeServisi
{
    public class PrijavaKlijentaServis
    {
        public bool ObradiPrijavu(Socket socket, string podaciZaPrijavu, List<int> listaIdKorisnika, TcpSlanjeServis tcpSlanjeServis)
        {
            int idKorisnika;

            try
            {
                idKorisnika = Int32.Parse(podaciZaPrijavu.Split(':')[1]);       // Format: PRIJAVA:{ID}
            }
            catch (Exception)
            {
                idKorisnika = -1;       // Ne mozemo obraditi dobijeni ID -> posmatramo kao da je novi korisnik
            }

            if (!listaIdKorisnika.Contains(idKorisnika))     // Novi korisnik
            {
                do
                {
                    idKorisnika = new Random().Next(100, 999);      // Dodeljujemo ID novom korisniku, koji ne sme biti duplikat
                } while (listaIdKorisnika.Contains(idKorisnika));

                listaIdKorisnika.Add(idKorisnika);
            }

            return tcpSlanjeServis.PosaljiPoruku(socket, $"Uspesna prijava. ID:{idKorisnika}");
        }
    }
}
