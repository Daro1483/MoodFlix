using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace BE
{
    public class Emocion: IEntity
    {
        private int _id;
        public int ID { get => _id; set=> _id=value; }

        private string _uri;
        public string Uri { get => _uri; set => _uri=value; }

        private TipoEmocion _emocion;
        public TipoEmocion TipoEmocion { get => _emocion; set => _emocion = value; }

        public string Nombre => TipoEmocion.ToString();


        public override string ToString()
        {
            return Nombre;
        }
    }
}
