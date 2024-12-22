using System;
using System.Net.Sockets;
using System.Text;

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
