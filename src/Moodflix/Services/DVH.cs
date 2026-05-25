using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DVH
    {
        private string _tabla;

        public string Tabla
        {
            get { return _tabla; }
            set { _tabla = value; }
        }

        private int _registro;

        public int Registro
        {
            get { return _registro; }
            set { _registro = value; }
        }

        private string _dv;

        public string DV
        {
            get { return _dv; }
            set { _dv = value; }
        }
    }
}
