using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Services.CitanjePorukaServisi
{
    public class UDPCitanjeServis
    {
        public string ProcitajPoruku(Socket socket, ref EndPoint posiljaocEP){
            string poruka;
            byte[] buffer = new byte[1024];

            try
            {
                int bytesReceived = socket.ReceiveFrom(buffer,ref posiljaocEP);
                poruka = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                
            }
            catch (Exception e)
            {
                poruka = String.Empty;
                
            }

            return poruka;
        }
    }
}
