using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Idioma
    {
        public string Codigo { get; set; } // "ES" o "EN"
        public string Descripcion { get; set; } // "Español" o "Inglés"
        public Dictionary<string, string> Traducciones { get; set; }

        public Idioma()
        {
            Traducciones = new Dictionary<string, string>();
        }
    }
}
