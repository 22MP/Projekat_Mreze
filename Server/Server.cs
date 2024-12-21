using Domain.Models;
using Server.Services.CuvanjePodatakaServisi;
using Server.Services.UcitavanjePodatakaServisi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private static readonly string datoteka_knjige = "dostupne_knjige.txt";
        private static readonly string datoteka_iznajmljivanja = "trenutna_iznajmljivanja.txt";
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

            #region Ucitavanje podataka iz datoteka
            CitanjeDatotekeServis citanjeDatServis = new CitanjeDatotekeServis();
            List<Knjiga> listaKnjiga = new UcitavanjeKnjigaServis().UcitajKnjige(citanjeDatServis.ProcitajIzDatoteke(datoteka_knjige));
            List<Iznajmljivanje> listaIznajmljivanja = new UcitavanjeIznajmljivanjaServis().UcitajIznajmljivanja(citanjeDatServis.ProcitajIzDatoteke(datoteka_iznajmljivanja));
            #endregion

            #region Cuvanje izmenjenih podataka u datoteke
            new CuvanjeKnjigaServis().SacuvajKnjige(datoteka_knjige, listaKnjiga);
            new CuvanjeIznajmljivanjaServis().SacuvajIznajmljivanja(datoteka_iznajmljivanja, listaIznajmljivanja);
            #endregion
        }
    }
}
