using Domain.Models;
using Server.Services.PretragaKnjigaServisi;
using Services.SlanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server.Services.ProvjeraDostupnostiServisi
{
    public class ProvjeraDostupnostiKnjige
    {
        public void provjeriDostupnost(Socket socket, EndPoint posiljaocEP, UDPSlanjeServis udpSlanjeServis, string poruka, List<Knjiga> listaKnjiga)
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
                udpSlanjeServis.PosaljiPoruku(socket, "Nema dostupnih primjeraka trazene knjige.", posiljaocEP);
                return;
            }

            string odgovor = $"Trazena knjiga je dostupna.\n {knjiga}";


            udpSlanjeServis.PosaljiPoruku(socket, odgovor, posiljaocEP);
            Console.WriteLine($"ODOGOVOR SERVERA: {odgovor}");
        }
    }
}
