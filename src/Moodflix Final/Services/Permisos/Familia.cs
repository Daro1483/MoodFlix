using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Permisos
{
    public class Familia : IComponente
    {
        public string Nombre { get; set; }

        private List<IComponente> hijos = new List<IComponente>();

        public Familia(string nombre)
        {
            Nombre = nombre;
        }

        public void Agregar(IComponente componente)
        {
            hijos.Add(componente);
        }

        public void Quitar(IComponente componente)
        {
            hijos.Remove(componente);
        }

        public List<IComponente> ObtenerHijos()
        {
            return hijos;
        }
    }
}
