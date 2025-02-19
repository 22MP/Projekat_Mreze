using Domain.Models;
using Server.Services.CitanjePorukaServisi;
using Server.Services.CuvanjePodatakaServisi;
using Server.Services.DodavanjeKnjigaServisi;
using Server.Services.IstekRokaVracanjaServisi;
using Server.Services.IznajmljivanjeServisi;
using Server.Services.OdredjivanjeDuznikaServisi;
using Server.Services.PregledDostupnihServisi;
using Server.Services.PrijavljivanjeServisi;
using Server.Services.ProvjeraDostupnostiServisi;
using Server.Services.UcitavanjePodatakaServisi;
using Server.Services.VracanjeKnjigaServisi;
using Services.CitanjeDatotekeServisi;
using Services.CitanjePorukaServisi;
using Services.PisanjePorukaServisi;
using Services.SlanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Server
    {
        private static readonly string datoteka_knjige = "dostupne_knjige.txt";
        private static readonly string datoteka_iznajmljivanja = "trenutna_iznajmljivanja.txt";
        private static readonly string datoteka_korisnici = "korisnici_id.txt";
        private static bool shouldStop = false;     // Informacija da se server treba zaustaviti
        private static List<Socket> aktivniSocketi = new List<Socket>();   // Socketi koje treba osluskivati
        static void Main(string[] args)
        {
            Console.WriteLine("Server je poceo sa radom.");
            Console.WriteLine();


            #region Inicijalizacija server socketa
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint posiljaocEP = new IPEndPoint(IPAddress.Any, 0);
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
            List<int> listaKorisnika = new UcitavanjeKorisnikaServis().UcitajKorisnike(citanjeDatServis.ProcitajIzDatoteke(datoteka_korisnici));
            #endregion

            #region Inicijalizacija potrebnih servisa
            TcpCitanjeServis tcpCitanjeServis = new TcpCitanjeServis();
            UDPCitanjeServis udpCitanjeServis = new UDPCitanjeServis();
            TcpSlanjeServis tcpSlanjeServis = new TcpSlanjeServis();
            UDPSlanjeServis udpSlanjeServis = new UDPSlanjeServis();
            PrijavaKlijentaServis prijavaKlijenataServis = new PrijavaKlijentaServis();
            IznajmljivanjeServis iznajmljivanjeServis = new IznajmljivanjeServis();
            VracanjeKnjigeServis vracanjeKnjigeServis = new VracanjeKnjigeServis();
            ProvjeraDostupnostiKnjige provjeraDostupnostiKnjige = new ProvjeraDostupnostiKnjige();
            DodavanjeKnjigaServis dodavanjeKnjigaServis = new DodavanjeKnjigaServis();
            PregledDostupnihServis pregledDostupnihServis = new PregledDostupnihServis();
            OdredjivanjeDuznikaServis odredjivanjeDuznikaServis = new OdredjivanjeDuznikaServis();
            ObavjestavanjeDuznikaServis obavjestavanjeDuznikaServis = new ObavjestavanjeDuznikaServis();
            #endregion

            # region Dodavanje knjige u biblioteku
            //Dodavanje knjige je moguce samo na pocetku rada servera 
            while (true)
            {
                Console.WriteLine("\nDa li zelite da dodate novu knjigu u bibloteku? Da/Ne");
                string komanda = Console.ReadLine().Trim();
                if (komanda.ToLower() != "da" && komanda.ToLower() != "ne")
                {
                    Console.WriteLine("Ponudjeni odgovori su: Da/Ne");
                }
                else if (komanda.ToLower() == "da")
                {
                    dodavanjeKnjigaServis.DodajKnjigu(listaKnjiga);
                }
                else if (komanda.ToLower() == "ne")
                {
                    break;
                }

            }
            #endregion

            #region Odredjivanje duznika 
            List<Iznajmljivanje> duznici = odredjivanjeDuznikaServis.odredjivanjeDuznika(listaIznajmljivanja);
            #endregion

            #region Slusanje socketa
            Console.CancelKeyPress += new ConsoleCancelEventHandler(ObradiCancelKeyPress);      // Handler za CTRL+C

            tcpSocket.Listen(50);
            aktivniSocketi.Add(tcpSocket);
            aktivniSocketi.Add(udpSocket);

            while (!shouldStop)
            {
                List<Socket> checkRead = new List<Socket>(aktivniSocketi);   // Da li treba citati sa bilo kojeg od aktivnih socketa

                Socket.Select(checkRead, null, null, 5_000_000);   // Server svakih 5s proverava da li je vreme da se ugasi

                foreach (Socket socket in checkRead)
                {
                    if (socket == tcpSocket)
                    {
                        Socket clientSocket = tcpSocket.Accept();   // Prihvatamo zahtev za novu TCP konekciju od klijenta
                        aktivniSocketi.Add(clientSocket);
                    }
                    else if (socket == udpSocket)
                    { //UDP poruka

                        string poruka = udpCitanjeServis.ProcitajPoruku(udpSocket, ref posiljaocEP);
                        Console.WriteLine($"PORUKA KLIJENTA: {poruka}");

                        if (poruka.StartsWith("PROVJERI DOSTUPNOST:"))
                        {
                            provjeraDostupnostiKnjige.provjeriDostupnost(socket, posiljaocEP, udpSlanjeServis, poruka, listaKnjiga);
                        }
                        else if (poruka.Equals("PREGLED DOSTUPNIH KNJIGA"))
                        {
                            pregledDostupnihServis.pregledDostupnihKjiga(udpSocket, posiljaocEP, udpSlanjeServis, poruka, listaKnjiga);
                        }
                    }
                    else // TCP poruka
                    {
                        string poruka = tcpCitanjeServis.ProcitajPoruku(socket);

                        if (poruka == String.Empty)     // Taj socket je poslao poruku kao indikator da prekida konekciju
                        {
                            aktivniSocketi.Remove(socket);
                        }
                        else if (poruka.StartsWith("PRIJAVA:"))
                        {
                            (bool uspesnaPrijava, int id) = prijavaKlijenataServis.ObradiPrijavu(socket, poruka, listaKorisnika, tcpSlanjeServis, obavjestavanjeDuznikaServis, duznici);

                            if (!uspesnaPrijava)
                                aktivniSocketi.Remove(socket);

                        }
                        else if (poruka.StartsWith("IZNAJMLJIVANJE"))
                        {
                            iznajmljivanjeServis.ObradiZahtev(socket, poruka, listaIznajmljivanja, listaKnjiga, tcpCitanjeServis, tcpSlanjeServis);
                        }
                        else if (poruka.StartsWith("VRACANJE"))
                        {
                            vracanjeKnjigeServis.ObradiZahtev(socket, poruka, listaIznajmljivanja, listaKnjiga, tcpCitanjeServis, tcpSlanjeServis);
                        }
                        else
                            Console.WriteLine("Greska");
                    }

                }
            }
            #endregion

            #region Cuvanje izmenjenih podataka u datoteke
            new CuvanjeKnjigaServis().SacuvajKnjige(datoteka_knjige, listaKnjiga);
            new CuvanjeIznajmljivanjaServis().SacuvajIznajmljivanja(datoteka_iznajmljivanja, listaIznajmljivanja);
            new CuvanjeKorisnikaServis().SacuvajKorisnike(datoteka_korisnici, listaKorisnika);
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
