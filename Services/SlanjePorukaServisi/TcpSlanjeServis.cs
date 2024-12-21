using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Services.PisanjePorukaServisi
{
    public class TcpSlanjeServis
    {
        public bool PosaljiPoruku(Socket socket, string poruka)
        {
            bool isSent;
            byte[] binarnaPoruka = Encoding.UTF8.GetBytes(poruka);

            try
            {
                socket.Send(binarnaPoruka, 0, binarnaPoruka.Length, SocketFlags.None);
                isSent = true;
            }
            catch (Exception)
            {
                isSent = false;
            }

            return isSent;
        }
    }
}
