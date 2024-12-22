using Domain.Models;
using Server.Services.CitanjePorukaServisi;
using Server.Services.PretragaKnjigaServisi;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.IznajmljivanjeServisi
{
    public class IznajmljivanjeServis
    {
        public void ObradiZahtev(Socket socket, string pocetnaPoruka, List<Iznajmljivanje> listaIznajmljivanja, List<Knjiga> listaKnjiga, TcpCitanjeServis tcpCitanjeServis, TcpSlanjeServis tcpSlanjeServis)
        {
            string[] knjigaPodaci = tcpCitanjeServis.ProcitajPoruku(socket).Split(',');
            string naslov = knjigaPodaci[0];
            string autor = knjigaPodaci[1];

            int idKorisnika;

            try
            {
                idKorisnika = Int32.Parse(pocetnaPoruka.Split(':')[1]);
            }
            catch (Exception)
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno iznajmljivanje: Greska pri identifikaciji korisnika.");
                return;
            }

            (bool knjigaPostoji, Knjiga knjiga) = new PronadjiKnjiguServis().PronadjiKnjigu(listaKnjiga, naslov, autor);
            
            if (!knjigaPostoji)
            {
                tcpSlanjeServis.PosaljiPoruku(socket, "Neuspesno iznajmljivanje: Knjiga nije dostupna.");
                return;
            }

            string formatIznajmljivanja = $"{knjiga.Naslov} - {knjiga.Autor}";
            DateTime datumVracanja = DateTime.Now.AddDays(14);

            tcpSlanjeServis.PosaljiPoruku(socket, $"Uspesno iznajmljivanje:{formatIznajmljivanja}");
            tcpSlanjeServis.PosaljiPoruku(socket, $"Datum vracanja: {datumVracanja.ToString("dd.MM.yyyy")}");

            Iznajmljivanje iznajmljivanje = new Iznajmljivanje(formatIznajmljivanja, idKorisnika, datumVracanja);
            listaIznajmljivanja.Add(iznajmljivanje);
            --knjiga.Kolicina;

            Console.WriteLine($"Korisnik [ID={idKorisnika}] je iznajmio knjigu \'{formatIznajmljivanja}\'.");
        }
    }
}
