using Domain.Models;
using Klijent.Services.PrijavaServisi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klijent
{
    public class Klijent
    {
        private static readonly string datoteka_id = "my_id.txt";
        private static int id;
        static void Main(string[] args)
        {
            #region Ucitavanje ID-a klijenta
            id = new IdUcitavanjeServis().UcitajId(datoteka_id);
            #endregion

            #region Inicijalizacija klijent socketa / Uspostavljanje veze sa serverom
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 50100);

            // tcpSocket.Connect(serverEP);
            #endregion

        }
    }
}
