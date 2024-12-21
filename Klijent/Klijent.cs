using Domain.Models;
using Klijent.Services.PrijavaServisi;
using Services.PisanjePorukaServisi;
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
            Console.WriteLine("Klijent poceo sa radom.");
            Console.WriteLine();

            #region Ucitavanje ID-a klijenta
            id = new IdUcitavanjeServis().UcitajId(datoteka_id);
            #endregion

            #region Inicijalizacija klijent socketa / Uspostavljanje veze sa serverom
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 50100);

            tcpSocket.Connect(serverEP);
            Console.WriteLine("Klijent uspesno povezan");
            #endregion

            #region Inicijalizacija potrebnih servisa
            TcpSlanjeServis tcpSlanjeServis = new TcpSlanjeServis();
            #endregion

            #region Prijavljivanje na server
            bool isSent = tcpSlanjeServis.PosaljiPoruku(tcpSocket, $"PRIJAVA:{id}");
            if (!isSent) {
                Console.WriteLine("Klijent se gasi zbog neuspesne prijave. Probajte opet.");
                return;     // Greska prilikom slanja poruke -> onemogucena prijava -> klijent se gasi
            }
            else
                Console.WriteLine("Uspesno slanje");
            #endregion

            #region Zatvaranje socketa
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
            udpSocket.Close();
            Console.WriteLine();
            Console.WriteLine("Klijent zavrsio sa radom.");
            #endregion
        }
    }
}
