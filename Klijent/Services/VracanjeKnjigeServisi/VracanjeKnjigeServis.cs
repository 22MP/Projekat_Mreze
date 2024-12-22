using Server.Services.CitanjePorukaServisi;
using Services.PisanjePorukaServisi;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Klijent.Services.VracanjeKnjigeServisi
{
    public class VracanjeKnjigeServis
    {
        public void VratiKnjigu(int id, Socket socket, List<string> iznajmljeneKnjige, TcpCitanjeServis tcpCitanjeServis, TcpSlanjeServis tcpSlanjeServis)
        {
            string knjiga = IzaberiKnjigu(iznajmljeneKnjige);

            if (knjiga == string.Empty)
                return;

            tcpSlanjeServis.PosaljiPoruku(socket, $"VRACANJE:{id}:{knjiga}");

            string odgovor = tcpCitanjeServis.ProcitajPoruku(socket);

            if (odgovor.StartsWith("Uspesno vracanje knjige"))
            {
                iznajmljeneKnjige.Remove(knjiga);
            }

            Console.WriteLine(odgovor);
        }

        private string IzaberiKnjigu(List<string> iznajmljeneKnjige)
        {
            if (iznajmljeneKnjige.Count < 1)
            {
                Console.WriteLine("Vi nemate iznajmljenih knjiga.");
                return string.Empty;
            }

            Console.WriteLine("Knjige koje iznajmljujete: ");
            for (int i = 0; i < iznajmljeneKnjige.Count; ++i)
                Console.WriteLine($"{i + 1}. {iznajmljeneKnjige[i]}");

            int broj;
            do
            {
                try
                {
                    Console.Write("Unesite broj knjige koju zelite da vratite (0 za izlaz): ");
                    broj = Int32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    broj = -1;
                }
            } while (broj < 0 || broj > iznajmljeneKnjige.Count);

            return broj == 0 ? string.Empty : iznajmljeneKnjige[broj - 1];
        }
    }
}
