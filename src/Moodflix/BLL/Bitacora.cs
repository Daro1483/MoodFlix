using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class Bitacora
    {
        private MP_Bitacora _mpBitacora = new MP_Bitacora();
        public int Insertar(Services.Bitacora bitacora)
        {
            return _mpBitacora.Insert(bitacora);
        }

        public List<Services.Bitacora> Listar()
        {
            return _mpBitacora.GetAll();
        }
        public List<Services.Bitacora> Filtrar(DateTime fi, DateTime ff)
        {
            return _mpBitacora.FiltrarBitacora(fi, ff);
        }

        public List<Services.Bitacora> FiltrarAvanzado(DateTime? fechaInicio, DateTime? fechaFin, string email, string modulo)
        {
            return _mpBitacora.FiltrarAvanzado(fechaInicio, fechaFin, email, modulo);
        }

        public string Concatenar(Services.Bitacora bitacora)
        {
            return string.Concat(bitacora.ID.ToString() + bitacora.Fecha.ToString() + bitacora.Modulo.ToString() +
                                 bitacora.Operacion.ToString() + bitacora.User.ToString());
        }
    }
}
