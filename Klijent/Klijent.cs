using Domain.Models;
using Klijent.Services.PrijavaServisi;
using Klijent.Services.PrijavljivanjeServisi;
using Server.Services.CitanjePorukaServisi;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
            TcpCitanjeServis tcpCitanjeServis = new TcpCitanjeServis();
            TcpSlanjeServis tcpSlanjeServis = new TcpSlanjeServis();
            PrijavaServis prijavaServis = new PrijavaServis();
            #endregion

            #region Prijavljivanje na server
            bool uspesnaPrijava;
            (uspesnaPrijava, id) = prijavaServis.PrijaviSe(id, tcpSocket, tcpCitanjeServis, tcpSlanjeServis);

            if (!uspesnaPrijava) {
                Console.WriteLine("Klijent se gasi zbog neuspesne prijave. Probajte opet.");
                return;
            }

            File.WriteAllText(datoteka_id, $"{id}");    // Cuvamo ID u datoteku
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
