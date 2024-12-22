using Domain.Models;
using Server.Services.CitanjePorukaServisi;
using Server.Services.PretragaKnjigaServisi;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server.Services.IznajmljivanjeServisi
{
    public class IznajmljivanjeServis
    {
        public void ObradiZahtev(Socket socket, string poruka, List<Iznajmljivanje> listaIznajmljivanja, List<Knjiga> listaKnjiga, TcpCitanjeServis tcpCitanjeServis, TcpSlanjeServis tcpSlanjeServis)
        {
            string[] porukaDelovi = poruka.Split(':');      // Format: IZNAJMLJIVANJE:{ID}:{NASLOV}:{AUTOR}

            if (porukaDelovi.Length != 4)       // Desio se neocekivan problem sa klijentske strane, zaustavljamo obradu
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno iznajmljivanje: Greska sa podacima.");
                return;
            }

            string naslov = porukaDelovi[2];
            string autor = porukaDelovi[3];

            int idKorisnika;

            try
            {
                idKorisnika = Int32.Parse(porukaDelovi[1]);
            }
            catch (Exception)
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno iznajmljivanje: Greska pri identifikaciji korisnika.");
                return;
            }

            (bool knjigaPostoji, Knjiga knjiga) = new PronadjiKnjiguServis().PronadjiKnjigu(listaKnjiga, naslov, autor);

            if (!knjigaPostoji)
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno iznajmljivanje: Ne posedujemo tu knjigu.");
                return;
            }
            else if (knjiga.Kolicina < 1)
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno iznajmljivanje: Nijedan primerak te knjige trenutno nije dostupan.");
                return;
            }

            string formatIznajmljivanja = $"{knjiga.Naslov} - {knjiga.Autor}";
            DateTime datumVracanja = DateTime.Now.AddDays(14);

            tcpSlanjeServis.PosaljiPoruku(socket, $"Uspesno iznajmljivanje:{formatIznajmljivanja}:\n" +
                                                  $"Datum vracanja: {datumVracanja.ToString("dd.MM.yyyy")}");

            Iznajmljivanje iznajmljivanje = new Iznajmljivanje(formatIznajmljivanja, idKorisnika, datumVracanja);
            listaIznajmljivanja.Add(iznajmljivanje);
            --knjiga.Kolicina;

            Console.WriteLine($"Korisnik [ID={idKorisnika}] je iznajmio knjigu \'{formatIznajmljivanja}\'.");
        }
    }
}
