using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Libro: Producto
    {
        private string _autor;

        public string Autor
        {
            get => _autor; set => _autor = value;
        }

        private string _editorial;

        public string Editorial
        {
            get => _editorial; set => _editorial = value;
        }
    }
}
