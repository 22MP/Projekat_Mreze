using Services.CitanjePorukaServisi;
using Services.SlanjePorukaServisi;
using System;
using System.Net;
using System.Net.Sockets;

namespace Klijent.Services.PregledDostupnihServisi
{
    public class ZahtijevanjePregledaDostupnih
    {
        public void pregledDostupnihKnjiga(Socket socket, EndPoint serverEP, UDPCitanjeServis udpCitanjeServis, UDPSlanjeServis udpSlanjeServis)
        {
            udpSlanjeServis.PosaljiPoruku(socket, "PREGLED DOSTUPNIH KNJIGA", serverEP);

            string odgovor = udpCitanjeServis.ProcitajPoruku(socket, ref serverEP);

            Console.WriteLine(odgovor);

        }
    }
}
