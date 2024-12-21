using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Knjiga
    {

        public string Naslov {  get; set; }
        public string Autor {  get; set; }
        public int Kolicina {  get; set; }
        public Knjiga() { }

        public Knjiga(string naslov, string autor, int kolicina)
        {
            Naslov = naslov;
            Autor = autor;
            Kolicina = kolicina;
        }

        public override string ToString()
        {
            return $"{Naslov,-20} {Autor,-20} [{Kolicina}]";
        }
    }
}
