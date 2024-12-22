using Server.Services.CitanjePorukaServisi;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klijent.Services.IznajmljivanjeServisi
{
    public class ZahtevanjeIznajmljivanja
    {
        public void ZatraziIznajmljivanje(Socket socket, int id, List<string> iznajmljeneKnjige, TcpCitanjeServis tcpCitanjeServis, TcpSlanjeServis tcpSlanjeServis)
        {
            tcpSlanjeServis.PosaljiPoruku(socket, $"IZNAJMLJIVANJE:{id}");

            Console.Write("Naslov knjige: ");
            string naslov = Console.ReadLine().Trim();

            Console.Write("Autor knjige: ");
            string autor = Console.ReadLine().Trim();

            tcpSlanjeServis.PosaljiPoruku(socket, $"{naslov},{autor}");

            string odgovorServera = tcpCitanjeServis.ProcitajPoruku(socket);

            if (odgovorServera.StartsWith("Uspesno iznajmljivanje"))
            {
                iznajmljeneKnjige.Add(odgovorServera.Split(':')[1]);    // Format: Uspesno iznajmljivanje:{Naslov} - {Autor}
                Console.WriteLine(odgovorServera);
                Console.WriteLine(tcpCitanjeServis.ProcitajPoruku(socket));     // Prateca poruka o datumu vracanja
            }
            else
                Console.WriteLine(odgovorServera);
        }
    }
}
