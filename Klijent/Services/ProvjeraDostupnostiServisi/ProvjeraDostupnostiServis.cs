using Services.CitanjePorukaServisi;
using Services.SlanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klijent.Services.ProvjeraDostupnostiServisi
{
    public class ProvjeraDostupnostiServis
    {
        public void ProvjeriDostupnost(Socket socket,EndPoint serverEP, UDPCitanjeServis udpCitanjeServis,UDPSlanjeServis udpSlanjeServis)
        {
            Console.Write("Naslov knjige: ");
            string naslov = Console.ReadLine().Trim();

            Console.Write("Autor knjige: ");
            string autor = Console.ReadLine().Trim();

            udpSlanjeServis.PosaljiPoruku(socket, $"PROVJERI DOSTUPNOST:{naslov}:{autor}",serverEP);
            
            string odgovor = udpCitanjeServis.ProcitajPoruku(socket,serverEP);

            Console.WriteLine(odgovor);
        }
    }
}
