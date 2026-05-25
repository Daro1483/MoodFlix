using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Permisos
{
    public interface IComponente
    {
        string Nombre { get; set; }
        void Agregar(IComponente componente);
        void Quitar(IComponente componente);
        List<IComponente> ObtenerHijos();
    }
}
