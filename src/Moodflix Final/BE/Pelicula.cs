using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Pelicula: Producto
    {
        private string _genero;

        public string Genero
        {
            get { return _genero; } set { _genero = value; }

        }

        private string _director;

        public string Director
        {
            get => _director; set { _director = value; }
        }
    }
}
