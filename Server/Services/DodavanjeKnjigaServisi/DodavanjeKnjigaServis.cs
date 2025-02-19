using Domain.Models;
using Server.Services.PretragaKnjigaServisi;
using System;
using System.Collections.Generic;

namespace Server.Services.DodavanjeKnjigaServisi
{
    public class DodavanjeKnjigaServis
    {
        public void DodajKnjigu(List<Knjiga> listaKnjiga)
        {

            Console.WriteLine("Naslov knjige: ");
            string naslov = Console.ReadLine().Trim();

            Console.WriteLine("Autor knjige: ");
            string autor = Console.ReadLine().Trim();

            int kolicina = 0;
            while (true)
            {
                Console.WriteLine("Kolicina knjige: ");

                try
                {
                    kolicina = Int32.Parse(Console.ReadLine().Trim());
                    if (kolicina < 1)
                    {
                        Console.WriteLine("\nKolicina ne moze da bude manja od 1!");
                        continue;

                    }
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("\nGreska prilikom unosenja kolicine!");

                }

            }


            (bool postoji, Knjiga knjiga) = new PronadjiKnjiguServis().PronadjiKnjigu(listaKnjiga, naslov, autor);

            if (!postoji) // Ako nova knjiga ne postoji u biblioteci dodaje se novi objekat
            {
                Knjiga novaKnjiga = new Knjiga(naslov, autor, kolicina);
                listaKnjiga.Add(novaKnjiga);
                Console.WriteLine("\nUspjesno ste dodali novu knjigu!");


            }
            else // Ako knjiga postoji povecava se kolicina
            {
                knjiga.Kolicina += kolicina;
                Console.WriteLine("\nUspjesno ste dodali novu knjigu!");

            }
        }
    }
}
