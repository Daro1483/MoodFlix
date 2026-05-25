using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ISujetoIdioma
    {
        void Suscribir(IIdiomaObserver observador);
        void Desuscribir(IIdiomaObserver observador);
        void Notificar();
    }
}
