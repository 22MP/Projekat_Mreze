using Services.CitanjePorukaServisi;
using Services.SlanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Server.Services.PregledDostupnihServisi
{
    public class PregledDostupnihServis
    {
        public void pregledDostupnihKjiga(Socket socket, EndPoint posiljaocEP, UDPSlanjeServis udpSlanjeServis, string poruka, List<Knjiga> listaKnjiga)
        {
            string odgovor ="";
            if (listaKnjiga.Count == 0) //Nismo dodali knjige u bibloteku.
            {
                odgovor = "Trenutno nema dostupnih knjiga u bibloteci.\n";
            }
            else
            {
                Console.WriteLine(listaKnjiga.Count);
                odgovor = "Dostupne knjige:\n";

                foreach (Knjiga knjiga in listaKnjiga)
                {
                    if (knjiga.Kolicina > 0)
                    {
                        odgovor += $"\n {knjiga}";
                    }
                }

            }
            if(odgovor.Equals("Dostupne knjige:\n")) // Dostupna kolicina svih knjiga je nula.
            {
                odgovor = "Trenutno nema dostupnih knjiga u bibloteci.\n";
            }

            udpSlanjeServis.PosaljiPoruku(socket, odgovor, posiljaocEP);
            Console.WriteLine($"ODOGOVOR: \'{odgovor}\'.");

        }
    }
}
