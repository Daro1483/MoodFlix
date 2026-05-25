using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using Interfaces;

namespace Services
{
    public class Bitacora: IEntity
    {

        private int _id;
        public int ID
        {
            get => _id;
            set => _id = value;
        }

        private DateTime _date;
        public DateTime Fecha
        {
            get=>_date; 
            set=>_date = value;
        }

        private Usuario _user;
        public Usuario User
        {
            get=>_user; 
            set=>_user=value;
        }

        private TipoModulo _modulo;
        public TipoModulo Modulo
        {
            get => _modulo; 
            set => _modulo = value;
        }

        private TipoOperacion _operacion;
        public TipoOperacion Operacion
        {
            get => _operacion; 
            set => _operacion=value;
        }


    }
}
