using Domain.Models;
using Server.Services.CitanjePorukaServisi;
using Server.Services.PretragaKnjigaServisi;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server.Services.VracanjeKnjigaServisi
{
    public class VracanjeKnjigeServis
    {
        public void ObradiZahtev(Socket socket, string poruka, List<Iznajmljivanje> listaIznajmljivanja, List<Knjiga> listaKnjiga, TcpCitanjeServis tcpCitanjeServis, TcpSlanjeServis tcpSlanjeServis)
        {
            string[] porukaDelovi = poruka.Split(':');      // Format: VRACANJE:{ID}:{Naslov} - {Autor}

            if (porukaDelovi.Length != 3)       // Desio se neocekivan problem sa klijentske strane, zaustavljamo obradu
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno vracanje knjige: Greska sa podacima.");
                return;
            }

            string[] knjigaPodaci = porukaDelovi[2].Split('-');
            string naslov = knjigaPodaci[0].Trim();
            string autor = knjigaPodaci[1].Trim();

            int idKorisnika;
            try
            {
                idKorisnika = Int32.Parse(porukaDelovi[1]);         // Format: IZNAJMLJIVANJE:{ID}
            }
            catch (Exception)
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno vracanje knjige: Greska pri identifikaciji korisnika.");
                return;
            }

            foreach (Iznajmljivanje iz in listaIznajmljivanja)
            {
                if (iz.Clan == idKorisnika && iz.Knjiga == porukaDelovi[2])
                {
                    listaIznajmljivanja.Remove(iz);
                    break;
                }
            }

            (bool knjigaPostoji, Knjiga knjiga) = new PronadjiKnjiguServis().PronadjiKnjigu(listaKnjiga, naslov, autor);

            if (!knjigaPostoji)         // Jedino u slucaju nekog problema sa bazom podataka
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno vracanje knjige: Greska sa podacima.");
                return;
            }

            ++knjiga.Kolicina;
            tcpSlanjeServis.PosaljiPoruku(socket, "Uspesno vracanje knjige.");
            Console.WriteLine($"Korisnik [ID={idKorisnika}] je vratio knjigu \'{porukaDelovi[2]}\'.");
        }
    }
}
