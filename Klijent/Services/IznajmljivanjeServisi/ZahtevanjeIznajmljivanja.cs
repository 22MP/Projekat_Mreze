using Server.Services.CitanjePorukaServisi;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Klijent.Services.IznajmljivanjeServisi
{
    public class ZahtevanjeIznajmljivanja
    {
        public void ZatraziIznajmljivanje(Socket socket, int id, List<string> iznajmljeneKnjige, TcpCitanjeServis tcpCitanjeServis, TcpSlanjeServis tcpSlanjeServis)
        {
            Console.Write("Naslov knjige: ");
            string naslov = Console.ReadLine().Trim();

            Console.Write("Autor knjige: ");
            string autor = Console.ReadLine().Trim();

            tcpSlanjeServis.PosaljiPoruku(socket, $"IZNAJMLJIVANJE:{id}:{naslov}:{autor}");

            string odgovorServera = tcpCitanjeServis.ProcitajPoruku(socket);

            if (odgovorServera.StartsWith("Uspesno iznajmljivanje"))
                iznajmljeneKnjige.Add(odgovorServera.Split(':')[1]);    // Format: Uspesno iznajmljivanje:{Naslov} - {Autor}:Datum ...

            Console.WriteLine(odgovorServera);
        }
    }
}
