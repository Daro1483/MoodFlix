using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace BE
{
    public abstract class Producto: IEntity
    {
        private int _id;

        public int ID
        {
            get => _id; 
            set => _id=value;
        }


        private string _nombre;
        public string Nombre { get => _nombre; set=> _nombre=value; }

        private string _descripcion;

        public string Descripcion
        {
            get => _descripcion;
            set => _descripcion=value;
        }


        private DateTime _date;

        public DateTime Fecha
        {
            get => _date;
            set => _date=value;
        }


        private Emocion _emocion;
        public Emocion Emocion { get => _emocion; set => _emocion=value; }

        private string _uri;

        public string Uri
        {
            get=> _uri; set => _uri = value;
        }

        private float _precio;

        public float Precio
        {
            get => _precio; set => _precio = value;
        }
    }
}
