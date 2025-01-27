using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Services.SlanjePorukaServisi
{
    public class UDPSlanjeServis
    {
        public bool PosaljiPoruku(Socket socket, string poruka, EndPoint primaocEP)
        {
            bool isSent;
            byte[] binarnaPoruka = Encoding.UTF8.GetBytes(poruka);

            try
            {
                socket.SendTo(binarnaPoruka, 0, binarnaPoruka.Length, SocketFlags.None, primaocEP);
                isSent = true;
            }
            catch (Exception e)
            {
                isSent = false;
                Console.WriteLine($"Doslo je do greske: {e.Message}");
                

            }

            return isSent;
        }
    }
}
