using System;
using System.Collections.Generic;
using DAL;
using BE;

namespace BLL
{
    public class RolHijoBLL
    {
        private readonly MP_RolHijo mapper = new MP_RolHijo();


        public List<RolHijo> ObtenerHijos(int idRolPadre)
        {
            List<RolHijo> lista = new List<RolHijo>();
            var todos = mapper.GetAll();

            foreach (var item in todos)
            {
                if (item.IdRolPadre == idRolPadre)
                    lista.Add(item);
            }

            return lista;
        }


        /// <summary>
        /// Inserta un rol hijo si no existe.
        /// </summary>
        public void InsertarRolHijo(int idRolPadre, int idRolHijo)
        {
            var hijosActuales = ObtenerHijos(idRolPadre);

            foreach (var h in hijosActuales)
            {
                if (h.IdRolHijo == idRolHijo)
                    return; // Ya existe
            }

            RolHijo nuevo = new RolHijo()
            {
                IdRolPadre = idRolPadre,
                IdRolHijo = idRolHijo
            };

            mapper.Insert(nuevo);
        }

        /// <summary>
        /// Elimina todos los hijos asociados al rol padre.
        /// </summary>
        public void EliminarHijosDeRol(int idRolPadre)
        {
            mapper.Delete(idRolPadre);
        }

        /// <summary>
        /// Lista todas las asociaciones rol-padre / rol-hijo.
        /// </summary>
        public List<RolHijo> ListarTodo()
        {
            return mapper.GetAll();
        }

        public void ReemplazarHijos(int idRolPadre, List<int> nuevosHijos)
        {
            // 1. Eliminar todos los actuales
            mapper.Delete(idRolPadre);

            // 2. Insertar los nuevos
            foreach (int idRolHijo in nuevosHijos)
            {
                InsertarRolHijo(idRolPadre, idRolHijo);
            }
        }

        public void EliminarHijosDeFamilia(int idFamilia)
        {
            mapper.EliminarHijosDeFamilia(idFamilia);
        }

        public void EliminarHijoPuntual(int idPadre, int idHijo)
        {
            mapper.DeletePuntual(idPadre, idHijo);
        }

    }
}
