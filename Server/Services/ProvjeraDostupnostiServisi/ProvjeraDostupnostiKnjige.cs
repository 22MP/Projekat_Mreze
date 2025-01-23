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
using Server.Services.PretragaKnjigaServisi;
using Services.PisanjePorukaServisi;

namespace Server.Services.ProvjeraDostupnostiServisi
{
    public class ProvjeraDostupnostiKnjige
    {
        public void provjeriDostupnost(Socket socket, EndPoint posiljaocEP, UDPSlanjeServis udpSlanjeServis,string poruka, List<Knjiga> listaKnjiga)
        {
            string[] dijeloviPoruke = poruka.Split(':');      // Format poruke: PROVJERI DOSTUPNOST:{naslov}:{autor}

            if (dijeloviPoruke.Length != 3)       // Poruka nije primljena u odgovarajucem formatu
            {
                udpSlanjeServis.PosaljiPoruku(socket, "Greska! Zahtjev nije primljen u odgovarajucem formatu", posiljaocEP);
                return;
            }

            string naslov = dijeloviPoruke[1];
            string autor = dijeloviPoruke[2];

            

            (bool postoji, Knjiga knjiga) = new PronadjiKnjiguServis().PronadjiKnjigu(listaKnjiga, naslov, autor); // Trazi knjigu u listi dostupnih, ne provjerava kolicinu

            if (!postoji)
            {
                udpSlanjeServis.PosaljiPoruku(socket, "Ne posjedujemo trazenu knjigu u nasoj biblioteci.", posiljaocEP);
                return;
            }
            else if (knjiga.Kolicina < 1)
            {
                udpSlanjeServis.PosaljiPoruku(socket, "Nijedan primerak trazene knjige trenutno nije dostupan.", posiljaocEP);
                return;
            }

            string odgovor = $"Knjiga: {knjiga.Naslov} - {knjiga.Autor} je dostupna.\nBroj primjeraka:{knjiga.Kolicina}.";


            udpSlanjeServis.PosaljiPoruku(socket, odgovor, posiljaocEP);
            Console.WriteLine($"ODOGOVOR: \'{odgovor}\'.");
        }
    }
}
