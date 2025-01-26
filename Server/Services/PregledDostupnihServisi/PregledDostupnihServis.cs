using Services.CitanjePorukaServisi;
using Services.SlanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Server.Services.PregledDostupnihServisi
{
    public class PregledDostupnihServis
    {
        public void pregledDostupnihKjiga(Socket socket, EndPoint posiljaocEP, UDPSlanjeServis udpSlanjeServis, string poruka, List<Knjiga> listaKnjiga)
        {

            string odgovor = "Dostupne knjige:\n";

            foreach (Knjiga knjiga in listaKnjiga)
            {
                odgovor += $"\n {knjiga}";
            }

            udpSlanjeServis.PosaljiPoruku(socket, odgovor, posiljaocEP);
            Console.WriteLine($"ODOGOVOR: \'{odgovor}\'.");

        }
    }
}
