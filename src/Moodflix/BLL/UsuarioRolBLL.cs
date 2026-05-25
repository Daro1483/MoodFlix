using System;
using System.Collections.Generic;
using DAL;
using BE;

namespace BLL
{
    public class UsuarioRolBLL
    {
        private readonly MP_UsuarioRol mapper = new MP_UsuarioRol();
        

        /// <summary>
        /// Obtiene todos los roles asignados a un usuario.
        /// </summary>
        public List<UsuarioRol> ObtenerRolesDeUsuario(int idUsuario)
        {
            List<UsuarioRol> lista = new List<UsuarioRol>();
            var todos = mapper.GetAll();

            foreach (var item in todos)
            {
                if (item.IdUsuario == idUsuario)
                    lista.Add(item);
            }

            return lista;
        }

        /// <summary>
        /// Asigna un rol a un usuario si no existe.
        /// </summary>
        public void AsignarRol(int idUsuario, int idRol)
        {
            var asignados = ObtenerRolesDeUsuario(idUsuario);

            foreach (var r in asignados)
            {
                if (r.IdRol == idRol)
                    return; // Ya existe
            }

            UsuarioRol nuevo = new UsuarioRol()
            {
                IdUsuario = idUsuario,
                IdRol = idRol
            };

            mapper.Insert(nuevo);
        }

        /// <summary>
        /// Elimina todos los roles de un usuario.
        /// </summary>
        public void EliminarRolesDeUsuario(int idUsuario)
        {
            mapper.Delete(idUsuario);
        }

        public List<UsuarioRol> ObtenerUsuariosPorRol(int idRol)
        {
            return mapper.ObtenerUsuariosPorRol(idRol);
        }



        public void EliminarRol(int idUsuario, int idRol)
        {
            var todos = mapper.GetAll();

            foreach (var item in todos)
            {
                if (item.IdUsuario == idUsuario && item.IdRol == idRol)
                {
                    mapper.Delete(item.IdUsuario, item.IdRol);
                    return;
                }
            }
        }


        /// <summary>
        /// Reemplaza los roles actuales del usuario por un nuevo conjunto.
        /// </summary>
        public void ReasignarRoles(int idUsuario, List<int> nuevosRoles)
        {
            // Elimina todo
            mapper.Delete(idUsuario);

            // Inserta nuevamente
            foreach (int idRol in nuevosRoles)
            {
                AsignarRol(idUsuario, idRol);
            }
        }

        /// <summary>
        /// Lista todas las asociaciones Usuario-Rol.
        /// </summary>
        public List<UsuarioRol> ListarTodo()
        {
            return mapper.GetAll();
        }



        /// <summary>
        /// Obtiene únicamente las FAMILIAS asignadas a un usuario.
        /// (Los IDROL de USUARIOROL SIEMPRE son familias)
        /// </summary>
        public List<Rol> GetFamiliasByUsuario(int idUsuario)
        {
            List<Rol> familias = new List<Rol>();

            // 1) Obtener relaciones usuario → rol (familia)
            var relaciones = ObtenerRolesDeUsuario(idUsuario);

            RolBLL rolBLL = new RolBLL();

            // 2) Convertir IDROL en objeto Rol
            foreach (var r in relaciones)
            {
                Rol rol = rolBLL.GetById(r.IdRol);

                // Por seguridad, filtramos que sea efectivamente FAMILIA
                if (rol != null && rol.Tipo == "FAMILIA")
                {
                    familias.Add(rol);
                }
            }

            return familias;
        }



    }
}
