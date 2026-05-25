using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Permisos
{
    public class Patente : IComponente
    {
        public string Nombre { get; set; }

        public Patente(string nombre)
        {
            Nombre = nombre;
        }

        public void Agregar(IComponente componente)
        {
            // una patente no puede tener hijos
        }

        public void Quitar(IComponente componente)
        {
            // no aplica
        }

        public List<IComponente> ObtenerHijos()
        {
            return new List<IComponente>();
        }
    }
}
