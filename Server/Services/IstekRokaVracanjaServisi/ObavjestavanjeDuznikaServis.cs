using Domain.Models;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.IstekRokaVracanjaServisi
{
    public class ObavjestavanjeDuznikaServis
    {
        public void Obavjest(Socket socket,int id,List<Iznajmljivanje> duznici,TcpSlanjeServis tcpSlanjeServis)
        {
            string poruka = "\n";
            foreach (Iznajmljivanje iz in duznici)
            {
                if (iz.Clan == id)
                {
                    Console.WriteLine($"Korisnik sa id: {id} se prijavio i duzan je knjigu: {iz.Knjiga}");
                    poruka += $"Rok za vracanje knjige {iz.Knjiga} je istekao {iz.DatumVracanja}\n";
                    
                }
            }

            tcpSlanjeServis.PosaljiPoruku(socket,poruka);
        }
    }
}
