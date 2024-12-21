using Domain.Models;
using Server.Services.UcitavanjePodatakaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        static void Main(string[] args)
        {
            #region Inicijalizacija server socketa
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 50100);
            tcpSocket.Bind(serverEP);
            udpSocket.Bind(serverEP);

            Console.WriteLine($"PRISTUPNA UTICNICA: {serverEP}");
            Console.WriteLine($"INFO UTICNICA: {serverEP}");
            Console.WriteLine();
            #endregion

            CitanjeDatotekeServis citanjeDatServis = new CitanjeDatotekeServis();
            List<Knjiga> dostupneKnjige = new UcitavanjeKnjigaServis().UcitajKnjige(citanjeDatServis.ProcitajIzDatoteke("dostupne_knjige.txt"));
            
            foreach (Knjiga knjiga in dostupneKnjige)
                Console.WriteLine(knjiga);
        }
    }
}
