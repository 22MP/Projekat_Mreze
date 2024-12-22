using Domain.Models;
using Klijent.Services.IznajmljivanjeServisi;
using Klijent.Services.PrijavaServisi;
using Klijent.Services.PrijavljivanjeServisi;
using Server.Services.CitanjePorukaServisi;
using Services.CitanjeDatotekeServisi;
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
        private static readonly string datoteka_id = "moj_id.txt";
        private static readonly string datoteka_iznajmljivanja = "iznajmljene_knjige.txt";
        private static int id;
        private static bool shouldStop;
        static void Main(string[] args)
        {
            Console.WriteLine("Klijent poceo sa radom.");
            Console.WriteLine();

            #region Ucitavanje ID-a klijenta / Ucitavanje iznajmljenih knjiga klijenta
            id = new IdUcitavanjeServis().UcitajId(datoteka_id);
            List<string> iznajmljeneKnjige = new List<string>();
            if (File.Exists(datoteka_iznajmljivanja))               // Novi klijenti mozda nece imati ovu datoteku
                iznajmljeneKnjige = new CitanjeDatotekeServis().ProcitajIzDatoteke(datoteka_iznajmljivanja);
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
            ZahtevanjeIznajmljivanja zahtevanjeIznajmljivanja = new ZahtevanjeIznajmljivanja();
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

            #region Rad sa bibliotekom
            Console.CancelKeyPress += new ConsoleCancelEventHandler(ObradiCancelKeyPress);      // Handler za CTRL+C
            shouldStop = false;
            while (!shouldStop)
            {
                IspisiMeni();
                int komanda = UcitajKomandu();

                switch (komanda)
                {
                    case 1:
                        zahtevanjeIznajmljivanja.ZatraziIznajmljivanje(tcpSocket, id, iznajmljeneKnjige, tcpCitanjeServis, tcpSlanjeServis);
                        break;
                    case 5:
                        shouldStop = true;
                        break;
                }
            }
            #endregion

            #region Cuvanje podataka o iznajmljenim knjigama u datoteku
            File.WriteAllLines(datoteka_iznajmljivanja, iznajmljeneKnjige);
            #endregion

            #region Zatvaranje socketa
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
            udpSocket.Close();
            Console.WriteLine();
            Console.WriteLine("Klijent zavrsio sa radom.");
            #endregion
        }        

        private static void IspisiMeni()
        {
            Console.WriteLine("");
            Console.WriteLine("1. IZNAJMLJIVANJE KNJIGE");
            Console.WriteLine("2. VRACANJE KNJIGE");
            Console.WriteLine("3. PROVERA DOSTUPNOSTI KNJIGE");
            Console.WriteLine("4. PREGLED DOSTUPNIH KNJIGA");
            Console.WriteLine("5. ZATVARANJE APLIKACIJE");
        }

        private static int UcitajKomandu()
        {
            int komanda;

            do
            {
                Console.Write("Unesite komandu: ");
                try
                {
                    komanda = Int32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    komanda = -1;
                }
            } while (komanda < 1 || komanda > 5);

            return komanda;
        }

        static void ObradiCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            shouldStop = true;
            args.Cancel = true;    // Sprecavamo server da se nasilno zatvori, zelimo da prvo sacuva podatke
        }
    }
}
