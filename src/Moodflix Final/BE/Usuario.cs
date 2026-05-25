using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace BE
{
    public class Usuario: IEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string Username { get; set; }
        public int ID { get; set; }


        // Patentes y familias ya resueltas por la capa BLL
        public List<Patente> Patentes { get; set; } = new List<Patente>();

        // Método clave
        public bool TienePatente(string nombrePatente)
        {
            return Patentes.Any(p => p.Nombre.Equals(nombrePatente, StringComparison.OrdinalIgnoreCase));
        }

        public List<Rol> Roles { get; set; } = new List<Rol>();


        public override string ToString()
        {
            return Email;
        }
    }
}
