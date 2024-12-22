using Server.Services.CitanjePorukaServisi;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Klijent.Services.PrijavljivanjeServisi
{
    public class PrijavaServis
    {
        public (bool, int) PrijaviSe(int id, Socket socket, TcpCitanjeServis tcpCitanjeServis, TcpSlanjeServis tcpSlanjeServis)
        {
            bool isSent = tcpSlanjeServis.PosaljiPoruku(socket, $"PRIJAVA:{id}");
            if (!isSent)
            {
                return (false, -1);
            }

            string podaciOPrijavi = tcpCitanjeServis.ProcitajPoruku(socket);

            try
            {
                id = Int32.Parse(podaciOPrijavi.Split(':')[1]);     // Format: Uspesna prijava. ID:{ID}
            }
            catch (Exception)
            {
                return (false, -1);
            }

            Console.WriteLine(podaciOPrijavi);
            return (true, id);
        }
    }
}
