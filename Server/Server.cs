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
        private static bool shouldStop = false;     // Informacija da se server treba zaustaviti
        static void Main(string[] args)
        {
            Console.WriteLine("Server je poceo sa radom.");
            Console.WriteLine();

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

            #region Slusanje socketa
            Console.CancelKeyPress += new ConsoleCancelEventHandler(ObradiCancelKeyPress);
            while (!shouldStop)
            {
                
            }
            #endregion

            #region Cuvanje izmenjenih podataka u datoteke
            new CuvanjeKnjigaServis().SacuvajKnjige(datoteka_knjige, listaKnjiga);
            new CuvanjeIznajmljivanjaServis().SacuvajIznajmljivanja(datoteka_iznajmljivanja, listaIznajmljivanja);
            #endregion

            Console.WriteLine();
            Console.WriteLine("Server je stabilno zavrsio sa radom.");
        }

        static void ObradiCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            shouldStop = true;
            args.Cancel = true;    // Sprecavamo server da se nasilno zatvori, zelimo da prvo sacuva podatke
        }
    }
}
