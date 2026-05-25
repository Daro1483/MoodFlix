using System.Collections.Generic;
using BE;

namespace BLL
{
    public class PermisoBLL
    {
        private readonly UsuarioRolBLL usuarioRolBLL = new UsuarioRolBLL();
        private readonly RolBLL rolBLL = new RolBLL();
        private readonly RolHijoBLL rolHijoBLL = new RolHijoBLL();

        /// <summary>
        /// Devuelve la lista de nombres de PATENTES
        /// que tiene un usuario a partir de sus familias.
        /// </summary>
        public List<string> ObtenerPatentesDeUsuario(int idUsuario)
        {
            List<string> patentes = new List<string>();

            // 1) Obtener las familias del usuario (USUARIOROL)
            var familias = usuarioRolBLL.GetFamiliasByUsuario(idUsuario);

            // 2) Por cada familia, obtener sus hijos (patentes) desde ROLHIJO
            foreach (var fam in familias)
            {
                var hijos = rolHijoBLL.ObtenerHijos(fam.IdRol);

                foreach (var h in hijos)
                {
                    Rol patente = rolBLL.GetById(h.IdRolHijo);

                    // Solo tomamos las que sean PATENTE
                    if (patente != null && patente.Tipo == "PATENTE")
                    {
                        if (!patentes.Contains(patente.Nombre))
                        {
                            patentes.Add(patente.Nombre);
                        }
                    }
                }
            }

            return patentes;
        }
    }
}
