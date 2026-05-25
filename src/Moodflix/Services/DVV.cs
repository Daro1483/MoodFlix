using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DVV
    {
        private string _tabla;

        public string Tabla
        {
            get { return _tabla; }
            set { _tabla = value; }
        }

        private int _columna;

        public int Columna
        {
            get { return _columna; }
            set { _columna = value; }
        }

        private string _dv;

        public string DV
        {
            get { return _dv; }
            set { _dv = value; }
        }
    }
}
